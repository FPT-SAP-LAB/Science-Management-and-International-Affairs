using System.Reflection;
using System.Web.Mvc;

namespace GUEST.Models
{
    public class AjaxOnlyAttribute : ActionMethodSelectorAttribute
    {
        public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
        {
            return controllerContext.HttpContext.Request?.Headers["X-Requested-With"] == "XMLHttpRequest";
        }
    }
}