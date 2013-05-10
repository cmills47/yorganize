using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using Yorganize.Business.Exceptions;
using Yorganize.Business.Repository;
using Yorganize.Domain.Models;
using Yorganize.Web.Infrastructure;
using Yorganize.Business.Providers.Membership.Principal;
using System.Web;

namespace Yorganize.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {

        private readonly IKeyedRepository<Guid, Member> _memberRepository;

        public AccountController(IKeyedRepository<Guid, Member> memberRepository)
        {
            _memberRepository = memberRepository;
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        private void CreateAuthenticationTicket(string username, Guid userId, string liveId, bool persistent)
        {

            LiveIdPrincipalSerializedModel serializeModel = new LiveIdPrincipalSerializedModel()
            {
                UserId = userId,
                LiveId = liveId
            };

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string userData = serializer.Serialize(serializeModel);

            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, username, DateTime.Now, DateTime.Now.AddDays(8), persistent, userData);
            string encTicket = FormsAuthentication.Encrypt(authTicket);
            HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
            Response.Cookies.Add(faCookie);
        }

        [AllowAnonymous]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        }

        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            // Verify Authentication
            AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));

            if (!result.IsSuccessful)
                if (result.Error != null)
                    throw result.Error;
                else
                    return RedirectToAction("Error", "Home");

            // Verify Authorization
            var member = _memberRepository.FindBy(m => m.LiveId == result.ProviderUserId);

            if (member == null) // create account
            {

                member = new Member()
                    {
                        LiveId = result.ProviderUserId,
                        AccessToken = result.ExtraData["token.access"],
                        AuthenticationToken = result.ExtraData["token.authentication"],
                        RefreshToken = result.ExtraData["token.refresh"],
                        FirstName = result.ExtraData["firstname"],
                        LastName = result.ExtraData["lastname"],
                        Locale = result.ExtraData["locale"],
                        PreferredEmail = result.ExtraData["email"],
                        PersonalEmail = result.ExtraData["email.personal"],
                        BusinessEmail = result.ExtraData["email.business"],
                        AccountEmail = result.ExtraData["email.account"]
                    };

                member.YorganizeEmail = GetYorganizeEmail(new List<string> { member.PreferredEmail, member.PersonalEmail, member.BusinessEmail, member.AccountEmail }, member.FirstName, member.LastName);

                Guid memberID = _memberRepository.Insert(member);

                //FormsAuthentication.SetAuthCookie(result.UserName, false);
                CreateAuthenticationTicket(result.UserName, memberID, result.ProviderUserId, false); 
                TempData.Add("command", "prepare-profile");
                TempData.Add("token", result.ExtraData["token.access"]);

                return Redirect(Url.Action("Index", "Home"));
            }

            // user already registered
            //FormsAuthentication.SetAuthCookie(result.UserName, false);
            CreateAuthenticationTicket(result.UserName, member.ID, result.ProviderUserId, false); 
            return RedirectToLocal(returnUrl);
        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        #endregion

        public ActionResult CreateFolder()
        {
            string token = TempData["token"].ToString();
            const string folder = "Yorganize";
            string result;

            WebRequest request = WebRequest.Create("https://apis.live.net/v5.0/me/skydrive");
            request.Method = "POST";
            const string postData = "{name: \"" + folder + "\"}";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.Headers.Add("Authorization", "Bearer " + token);
            request.ContentType = "application/json";
            request.ContentLength = byteArray.Length;

            using (Stream reqst = request.GetRequestStream())
            {
                reqst.Write(byteArray, 0, byteArray.Length);
                reqst.Flush();
            }

            try
            {
                using (var res = (HttpWebResponse)request.GetResponse())
                {
                    using (var resst = res.GetResponseStream())
                    {
                        using (var sr = new StreamReader(resst))
                        {
                            result = sr.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw new BusinessException(string.Format("Could not create '{0}' folder in your SkyDrive root folders.", folder));
            }

            return new JsonNetResult(result);
        }

        private string GetYorganizeEmail(IEnumerable<string> emails, string firstName, string lastName)
        {

            const string domain = "@yorganize.net";

            // get all possible aliases from emails
            var aliases = (from email in emails.Where(eml => !string.IsNullOrEmpty(eml))
                           select email.Split('@') into parts
                           where parts.Length > 0
                           select string.Format("{0}{1}", parts[0], domain)).ToList();

            if (!string.IsNullOrEmpty(firstName))
            {
                aliases.Add(string.Format("{0}{1}", firstName, domain));
                if (!string.IsNullOrEmpty(lastName))
                    aliases.Add(string.Format("{0}.{1}{2}", firstName, lastName, domain));
                // add 20 random aliases starting with first name
                var rnd = new Random();
                for (int i = 0; i < 20; i++)
                    aliases.Add(string.Format("{0}{1}{2}", firstName, rnd.Next(1000), domain));
            }

            // add a guid alias
            aliases.Add(string.Format("{0}{1}", Guid.NewGuid().ToString().Replace("-", ""), domain));

            // get all emails from aliases that are already in the database
            var existing = (from member in _memberRepository.All()
                            where aliases.Contains(member.YorganizeEmail)
                            select member.YorganizeEmail).ToList();

            // remove all aliases that are in use
            aliases.RemoveAll(existing.Contains);

            // return first available alias
            if (aliases.Count > 0)
                return aliases.First();

            // if no alias remains (all are used) return null
            return null;
        }
    }
}