using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Yorganize.Showcase.Web.Models
{
    public class BlogModel
    {
        public List<BlogPostModel> Posts { get; set; }
    }

    public class BlogPostModel
    {
        public Guid ID
        {
            get;
            set;
        }

        public bool IsNew
        {
            get { return ID == Guid.Empty; }
        }

        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Header { get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; }
        public string Author { get; set; }
        public string ImageUrl { get; set; }
    }

    public class BlogPostItemModel
    {
        public Guid ID
        {
            get;
            set;
        }

        public string Title { get; set; }
        public string Slug { get; set; }
        public string Excerpt { get; set; }
        public DateTime Created { get; set; }
        public string Author { get; set; }
        public string ThumbnailUrl { get; set; }
    }

    public class BlogArchiveModel
    {
        public string Month { get; set; }
        public uint Year { get; set; }
        public uint Posts { get; set; }
    }
}