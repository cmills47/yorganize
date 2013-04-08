using System;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using Yorganize.Business.Exceptions;
using Yorganize.Business.Repository;
using Yorganize.Showcase.Domain.Models;

namespace Yorganize.Showcase.Web.Controllers
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

            // Verify Authorization
            var member = _memberRepository.FindBy(m => m.LiveId == result.ProviderUserId);
            if (member == null)
                throw new BusinessException(string.Format("You are not authorized to log into this site! Provider user ID: {0}", result.ProviderUserId));

            if (!result.IsSuccessful)
                return RedirectToAction("Index", "Home");

            FormsAuthentication.SetAuthCookie(result.UserName, false);
            return RedirectToLocal(returnUrl);
        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
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
    }
}
