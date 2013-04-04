using System;
using Yorganize.Business.Repository;

namespace Yorganize.Showcase.Domain.Models
{
    public class Member : IEntity<Guid>
    {
        public virtual Guid ID
        {
            get;
            set;
        }

        public virtual string LiveId { get; set; }
    }
}
