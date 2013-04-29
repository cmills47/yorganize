using System;
using Yorganize.Business.Repository;

namespace Yorganize.Domain.Models
{
    public class Action : IEntity<Guid>
    {
        public virtual Guid ID
        {
            get;
            set;
        }

        public virtual string Name { get; set; }
        public virtual string Status { get; set; }
        public virtual string Type { get; set; }
        public virtual int Position { get; set; }

        public virtual int EstimatedCompletionTime { get; set; }
        public virtual string EstimatedCompletionUnit { get; set; }

        public virtual DateTime? StartDate { get; set; }
        public virtual DateTime? DueDate { get; set; }
        public virtual DateTime? LastSaved { get; set; }
        public virtual DateTime? ReferenceDate { get; set; }

        public virtual string RepeatBehavior { get; set; }
        public virtual int? RepeatInterval { get; set; }
        public virtual string RepeatUnit { get; set; }

        public virtual Flag Flag { get; set; }
        public virtual Note SelectedNote { get; set; }

        public virtual Project Project { get; set; }
        public virtual Member Owner { get; set; }
    }
}
