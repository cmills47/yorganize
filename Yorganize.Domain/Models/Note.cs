using System;
using Yorganize.Business.Repository;

namespace Yorganize.Domain.Models
{
    public class Note : IEntity<Guid>
    {
        public virtual Guid ID
        {
            get;
            set;
        }

        public virtual string Name { get; set; }
        public virtual string Type { get; set; }
        public virtual string Content { get; set; }
        public virtual int Position { get; set; }
        public virtual bool IsProject { get; set; }

        public virtual Project Project { get; set; }
        public virtual Action Action { get; set; }
        public virtual Member Owner { get; set; }
    }
}
