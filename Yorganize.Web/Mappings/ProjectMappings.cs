using AutoMapper;
using Yorganize.Domain.Models;
using Yorganize.Web.Models;

namespace Yorganize.Web.Mappings
{
    public class ProjectMappings : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Project, ProjectModel>()
                  .ForMember(m => m.Flag, o => o.Ignore());

            Mapper.CreateMap<ProjectModel, Project>()
                .ForMember(m => m.Folder, o => o.ResolveUsing<FolderMappings.FolderResolver>().FromMember(s => s.FolderID));
        }
    }
}