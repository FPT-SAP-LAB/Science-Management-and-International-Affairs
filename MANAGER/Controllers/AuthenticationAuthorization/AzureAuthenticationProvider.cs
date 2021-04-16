//using Microsoft.Graph;
//using Microsoft.Identity.Client;
//using System;
//using System.IdentityModel.Tokens;
//using System.Net.Http;
//using System.Threading.Tasks;

//namespace MANAGER.Controllers.AzureAuthenticationProvider
//{
//    public class AzureAuthenticationProvider : IAuthenticationProvider
//    {
//        private string _azureDomain = "myDevDom.onmicrosoft.com";

//        public async Task AuthenticateRequestAsync(HttpRequestMessage request)
//        {
//            try
//            {
//                string clientId = "2b823c67-1b0d-4a10-a9e1-737142516f5q";
//                string clientSecret = "xxxxxx";

//                AuthenticationContext authContext = new AuthenticationContext("https://login.windows.net/" + _azureDomain + "/oauth2/token");

//                ClientCredential credentials = new ClientCredential(clientId, clientSecret);

//                AuthenticationResult authResult = await authContext.AcquireTokenAsync("https://graph.microsoft.com/", credentials);

//                request.Headers.Add("Authorization", "Bearer " + authResult.AccessToken);
//            }
//            catch (Exception ex)
//            {
//            }
//        }
//    }
//}