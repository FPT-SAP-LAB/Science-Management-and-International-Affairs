using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL.Authen;
using Google.Apis.Auth;
using Microsoft.Graph;
using Microsoft.Graph.Auth;
using Microsoft.Identity.Client;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
namespace MANAGER.Controllers.AuthenticationAuthorization
{
    public class AuthenController : Controller
    {
        LoginRepo repo;
        public async System.Threading.Tasks.Task<ActionResult> Login()
        {
            if (Request.IsAuthenticated)
            {
                repo = new LoginRepo();
                var userClaims = User.Identity as System.Security.Claims.ClaimsIdentity;

                IConfidentialClientApplication confidentialClientApplication = ConfidentialClientApplicationBuilder
                    .Create("87bc895b-a5dc-4fa1-a1a4-b65e15742e4c")
                    .WithTenantId("881de24c-35fa-4040-9655-b749e9dc154d")
                    .WithClientSecret("Ol4h_~VdlN36MCtVsdUQWL0020P-e~pDul")
                    .Build();

                AuthorizationCodeProvider authProvider = new AuthorizationCodeProvider(confidentialClientApplication);

                // Create an authentication provider.
                ClientCredentialProvider authenticationProvider = new ClientCredentialProvider(confidentialClientApplication);
                // Configure GraphServiceClient with provider.
                GraphServiceClient graphServiceClient = new GraphServiceClient(authenticationProvider);
                var users = await graphServiceClient.Users.Request().GetAsync();

                var temp = await graphServiceClient.Users["8f25ffa4-fe8f-4f34-b6ed-b6734fe0c36f"]
                    .Request()
                    .GetAsync();
                using (var photoStream = await graphServiceClient.Users["8f25ffa4-fe8f-4f34-b6ed-b6734fe0c36f"].Photo.Content
                               .Request()
                               .GetAsync())
                {
                    // your code      
                    var a = "";
                }
                //var photo = temp.Photo;
                // Make a request
                var me = await graphServiceClient.Me.Request().WithForceRefresh(true).GetAsync();

                ENTITIES.CustomModels.Authen.Gmail user = new ENTITIES.CustomModels.Authen.Gmail
                {
                    email = userClaims?.FindFirst("preferred_username")?.Value,
                    id = String.Empty,
                    name = userClaims?.FindFirst("name")?.Value,
                    imageurl = userClaims.Claims.FirstOrDefault(x => x.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value
                };
                List<int> roleAccept = new List<int>() { 2, 3 };
                LoginRepo.User u = repo.getAccount(user, roleAccept);
                if (u != null)
                {
                    Session["User"] = u;
                    return Redirect(u.url);
                }
                else
                {
                    return View();
                }
            }
            else
                return View();
        }
        public void SignIn()
        {
            if (!Request.IsAuthenticated)
            {
                HttpContext.GetOwinContext().Authentication.Challenge(
                    new AuthenticationProperties { RedirectUri = "/" },
                    OpenIdConnectAuthenticationDefaults.AuthenticationType);
            }
        }
        public ActionResult Logout()
        {
            HttpContext.GetOwinContext().Authentication.SignOut(
                    OpenIdConnectAuthenticationDefaults.AuthenticationType,
                    CookieAuthenticationDefaults.AuthenticationType);
            Session.Abandon();
            return RedirectToAction("Login");
        }
        //[HttpPost]
        //public async System.Threading.Tasks.Task<ActionResult> SigninGoogleAsync(string idtoken)
        //{
        //    repo = new LoginRepo();
        //    ENTITIES.CustomModels.Authen.Gmail user = await GetUserDetailsAsync(idtoken);
        //    List<int> roleAccept = new List<int>() { 2, 3 };
        //    LoginRepo.User u = repo.getAccount(user, roleAccept);
        //    if (u == null)
        //    {
        //        return Json(String.Empty);
        //    }
        //    Session["User"] = u;
        //    return Redirect(u.url);
        //}
        //public async System.Threading.Tasks.Task<ENTITIES.CustomModels.Authen.Gmail> GetUserDetailsAsync(string providerToken)
        //{
        //    GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(providerToken);
        //    if (!payload.Audience.Equals("24917390994-co652l6gu3eeoqaf96oc9h4av23eprot.apps.googleusercontent.com"))
        //        return null;
        //    if (!payload.Issuer.Equals("accounts.google.com") && !payload.Issuer.Equals("https://accounts.google.com"))
        //        return null;
        //    if (payload.ExpirationTimeSeconds == null)
        //        return null;
        //    else
        //    {
        //        DateTime now = DateTime.Now.ToUniversalTime();
        //        DateTime expiration = DateTimeOffset.FromUnixTimeSeconds((long)payload.ExpirationTimeSeconds).DateTime;
        //        if (now > expiration)
        //        {
        //            return null;
        //        }
        //    }
        //    return new ENTITIES.CustomModels.Authen.Gmail
        //    {
        //        email = payload.Email,
        //        id = payload.Subject,
        //        name = payload.Name,
        //        imageurl = payload.Picture
        //    };
        //}
    }
}