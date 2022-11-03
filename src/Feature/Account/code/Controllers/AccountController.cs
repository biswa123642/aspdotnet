using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using CGP.Feature.Account.Model;
using CGP.Foundation.SitecoreExtensions.Repositories;
using CGP.Foundation.SitecoreExtensions.Utilities;
using Sitecore.XA.Foundation.Mvc.Controllers;

namespace CGP.Feature.Account.Controllers
{
    public class AccountController : StandardController
    {
        private readonly ISiteConfiguration siteConfiguration;

        public AccountController(ISiteConfiguration siteConfiguration)
        {
            this.siteConfiguration = siteConfiguration;
        }

        // GET: Account
        public ActionResult Login()
        {
            var getSiteConfiguration = siteConfiguration.GetSiteConfiguration();

            //Check the userlogin enabled in site level
            if (getSiteConfiguration.UserLogin.EnableLogin)
            {
                //Check the userlogin enabled at Page Level
                if (!FieldUtil.IsChecked(Sitecore.Context.Item, Constants.UserLogin.EnableLogin.ToString()))
                    return new EmptyResult();

                //Check if user is logged In or NOT
                HttpCookie userInfoCookies = Request.Cookies["userInfo"];
                if (userInfoCookies != null && userInfoCookies.Value == Context.User.Name && this.Context.User.Identity.IsAuthenticated)
                    return new EmptyResult();

                if (getSiteConfiguration.UserLogin.PasswordAuth)
                    ViewBag.PasswordAuth = getSiteConfiguration.UserLogin.PasswordAuth;

                return View("~/Views/OneWeb/Account/Login.cshtml");
            }
            return new EmptyResult();
        }

        [HttpPost]
        public JsonResult checkUser(UserModel userModel)
        {
            bool success = false;
            var getSiteConfiguration = siteConfiguration.GetSiteConfiguration();

            if (getSiteConfiguration.UserLogin.PasswordAuth)
            {
                success = Sitecore.Security.Authentication.AuthenticationManager.Login(getSiteConfiguration.UserLogin.PasswordAuthUser, userModel.Password);
            }
            else
            {
                var accessPermission = getSiteConfiguration.UserLogin.usersList.Contains(string.Format("{0}\\{1}", Constants.Login.loginDomain, userModel.Name));

                if (!accessPermission)
                    return Json(new { message = SitecoreUtil.DefaultDictionaryValue("No_Permission"), isSuccess = false }, JsonRequestBehavior.AllowGet);

                success = Sitecore.Security.Authentication.AuthenticationManager.Login(string.Format("{0}\\{1}", Constants.Login.loginDomain, userModel.Name), userModel.Password);

            }
            if (success)
            {
                if (userModel.RememberMe)
                {
                    HttpCookie userInfo = new HttpCookie("userInfo");
                    userInfo.Value = Context.User.Name;
                    userInfo.Expires = DateTime.Now.AddYears(100);
                    Response.Cookies.Add(userInfo);
                    FormsAuthentication.SetAuthCookie(Context.User.Name, true);
                }

                return Json(new { message = SitecoreUtil.DefaultDictionaryValue("LoggedIn"), isSuccess = true }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { message = SitecoreUtil.DefaultDictionaryValue("No_Permission"), isSuccess = false }, JsonRequestBehavior.AllowGet);
        }
    }
}