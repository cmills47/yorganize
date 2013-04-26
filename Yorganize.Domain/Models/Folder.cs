using System;
using System.Collections.Generic;
using Yorganize.Business.Repository;

namespace Yorganize.Domain.Models
{
    public class Folder : IEntity<Guid>
    {
        public virtual Guid ID
        {
            get;
            set;
        }

        public virtual string Name { get; set; }
        public virtual int? Position { get; set; }

        public virtual Folder Parent { get; set; }
        public virtual Member Owner { get; set; }

        public virtual IEnumerable<Project> Projects { get; set; }

    }
}
