using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace Yorganize.Business.Providers.Membership.Principal
{
    public class LiveIdPrincipal : ILiveIdPrincipal
    {
        public Guid UserId
        {
            get;
            set;
        }

        public string LiveId
        {
            get;
            set;
        }

        public System.Security.Principal.IIdentity Identity
        {
            get;
            private set;
        }

        public bool IsInRole(string role)
        {
            return Identity != null && Identity.IsAuthenticated &&
           !string.IsNullOrWhiteSpace(role) && Roles.IsUserInRole(Identity.Name, role);
        }

        public LiveIdPrincipal(string username)
        {
            Identity = new GenericIdentity(username);

        }
    }

    public class LiveIdPrincipalSerializedModel
    {
        public Guid UserId { get; set; }
        public string LiveId { get; set; }
    }
}
