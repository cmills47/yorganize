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
        }
    }
}