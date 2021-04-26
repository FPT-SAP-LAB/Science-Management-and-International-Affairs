using BLL.ModelDAL;
using BLL.InternationalCollaboration.AcademicActivity;
using Microsoft.AspNet.SignalR;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using System.Configuration;
using Owin;

[assembly: OwinStartup(typeof(MANAGER.Startup))]
namespace MANAGER
{
    public class Startup
    {
        public readonly static string clientId = ConfigurationManager.AppSettings["ClientId"];
        public readonly static string redirectUri = ConfigurationManager.AppSettings["RedirectUri"];
        public readonly static string tenant = ConfigurationManager.AppSettings["Tenant"];
        public readonly static string GuestURI = ConfigurationManager.AppSettings["GuestURI"];
        public readonly static string ManagerURI = ConfigurationManager.AppSettings["ManagerURI"];
        public readonly static string authority = string.Format(System.Globalization.CultureInfo.InvariantCulture, ConfigurationManager.AppSettings["Authority"], tenant);
        public void Configuration(IAppBuilder app)
        {
            NotificationRepo.GuestURI = GuestURI;
            NotificationRepo.ManagerURI = ManagerURI;
            FormRepo.GuestURL = GuestURI;
            FormRepo.ManagerURL = ManagerURI;
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);
            app.UseCookieAuthentication(new CookieAuthenticationOptions());
            app.UseOpenIdConnectAuthentication(
                new OpenIdConnectAuthenticationOptions
                {
                    ClientId = clientId,
                    Authority = authority,
                    RedirectUri = redirectUri,
                    PostLogoutRedirectUri = redirectUri,
                    Scope = OpenIdConnectScope.OpenIdProfile,
                    ResponseType = OpenIdConnectResponseType.CodeIdToken,
                    TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = false
                    },
                    //Notifications = new OpenIdConnectAuthenticationNotifications
                    //{
                    //    AuthenticationFailed = OnAuthenticationFailed
                    //}
                }
            );
            // Branch the pipeline here for requests that start with "/signalr"
            app.Map("/signalr", map =>
            {
                // Setup the CORS middleware to run before SignalR.
                // By default this will allow all origins. You can 
                // configure the set of origins and/or http verbs by
                // providing a cors options with a different policy.
                map.UseCors(CorsOptions.AllowAll);
                var hubConfiguration = new HubConfiguration
                {
                    // You can enable JSONP by uncommenting line below.
                    // JSONP requests are insecure but some older browsers (and some
                    // versions of IE) require JSONP to work cross domain
                    // EnableJSONP = true
                };
                // Run the SignalR pipeline. We're not using MapSignalR
                // since this branch already runs under the "/signalr"
                // path.
                map.RunSignalR(hubConfiguration);
            });
        }
    }
}