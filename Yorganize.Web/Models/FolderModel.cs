using System;
using System.Collections.Generic;

namespace Yorganize.Web.Models
{
    public class FolderModel
    {
        public Guid? ID
        {
            get;
            set;
        }

        public Guid? ParentID { get; set; }

        public  string Name { get; set; }
        public  int Position { get; set; }
      
        public IEnumerable<ProjectModel> Projects { get; set; }
    }
}