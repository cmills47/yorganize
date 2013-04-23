using Microsoft.Web.WebPages.OAuth;
using Yorganize.Web.Infrastructure;

namespace Yorganize.Web
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            // To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
            // you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166


            const string scope = "wl.basic wl.emails wl.signin wl.photos wl.offline_access wl.skydrive_update";

#if DEBUG
            // Yorganize.com
            //OAuthWebSecurity.RegisterMicrosoftClient(
            //    clientId: "00000000400F0D53",
            //    clientSecret: "eu8W8zp5BkAfDVOPIfRhv17IWidIwGjp");

            const string clientId = "00000000400F0D53";
            const string clientSecret = "eu8W8zp5BkAfDVOPIfRhv17IWidIwGjp";

            OAuthWebSecurity.RegisterClient(new MicrosoftScopedClient(clientId, clientSecret, scope), "Microsoft", null);
#endif

#if (!DEBUG)


            const string clientId = "00000000480F1887";
            const string clientSecret = "Ue5b2ZVREtMPh2-l1ic0I9AiK1BJPDyX";

            OAuthWebSecurity.RegisterClient(new MicrosoftScopedClient(clientId, clientSecret, scope), "Microsoft", null);
#endif


        }
    }
}
