using AutoMapper;
using Yorganize.Showcase.Domain.Models;
using Yorganize.Showcase.Web.Models;

namespace Yorganize.Showcase.Web.Mappings
{
    public class VideoMappings : Profile
    {
        protected override void Configure()
        {
            Mapper
                .CreateMap<VideoCategory, VideoCategoryModel>();

            Mapper
                .CreateMap<Video, VideoModel>();

            Mapper
                .CreateMap<VideoModel, Video>()
                .ForMember(m => m.Category, o => o.ResolveUsing<VideoCategoryResolver>().FromMember(s=>s.CategoryID));

        }

        private class VideoCategoryResolver : ValueResolver<int, VideoCategory>
        {

            protected override VideoCategory ResolveCore(int source)
            {
                return new VideoCategory() { ID = source };
            }
        }
    }
}