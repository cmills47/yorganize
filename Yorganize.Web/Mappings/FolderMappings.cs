using AutoMapper;
using System;
using Yorganize.Domain.Models;
using Yorganize.Web.Models;

namespace Yorganize.Web.Mappings
{
    public class FolderMappings : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Folder, FolderModel>()
                  .ForMember(m => m.ID, o => o.ResolveUsing<NullableGuidResolver>().FromMember(s => s.ID));

            Mapper.CreateMap<FolderModel, Folder>()
                .ForMember(m => m.Parent, o => o.ResolveUsing<FolderResolver>().FromMember(s => s.ParentID));
        }

        private class NullableGuidResolver : ValueResolver<Guid, Guid?>
        {
            protected override Guid? ResolveCore(Guid source)
            {
                return source == default(Guid) ? default(Guid?) : source;
            }
        }

        public class FolderResolver : ValueResolver<Guid?, Folder>
        {
            protected override Folder ResolveCore(Guid? source)
            {
                return source.HasValue ? new Folder { ID = source.Value } : null;
            }
        }
    }
}