using System.Text.RegularExpressions;
using System.Web;
using AutoMapper;
using Yorganize.Showcase.Domain.Models;
using Yorganize.Showcase.Web.Models;

namespace Yorganize.Showcase.Web.Mappings
{
    public class BlogMappings : Profile
    {
        protected override void Configure()
        {
            Mapper
                .CreateMap<BlogPost, BlogPostModel>()
                .ForMember(m => m.Content, o => o.AddFormatter(new RawHtmlFormatter()));

            Mapper
                .CreateMap<BlogPost, BlogPostItemModel>()
                .ForMember(m => m.Excerpt, o => o.MapFrom(s => s.Content))
                .ForMember(m => m.Excerpt, o => o.AddFormatter(new ExcerptFormatter()));

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
                //const string pattern = @"<(.|\n)*?>";
                const string pattern = @"(?></?\w+)(?>(?:[^>'""]+|'[^']*'|""[^""]*"")*)>";
                value = HttpUtility.HtmlDecode(value);
                string formatted = Regex.Replace(value, pattern, string.Empty);
                var maxlen = System.Math.Min(435, formatted.Length);
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

        private class RawHtmlFormatter: ValueFormatter<string>
        {
            protected override string FormatValueCore(string value)
            {
                return HttpUtility.HtmlDecode(value);
            }
        }
    }
}