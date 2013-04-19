using Microsoft.Web.WebPages.OAuth;

namespace Yorganize.Showcase.Web.App_Start
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
#if DEBUG
            // Yorganize.com
            OAuthWebSecurity.RegisterMicrosoftClient(
                clientId: "00000000400F0D53",
                clientSecret: "eu8W8zp5BkAfDVOPIfRhv17IWidIwGjp");

            // Production
            //OAuthWebSecurity.RegisterMicrosoftClient(
            //   clientId: "00000000480CEF36",
            //   clientSecret: "ZCoAXAcvS6GvoM/V1+A89kxXrGildT3j");
#endif

#if (!DEBUG)

            // Yorganize.azurewebsites.net
            OAuthWebSecurity.RegisterMicrosoftClient(
               clientId: "00000000480EEE8E",
               clientSecret: "w00zI8dzX0sb-ykE12lnOJ9NGfZHf1gU");
#endif
        }
    }
}
