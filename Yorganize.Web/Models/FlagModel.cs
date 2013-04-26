using System;

namespace Yorganize.Web.Models
{
    public class FlagModel
    {
        public virtual Guid ID
        {
            get;
            set;
        }

        public virtual string Url { get; set; }
    }
}