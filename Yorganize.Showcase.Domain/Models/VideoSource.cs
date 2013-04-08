using System;
using Yorganize.Business.Repository;

namespace Yorganize.Showcase.Domain.Models
{
    public class VideoSource : IEntity<Guid>
    {
        public virtual Guid ID { get; set; }
        public virtual string Format { get; set; }
        public virtual string Url { get; set; }
    }
}
