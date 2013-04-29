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
                  .ForMember(m => m.Actions, o => o.Ignore())
                  .ForMember(m => m.Flag, o => o.Ignore());
            //TODO: remap flag and actions
        }
    }
}