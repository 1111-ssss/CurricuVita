using AutoMapper;
using Domain.Contracts.AttributeContracts;
using Domain.Entities;
using Domain.Helpers;

namespace Application.Mapping;

public sealed class AttributeMappingProfile : Profile
{
    public AttributeMappingProfile()
    {
        CreateMap<AttributeDefinition, AttributeDto>()
            .ForCtorParam(nameof(AttributeDto.Options), o => o.MapFrom(s => AttributeOptionsHelper.DeserializeOptions(s.OptionsJson)))
            .ForCtorParam(nameof(AttributeDto.UsageCount), o => o.MapFrom(s => 0));
    }
}
