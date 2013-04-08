using System;
using Yorganize.Business.Repository;

namespace Yorganize.Showcase.Domain.Models
{
    public class Video : IEntity<Guid>
    {
        public virtual Guid ID { get; set; }
        public virtual string Title { get; set; }
        public virtual string Description { get; set; }
    }
}
