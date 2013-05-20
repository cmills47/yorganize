using System;

namespace Yorganize.Web.Models
{
    public class ActionModel
    {
        public Guid ID
        {
            get;
            set;
        }

        public string Name { get; set; }
        public int? Position { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }

        public int? EstimatedCompletionTime { get; set; }
        public string EstimatedCompletionUnit { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? LastSaved { get; set; }
        public DateTime? ReferenceDate { get; set; }

        public string RepeatBehavior { get; set; }
        public int? RepeatInterval { get; set; }
        public string RepeatUnit { get; set; }

        public FlagModel Flag { get; set; }
        public Guid? SelectedNoteID { get; set; }

        public Guid? ProjectID { get; set; }
        public Guid OwnerID { get; set; }
    }
}