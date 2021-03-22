using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MANAGER.Support
{
    public class Auther : ActionFilterAttribute, IAuthorizationFilter
    {
        public string RightID { get; set; }
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            BLL.Authen.LoginRepo.User u = (BLL.Authen.LoginRepo.User)filterContext.HttpContext.Session["User"];
            if (u == null)
            {
                var Url = new UrlHelper(filterContext.RequestContext);
                var url = Url.Action("Login", "Authen");
                filterContext.Result = new RedirectResult(url);
            }
            else
            {
                List<int> RightIDs = u.rights;
                bool Check = false;
                foreach (var r in RightID.Split(','))
                {
                    if (RightIDs.Contains(int.Parse(r)))
                    {
                        Check = true;
                        break;
                    }
                }
                if (!Check)
                {
                    string url = u.url;
                    filterContext.Result = new RedirectResult(url);
                }
            }
        }
    }
}