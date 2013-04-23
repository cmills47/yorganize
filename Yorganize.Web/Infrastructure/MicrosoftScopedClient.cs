using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using DotNetOpenAuth.AspNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetOpenAuth.AspNet.Clients;
using Newtonsoft.Json;
using Yorganize.Business.Exceptions;

namespace Yorganize.Web.Infrastructure
{
    public class MicrosoftScopedClient : IAuthenticationClient
    {
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _scope;

        private const string baseUrl = "https://login.live.com/oauth20_authorize.srf";
        private const string tokenUrl = "https://login.live.com/oauth20_token.srf";

        public MicrosoftScopedClient(string clientId, string clientSecret, string scope)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
            _scope = scope;
        }

        public string ProviderName
        {
            get { return "Microsoft"; }
        }

        public void RequestAuthentication(HttpContextBase context, Uri returnUrl)
        {
            string url = baseUrl + "?client_id=" + _clientId + "&redirect_uri=" + HttpUtility.UrlEncode(returnUrl.ToString()) + "&scope=" + HttpUtility.UrlEncode(_scope) + "&response_type=code";
            context.Response.Redirect(url);
        }

        public AuthenticationResult VerifyAuthentication(HttpContextBase context)
        {
            string code = context.Request.QueryString["code"];
            string error = context.Request.QueryString["error"];
            string errorDescription = context.Request.QueryString["error_description"];

            if (!string.IsNullOrEmpty(error))
                return new AuthenticationResult(new BusinessException(string.Format("{0}:{1}",error, errorDescription)));

            string rawUrl = context.Request.Url.ToString();
            rawUrl = Regex.Replace(rawUrl, "&code=[^&]*", "");

            IDictionary<string, string> userData = GetUserData(code, rawUrl);

            if (userData == null)
                return new AuthenticationResult(false, ProviderName, null, null, null);

            string id = userData["id"];
            string username = userData["username"];

            var result = new AuthenticationResult(true, ProviderName, id, username, userData);

            return result;
        }

        private IDictionary<string, string> GetUserData(string accessCode, string redirectUri)
        {
            var tokens = QueryAccessToken(redirectUri, accessCode);

            if (tokens == null)
                return null;

            var userData = GetUserData(tokens.Access);

            userData.Add("token.access", tokens.Access);
            userData.Add("token.authentication", tokens.Authentication);
            userData.Add("token.refresh", tokens.Refresh);

            return userData;
        }

        private IDictionary<string, string> GetUserData(string accessToken)
        {
            ExtendedMicrosoftClientUserData graph;
            var request = WebRequest.Create("https://apis.live.net/v5.0/me?access_token=" + EscapeUriDataStringRfc3986(accessToken));

            using (var response = request.GetResponse())
            {
                using (var responseStream = response.GetResponseStream())
                {
                    using (var sr = new StreamReader(responseStream))
                    {
                        string data = sr.ReadToEnd();
                        graph = JsonConvert.DeserializeObject<ExtendedMicrosoftClientUserData>(data);
                    }
                }
            }

            var userData = new Dictionary<string, string>
                {
                    {"id", graph.Id},
                    {"username", graph.Name},
                    {"name", graph.Name},
                    {"link", graph.Link == null ? null : graph.Link.AbsoluteUri},
                    {"gender", graph.Gender},
                    {"firstname", graph.First_Name},
                    {"lastname", graph.Last_Name},
                    {"locale", graph.Locale},
                    {"email.personal", graph.Emails.Personal},
                    {"email.business", graph.Emails.Business},
                    {"email.account", graph.Emails.Account},
                    {"email", graph.Emails.Preferred}
                };

            return userData;
        }

        private dynamic QueryAccessToken(string returnUrl, string authorizationCode)
        {
            var entity =
                CreateQueryString(
                    new Dictionary<string, string> {
                        { "client_id", this._clientId },
                        { "redirect_uri", returnUrl },
                        { "client_secret", this._clientSecret},
                        { "code", authorizationCode },
                        { "grant_type", "authorization_code" },
                    });

            WebRequest tokenRequest = WebRequest.Create(tokenUrl);
            tokenRequest.ContentType = "application/x-www-form-urlencoded";
            tokenRequest.ContentLength = entity.Length;
            tokenRequest.Method = "POST";

            using (Stream requestStream = tokenRequest.GetRequestStream())
            {
                var writer = new StreamWriter(requestStream);
                writer.Write(entity);
                writer.Flush();
            }

            var tokenResponse = (HttpWebResponse)tokenRequest.GetResponse();
            if (tokenResponse.StatusCode == HttpStatusCode.OK)
            {
                using (var responseStream = tokenResponse.GetResponseStream())
                {
                    using (var sr = new StreamReader(responseStream))
                    {
                        string data = sr.ReadToEnd();
                        var tokenData = JsonConvert.DeserializeObject<OAuth2AccessTokenData>(data);
                        if (tokenData != null)
                        {
                            return new
                                {
                                    Access = tokenData.AccessToken,
                                    Authentication = authorizationCode,
                                    Refresh = tokenData.RefreshToken
                                };

                        }
                    }
                }
            }

            return null;
        }

        private static readonly string[] UriRfc3986CharsToEscape = new[] { "!", "*", "'", "(", ")" };
        private static string EscapeUriDataStringRfc3986(string value)
        {
            var escaped = new StringBuilder(Uri.EscapeDataString(value));

            // Upgrade the escaping to RFC 3986, if necessary.
            foreach (string t in UriRfc3986CharsToEscape)
            {
                escaped.Replace(t, Uri.HexEscape(t[0]));
            }

            // Return the fully-RFC3986-escaped string.
            return escaped.ToString();
        }

        private static string CreateQueryString(IEnumerable<KeyValuePair<string, string>> args)
        {
            if (!args.Any())
            {
                return string.Empty;
            }
            StringBuilder sb = new StringBuilder(args.Count() * 10);

            foreach (var p in args)
            {
                sb.Append(EscapeUriDataStringRfc3986(p.Key));
                sb.Append('=');
                sb.Append(EscapeUriDataStringRfc3986(p.Value));
                sb.Append('&');
            }
            sb.Length--; // remove trailing &

            return sb.ToString();
        }

        protected class ExtendedMicrosoftClientUserData
        {
            public string First_Name { get; set; }
            public string Gender { get; set; }
            public string Id { get; set; }
            public string Last_Name { get; set; }
            public Uri Link { get; set; }
            public string Name { get; set; }
            public Emails Emails { get; set; }
            public string Locale { get; set; }
        }

        protected class Emails
        {
            public string Preferred { get; set; }
            public string Account { get; set; }
            public string Personal { get; set; }
            public string Business { get; set; }
        }
    }
}