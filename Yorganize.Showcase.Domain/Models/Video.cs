using System;
using Yorganize.Business.Repository;

namespace Yorganize.Showcase.Domain.Models
{
    public class Video : IEntity<Guid>
    {
        public virtual Guid ID { get; set; }
        public virtual string Title { get; set; }
        public virtual string Description { get; set; }
        public virtual int Order { get; set; }
        public virtual string SourceMP4Url { get; set; }
        public virtual string SourceOGGUrl { get; set; }
        public virtual string SourceWEBMUrl { get; set; }

        public virtual VideoCategory Category { get; set; }
    }
}
