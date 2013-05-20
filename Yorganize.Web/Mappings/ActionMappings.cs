using AutoMapper;
using System;
using Yorganize.Domain.Models;
using Yorganize.Web.Models;

namespace Yorganize.Web.Mappings
{
    public class ActionMappings : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Domain.Models.Action, ActionModel>()
                  .ForMember(m => m.Flag, o => o.Ignore());

            Mapper.CreateMap<ActionModel, Domain.Models.Action>()
                  .ForMember(m => m.Project, o => o.ResolveUsing<ProjectResolver>().FromMember(s => s.ProjectID));

        }

        public class ProjectResolver : ValueResolver<Guid?, Project>
        {
            protected override Project ResolveCore(Guid? source)
            {
                return source.HasValue ? new Project { ID = source.Value } : null;
            }
        }
    }
}