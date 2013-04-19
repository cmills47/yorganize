using Microsoft.Web.WebPages.OAuth;

namespace Yorganize.Web
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            // To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
            // you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166

#if DEBUG
            // Yorganize.com
            OAuthWebSecurity.RegisterMicrosoftClient(
                clientId: "00000000400F0D53",
                clientSecret: "eu8W8zp5BkAfDVOPIfRhv17IWidIwGjp");

            //OAuthWebSecurity.RegisterClient(new MicrosoftScopedClient(ConfigurationManager.AppSettings["Microsoft.ClientId"].ToString(),
            // ConfigurationManager.AppSettings["Microsoft.Secret"].ToString(), "wl.basic wl.emails"), "Microsoft", null);
#endif

#if (!DEBUG)

            // Yorganize.azurewebsites.net
            OAuthWebSecurity.RegisterMicrosoftClient(
               clientId: "00000000480EEE8E",
               clientSecret: "w00zI8dzX0sb-ykE12lnOJ9NGfZHf1gU");
#endif

            //OAuthWebSecurity.RegisterTwitterClient(
            //    consumerKey: "",
            //    consumerSecret: "");

            //OAuthWebSecurity.RegisterFacebookClient(
            //    appId: "",
            //    appSecret: "");

            //OAuthWebSecurity.RegisterGoogleClient();
        }
    }
}
