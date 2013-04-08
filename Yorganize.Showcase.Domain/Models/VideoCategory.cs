using Yorganize.Business.Repository;

namespace Yorganize.Showcase.Domain.Models
{
    public class VideoCategory : IEntity<int>
    {
        public virtual int ID { get; set; }
        public virtual string Name { get; set; }

        public virtual VideoCategory Category { get; set; }
    }
}
