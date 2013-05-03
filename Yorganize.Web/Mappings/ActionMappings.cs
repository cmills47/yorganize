using AutoMapper;
using System;
using Yorganize.Web.Models;

namespace Yorganize.Web.Mappings
{
    public class ActionMappings : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Domain.Models.Action, ActionModel>()
                  .ForMember(m => m.Flag, o => o.Ignore());
        }
    }
}