using Domain.Contracts;
using Domain.Entities;
using Domain.Enums;
using Domain.Helpers;
using Domain.Interfaces.Database;
using Domain.ResultPattern.Errors;
using Domain.ResultPattern.Result;
using Domain.Specifications.Profile;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.Repositories;

public class UserAttributeValueRepository : BaseRepository<UserAttributeValue>, IUserAttributeValueRepository
{
    private readonly AppDbContext _context;

    public UserAttributeValueRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Result<List<UserAttributeValue>>> TrySaveAttributeValuesAsync(
        int userId,
        IReadOnlyList<AttributeValueInput> items,
        CancellationToken cancellationToken = default
    )
    {
        if (!await _context.Users.AnyAsync(u => u.Id == userId, cancellationToken))
        {
            return Result<List<UserAttributeValue>>.Failure(Errors.UserNotFound);
        }

        var definitionIds = items.Select(i => i.AttributeDefinitionId).Distinct().ToList();
        var definitions = await _context.AttributeDefinitions
            .Where(a => definitionIds.Contains(a.Id))
            .ToListAsync(cancellationToken);
        if (definitions.Count != definitionIds.Count)
        {
            return Result<List<UserAttributeValue>>.Failure(Errors.AttributeNotFound);
        }
        if (definitions.Any(d => d.Category == AttributeOptionsHelper.ProtectedCategory))
        {
            return Result<List<UserAttributeValue>>.Failure(Errors.AttributeProtected);
        }

        var existing = await _context.UserAttributeValues
            .Where(v => v.UserId == userId)
            .ToListAsync(cancellationToken);

        foreach (var item in items)
        {
            var definition = definitions.First(d => d.Id == item.AttributeDefinitionId);
            var check = ValidateValue(definition, item);
            if (check is not null)
            {
                return Result<List<UserAttributeValue>>.Failure(check);
            }

            if (item.ValueId is null)
            {
                if (existing.Any(v => v.AttributeDefinitionId == item.AttributeDefinitionId))
                {
                    return Result<List<UserAttributeValue>>.Failure(Errors.ValidationFailed);
                }
                var created = new UserAttributeValue
                {
                    UserId = userId,
                    AttributeDefinitionId = item.AttributeDefinitionId,
                    Version = 1,
                    UpdatedAt = DateTime.UtcNow
                };
                ApplyValue(created, definition, item);
                _context.UserAttributeValues.Add(created);
                existing.Add(created);
            }
            else
            {
                var entity = existing.FirstOrDefault(v => v.Id == item.ValueId && v.AttributeDefinitionId == item.AttributeDefinitionId);
                if (entity is null)
                {
                    return Result<List<UserAttributeValue>>.Failure(Errors.NotFound);
                }
                if (entity.Version != item.ExpectedVersion || !item.ExpectedVersion.HasValue)
                {
                    return Result<List<UserAttributeValue>>.Failure(Errors.ConcurrencyConflict);
                }
                _context.Entry(entity).Property(v => v.Version).OriginalValue = item.ExpectedVersion.Value;
                ApplyValue(entity, definition, item);
                entity.UpdatedAt = DateTime.UtcNow;
                entity.Version = item.ExpectedVersion.Value + 1;
            }
        }

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            return Result<List<UserAttributeValue>>.Failure(Errors.ConcurrencyConflict);
        }

        return Result<List<UserAttributeValue>>.Success(
            await ListAsync(new UserAttributeValuesByUserSpec(userId), cancellationToken)
        );
    }

    public async Task<Result> RemoveAttributeValueAsync(int userId, int valueId, CancellationToken cancellationToken = default)
    {
        var entity = await _context.UserAttributeValues
            .FirstOrDefaultAsync(v => v.Id == valueId && v.UserId == userId, cancellationToken);
        if (entity is null)
        {
            return Result.Failure(Errors.NotFound);
        }

        _context.UserAttributeValues.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    private static Domain.ResultPattern.Result.Error? ValidateValue(AttributeDefinition definition, AttributeValueInput item)
    {
        if (definition.DataType == AttributeDataType.Dropdown)
        {
            var options = AttributeOptionsHelper.DeserializeOptions(definition.OptionsJson);
            if (string.IsNullOrWhiteSpace(item.DropdownValue) || !options.Contains(item.DropdownValue!.Trim(), StringComparer.OrdinalIgnoreCase))
            {
                return Errors.ValidationFailed;
            }
        }
        if (definition.DataType == AttributeDataType.Numeric && definition.MinValue.HasValue && item.NumericValue.HasValue && item.NumericValue < definition.MinValue)
        {
            return Errors.ValidationFailed;
        }
        if (definition.DataType == AttributeDataType.Numeric && definition.MaxValue.HasValue && item.NumericValue.HasValue && item.NumericValue > definition.MaxValue)
        {
            return Errors.ValidationFailed;
        }
        if (definition.DataType == AttributeDataType.Period && item.PeriodStartValue.HasValue && item.PeriodEndValue.HasValue && item.PeriodEndValue < item.PeriodStartValue)
        {
            return Errors.ValidationFailed;
        }
        return null;
    }

    private static void ApplyValue(UserAttributeValue entity, AttributeDefinition definition, AttributeValueInput item)
    {
        entity.StringValue = null;
        entity.TextValue = null;
        entity.ImageValue = null;
        entity.NumericValue = null;
        entity.DateValue = null;
        entity.PeriodStartValue = null;
        entity.PeriodEndValue = null;
        entity.BooleanValue = null;

        switch (definition.DataType)
        {
            case AttributeDataType.String:
                entity.StringValue = item.StringValue?.Trim();
                break;
            case AttributeDataType.Text:
                entity.TextValue = item.TextValue;
                break;
            case AttributeDataType.Image:
                entity.ImageValue = item.ImageValue?.Trim();
                break;
            case AttributeDataType.Numeric:
                entity.NumericValue = item.NumericValue;
                break;
            case AttributeDataType.Date:
                entity.DateValue = item.DateValue;
                break;
            case AttributeDataType.Period:
                entity.PeriodStartValue = item.PeriodStartValue;
                entity.PeriodEndValue = item.PeriodEndValue;
                break;
            case AttributeDataType.Boolean:
                entity.BooleanValue = item.BooleanValue;
                break;
            case AttributeDataType.Dropdown:
                entity.StringValue = item.DropdownValue?.Trim();
                break;
        }
    }
}
