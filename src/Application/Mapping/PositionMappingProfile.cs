using AutoMapper;
using Domain.Contracts.PositionContracts;
using Domain.Entities;
using Domain.Enums;

namespace Application.Mapping;

public sealed class PositionMappingProfile : Profile
{
    public PositionMappingProfile()
    {
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
