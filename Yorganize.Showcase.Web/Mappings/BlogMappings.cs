using System;
using System.Text.RegularExpressions;
using System.Web;
using AutoMapper;
using Yorganize.Showcase.Domain.Models;
using Yorganize.Showcase.Web.Models;
using System.ServiceModel.Syndication;

namespace Yorganize.Showcase.Web.Mappings
{
    public class BlogMappings : Profile
    {
        protected override void Configure()
        {
            Mapper
                .CreateMap<BlogPost, BlogPostModel>()
                .ForMember(m => m.Content, o => o.AddFormatter(new RawHtmlFormatter()))
                .ForMember(m => m.ThumbnailUrl, o => o.MapFrom(s => s.ImageUrl))
                .ForMember(m => m.ThumbnailUrl, o => o.AddFormatter<ThumbnailUrlNoCacheFormatter>());


            Mapper
                .CreateMap<BlogPost, BlogPostItemModel>()
                .ForMember(m => m.ThumbnailUrl, o => o.MapFrom(s => s.ImageUrl))
                .ForMember(m => m.ThumbnailUrl, o => o.AddFormatter<ThumbnailUrlNoCacheFormatter>())
                .ForMember(m => m.Excerpt, o => o.MapFrom(s => s.Content))
                .ForMember(m => m.Excerpt, o => o.AddFormatter(new ExcerptFormatter()));

            Mapper
                .CreateMap<BlogPost, SyndicationItem>()
                .ForMember(m => m.Title, o => o.ResolveUsing<TextSyndicateContentResolver>().FromMember(s => s.Title))
                .ForMember(m => m.Summary, o => o.ResolveUsing<HtmlSyndicateContentResolver>().FromMember(s => s.Content))
                .ForMember(m => m.Content, o => o.ResolveUsing<HtmlSyndicateContentResolver>().FromMember(s => s.Content))
                .ForMember(m => m.Id, o => o.MapFrom(s => s.Title))
                .ForMember(m => m.Id, o => o.AddFormatter(new SlugFormatter()))

                .ForMember(m => m.PublishDate, o => o.MapFrom(s => s.Created));

            Mapper
                .CreateMap<BlogPostModel, BlogPost>()
                .ForMember(m => m.Created, o => o.Ignore())
                .ForMember(m => m.Slug, o => o.MapFrom(s => s.Title))
                .ForMember(m => m.Slug, o => o.AddFormatter(new SlugFormatter()));
        }

        private class ExcerptFormatter : ValueFormatter<string>
        {
            protected override string FormatValueCore(string value)
            {
                return GetExcerpt(value);
            }

            public static string GetExcerpt(string source)
            {
                if (string.IsNullOrEmpty(source))
                    return string.Empty;

                //const string pattern = @"<(.|\n)*?>";
                const string pattern = @"(?></?\w+)(?>(?:[^>'""]+|'[^']*'|""[^""]*"")*)>";
                source = HttpUtility.HtmlDecode(source);
                string formatted = Regex.Replace(source, pattern, string.Empty);
                var maxlen = System.Math.Min(255, formatted.Length);
                return formatted.Substring(0, maxlen) + " ...";
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

        private class RawHtmlFormatter : ValueFormatter<string>
        {
            protected override string FormatValueCore(string value)
            {
                return HttpUtility.HtmlDecode(value);
            }
        }

        private class HtmlSyndicateContentResolver : ValueResolver<string, SyndicationContent>
        {

            protected override SyndicationContent ResolveCore(string source)
            {
                return SyndicationContent.CreateHtmlContent(HttpUtility.HtmlDecode(source));
            }
        }

        private class TextSyndicateContentResolver : ValueResolver<string, SyndicationContent>
        {

            protected override SyndicationContent ResolveCore(string source)
            {
                return SyndicationContent.CreatePlaintextContent(ExcerptFormatter.GetExcerpt(source));
            }
        }

        private class ThumbnailUrlNoCacheFormatter : ValueFormatter<string>
        {

            protected override string FormatValueCore(string value)
            {
                return string.IsNullOrEmpty(value)
                           ? value
                           : string.Format("{0}?{1}", value, DateTime.Now.TimeOfDay.TotalSeconds);
            }
        }
    }
}