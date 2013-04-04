using System;
using Yorganize.Business.Repository;

namespace Yorganize.Showcase.Domain.Models
{
    public class BlogPost : IEntity<Guid>
    {
        public virtual Guid ID
        {
            get;
            set;
        }

        public virtual string Title { get; set; }
        public virtual string Slug { get; set; }
        public virtual string Header { get; set; }
        public virtual string Content { get; set; }

        public virtual string ImageUrl { get; set; }
        public virtual string ThumbnailUrl { get; set; }

        public virtual DateTime Created { get; set; }

        public virtual Member Author { get; set; }
    }
}
