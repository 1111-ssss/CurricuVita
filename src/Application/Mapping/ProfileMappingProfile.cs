using AutoMapper;
using Domain.Contracts.AttributeContracts;
using Domain.Contracts.PositionContracts;
using Domain.Contracts.UserContracts;
using Domain.Entities;
using Domain.Enums;
using Domain.Helpers;

namespace Application.Mapping;

public sealed class ProfileMappingProfile : Profile
{
    public ProfileMappingProfile()
    {
        CreateMap<User, MeDto>();

        CreateMap<UserAttributeValue, ProfileAttributeValueDto>()
            .ForCtorParam(nameof(ProfileAttributeValueDto.ValueId), o => o.MapFrom(s => (int?)s.Id))
            .ForCtorParam(nameof(ProfileAttributeValueDto.AttributeDefinitionId), o => o.MapFrom(s => s.AttributeDefinition.Id))
            .ForCtorParam(nameof(ProfileAttributeValueDto.Name), o => o.MapFrom(s => s.AttributeDefinition.Name))
            .ForCtorParam(nameof(ProfileAttributeValueDto.Category), o => o.MapFrom(s => s.AttributeDefinition.Category))
            .ForCtorParam(nameof(ProfileAttributeValueDto.Description), o => o.MapFrom(s => s.AttributeDefinition.Description))
            .ForCtorParam(nameof(ProfileAttributeValueDto.DataType), o => o.MapFrom(s => s.AttributeDefinition.DataType))
            .ForCtorParam(nameof(ProfileAttributeValueDto.Options), o => o.MapFrom(s => AttributeOptionsHelper.DeserializeOptions(s.AttributeDefinition.OptionsJson)))
            .ForCtorParam(nameof(ProfileAttributeValueDto.DropdownValue), o => o.MapFrom(s =>
                s.AttributeDefinition.DataType == AttributeDataType.Dropdown ? s.StringValue : null));

        CreateMap<Project, ProjectDto>()
            .ForCtorParam(nameof(ProjectDto.Tags), o => o.MapFrom(s => s.Tags
                .Where(t => t.Tag != null)
                .Select(t => t.Tag.Name)
                .OrderBy(n => n)
                .ToList()));

        CreateMap<CV, CvListItemDto>()
            .ForCtorParam(nameof(CvListItemDto.PositionTitle), o => o.MapFrom(s => s.Position.Title));

        CreateMap<AttributeDefinition, AttributeDto>()
            .ForCtorParam(nameof(AttributeDto.Options), o => o.MapFrom(s => AttributeOptionsHelper.DeserializeOptions(s.OptionsJson)))
            .ForCtorParam(nameof(AttributeDto.UsageCount), o => o.MapFrom(s => 0));

        CreateMap<PositionAttribute, PositionAttributeDto>()
            .ForCtorParam(nameof(PositionAttributeDto.Name), o => o.MapFrom(s => s.AttributeDefinition != null ? s.AttributeDefinition.Name : string.Empty))
            .ForCtorParam(nameof(PositionAttributeDto.Category), o => o.MapFrom(s => s.AttributeDefinition != null ? s.AttributeDefinition.Category : string.Empty))
            .ForCtorParam(nameof(PositionAttributeDto.DataType), o => o.MapFrom(s => s.AttributeDefinition != null ? s.AttributeDefinition.DataType : AttributeDataType.String));

        CreateMap<PositionAccessRule, PositionAccessRuleDto>()
            .ForCtorParam(nameof(PositionAccessRuleDto.AttributeName), o => o.MapFrom(s => s.AttributeDefinition != null ? s.AttributeDefinition.Name : string.Empty));

        CreateMap<Position, PositionDetailDto>()
            .ForCtorParam(nameof(PositionDetailDto.Attributes), o => o.MapFrom(s => s.RequiredAttributes.OrderBy(a => a.Order)))
            .ForCtorParam(nameof(PositionDetailDto.Tags), o => o.MapFrom(s => s.RequiredTags
                .Where(t => t.Tag != null && !string.IsNullOrWhiteSpace(t.Tag.Name))
                .Select(t => t.Tag.Name)
                .OrderBy(n => n)
                .ToList()));
    }
}
