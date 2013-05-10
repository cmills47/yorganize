using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yorganize.Business.Providers.Membership.Principal
{
    public interface ILiveIdPrincipal : System.Security.Principal.IPrincipal
    {
        Guid UserId { get; set; }
        string LiveId { get; set; }
    }
}
