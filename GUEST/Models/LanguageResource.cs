using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Web;

namespace GUEST.Models
{
    public class LanguageResource
    {
        public static ResourceManager GetResourceManager()
        {
            string language_id = "vi-VN";
            var cookie = HttpContext.Current.Request.Cookies["language_id"];
            if (cookie == null)
            {
                HttpContext.Current.Response.Cookies.Add(new HttpCookie("language_id", "vi-VN"));
            }
            else
            {
                language_id = cookie.Value;
            }

            switch (language_id)
            {
                case "vi-VN":
                    return Resources.vi_VN.ResourceManager;
                case "en-US":
                    return Resources.en_US.ResourceManager;
                default:
                    return Resources.vi_VN.ResourceManager;
            }
        }
    }
}