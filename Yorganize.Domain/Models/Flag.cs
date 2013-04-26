using System;
using Yorganize.Business.Repository;

namespace Yorganize.Domain.Models
{
    public class Flag : IEntity<Guid>
    {
        public virtual Guid ID
        {
            get;
            set;
        }

        public virtual string Url { get; set; }
    }
}
