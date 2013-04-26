using System;
using System.Collections.Generic;

namespace Yorganize.Web.Models
{
    public class ProjectModel
    {
        public virtual Guid ID
        {
            get;
            set;
        }

        public  string Name { get; set; }
        public  string Status { get; set; }
        public  string Type { get; set; }
        public  int Position { get; set; }

        public  int EstimatedCompletionTime { get; set; }
        public  string EstimatedCompletionUnit { get; set; }

        public  DateTime? StartDate { get; set; }
        public  DateTime? DueDate { get; set; }
        public  DateTime? LastSaved { get; set; }
        public  DateTime? LastReviewed { get; set; }
        public  DateTime? ReferenceDate { get; set; }

        public  string RepeatBehavior { get; set; }
        public  int? RepeatInterval { get; set; }
        public  string RepeatUnit { get; set; }

        public  FlagModel Flag { get; set; }
        public  int? SelectedNoteID { get; set; }

        public Guid? FolderID { get; set; }

        public  IEnumerable<ActionModel> Actions { get; set; }
    }
}