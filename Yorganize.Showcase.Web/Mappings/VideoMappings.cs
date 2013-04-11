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
                .CreateMap<VideoCategoryModel, VideoCategory>();

            Mapper
                .CreateMap<Video, VideoModel>();

            Mapper
                .CreateMap<VideoModel, Video>()
                .ForMember(m => m.Category, o => o.ResolveUsing<VideoCategoryResolver>().FromMember(s => s.CategoryID))
                .ForMember(m => m.SourceMP4Url, o => o.Ignore())
                .ForMember(m => m.SourceOGGUrl, o => o.Ignore())
                .ForMember(m => m.SourceWEBMUrl, o => o.Ignore())
                .AfterMap((src, dst) =>
                    {
                        // overwrite source just if new one is specified
                        if (!string.IsNullOrEmpty(src.SourceMP4Url))
                            dst.SourceMP4Url = src.SourceMP4Url;

                        if (!string.IsNullOrEmpty(src.SourceOGGUrl))
                            dst.SourceMP4Url = src.SourceOGGUrl;

                        if (!string.IsNullOrEmpty(src.SourceWEBMUrl))
                            dst.SourceMP4Url = src.SourceWEBMUrl;
                    });

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