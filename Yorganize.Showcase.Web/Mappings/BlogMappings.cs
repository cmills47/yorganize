using System.Text.RegularExpressions;
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
                .CreateMap<BlogPost, BlogPostModel>();

            Mapper
                .CreateMap<BlogPost, BlogPostItemModel>()
                .ForMember(m => m.Excerpt, o => o.MapFrom(s => s.Content))
                .ForMember(m => m.Excerpt, o => o.AddFormatter(new ExcerptFormatter()));
        }

        private class ExcerptFormatter : ValueFormatter<string>
        {

            protected override string FormatValueCore(string value)
            {
                //const string pattern = @"<(.|\n)*?>";
                const string pattern = @"(?></?\w+)(?>(?:[^>'""]+|'[^']*'|""[^""]*"")*)>";
                string formatted = Regex.Replace(value, pattern, string.Empty);
                var maxlen = System.Math.Min(435, formatted.Length);
                return formatted.Substring(0, maxlen) + " ...";
            }
        }
    }
}