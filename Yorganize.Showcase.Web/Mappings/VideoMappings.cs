using System.Text.RegularExpressions;
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
                .CreateMap<Video, VideoModel>()
                .ForMember(m => m.Slug, o => o.MapFrom(s => s.Title))
                .ForMember(m => m.Slug, o => o.AddFormatter<SlugFormatter>());

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
                            dst.SourceOGGUrl = src.SourceOGGUrl;

                        if (!string.IsNullOrEmpty(src.SourceWEBMUrl))
                            dst.SourceWEBMUrl = src.SourceWEBMUrl;
                    });

        }

        private class VideoCategoryResolver : ValueResolver<int, VideoCategory>
        {
            protected override VideoCategory ResolveCore(int source)
            {
                return new VideoCategory() { ID = source };
            }
        }

        private class SlugFormatter : ValueFormatter<string>
        {
            protected override string FormatValueCore(string value)
            {
                string str = RemoveAccent(value).ToLower();
                // invalid chars           
                str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
                // convert multiple spaces into one space   
                str = Regex.Replace(str, @"\s+", " ").Trim();
                // cut and trim 
                str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
                str = Regex.Replace(str, @"\s", "-"); // hyphens   

                return str;
            }

            private static string RemoveAccent(string txt)
            {
                byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(txt);
                return System.Text.Encoding.ASCII.GetString(bytes);
            }
        }
    }
}