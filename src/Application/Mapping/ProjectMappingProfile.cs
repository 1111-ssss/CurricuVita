using AutoMapper;
using Domain.Contracts.UserContracts;
using Domain.Entities;

namespace Application.Mapping;

public sealed class ProjectMappingProfile : Profile
{
    public ProjectMappingProfile()
    {
        CreateMap<Project, ProjectDto>()
            .ForCtorParam(nameof(ProjectDto.Tags), o => o.MapFrom(s => s.Tags
                .Where(t => t.Tag != null)
                .Select(t => t.Tag.Name)
                .OrderBy(n => n)
                .ToList()));
    }
}
