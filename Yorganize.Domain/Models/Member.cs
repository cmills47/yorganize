using System;
using Yorganize.Business.Repository;

namespace Yorganize.Domain.Models
{
    public class Member : IEntity<Guid>
    {
        public virtual Guid ID
        {
            get;
            set;
        }

        public virtual string LiveId { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Locale { get; set; }
        public virtual string PreferredEmail { get; set; }
        
        public virtual string AccountEmail { get; set; }
        public virtual string PersonalEmail { get; set; }
        public virtual string BusinessEmail { get; set; }

        public virtual string AccessToken { get; set; }
        public virtual string AuthenticationToken { get; set; }
        public virtual string RefreshToken { get; set; }


        public virtual string YorganizeEmail { get; set; }
    }
}
