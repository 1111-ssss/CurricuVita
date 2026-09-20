using AutoMapper;
using Domain.Contracts.UserContracts;
using Domain.Entities;

namespace Application.Mapping;

public sealed class CvMappingProfile : Profile
{
    public CvMappingProfile()
    {
        CreateMap<CV, CvListItemDto>()
            .ForCtorParam(nameof(CvListItemDto.PositionTitle), o => o.MapFrom(s => s.Position.Title))
            .ForCtorParam(nameof(CvListItemDto.PositionId), o => o.MapFrom(s => s.PositionId))
            .ForCtorParam(nameof(CvListItemDto.Status), o => o.MapFrom(s => s.Status))
            .ForCtorParam(nameof(CvListItemDto.IsHidden), o => o.MapFrom(s => false));
    }
}
