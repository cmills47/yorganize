using System;
using Yorganize.Business.Repository;

namespace Yorganize.Domain.Models
{
    public class Context : IEntity<Guid>
    {
        public virtual Guid ID
        {
            get;
            set;
        }

        public virtual string Name { get; set; }
        public virtual string Status { get; set; }
        public virtual int Position { get; set; }

        public DateTime LastSavedTime { get; set; }

        public virtual Context Parent { get; set; }
        public virtual Member Owner { get; set; }
    }
}
