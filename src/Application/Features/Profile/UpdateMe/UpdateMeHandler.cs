using AutoMapper;
using Domain.Contracts.UserContracts;
using Domain.Interfaces.Database;
using Domain.Interfaces.Services;
using Domain.ResultPattern.Errors;
using Domain.ResultPattern.Result;
using MediatR;

namespace Application.Features.Profile.UpdateMe;

public class UpdateMeHandler : IRequestHandler<UpdateMeCommand, Result<MeDto>>
{
    private readonly IUserRepository _users;
    private readonly IFileStorageService _files;
    private readonly IMapper _mapper;

    public UpdateMeHandler(
        IUserRepository users,
        IFileStorageService files,
        IMapper mapper
    )
    {
        _users = users;
        _files = files;
        _mapper = mapper;
    }

    public async Task<Result<MeDto>> Handle(UpdateMeCommand request, CancellationToken cancellationToken)
    {
        var user = await _users.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
        {
            return Result<MeDto>.Failure(Errors.UserNotFound);
        }

        var oldAvatarPublicId = user.AvatarPublicId;
        var avatarChanged = !string.Equals(user.AvatarUrl, request.AvatarUrl, StringComparison.Ordinal);

        user.FirstName = request.FirstName.Trim();
        user.LastName = request.LastName.Trim();
        user.Location = request.Location?.Trim() ?? string.Empty;
        user.AvatarUrl = string.IsNullOrWhiteSpace(request.AvatarUrl) ? null : request.AvatarUrl.Trim();
        user.AvatarPublicId = string.IsNullOrWhiteSpace(request.AvatarPublicId) ? null : request.AvatarPublicId.Trim();
        user.UpdatedAt = DateTime.UtcNow;

        var saved = await _users.TrySaveWithConcurrencyAsync(user, request.Version, cancellationToken);
        if (!saved)
        {
            return Result<MeDto>.Failure(Errors.ConcurrencyConflict);
        }

        if (avatarChanged && !string.IsNullOrWhiteSpace(oldAvatarPublicId))
        {
            await _files.DeleteAvatar(oldAvatarPublicId, cancellationToken);
        }

        var reloaded = await _users.GetByIdAsync(request.UserId, cancellationToken);
        return reloaded is null
            ? Result<MeDto>.Failure(Errors.UserNotFound)
            : Result<MeDto>.Success(_mapper.Map<MeDto>(reloaded));
    }
}
