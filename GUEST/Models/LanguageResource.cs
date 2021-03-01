using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Web;

namespace GUEST.Models
{
    public class LanguageResource
    {
        public static string GetCurrentLanguageName()
        {
            string language_name = "vi-VN";
            var cookie = HttpContext.Current.Request.Cookies["language_name"];
            if (cookie == null)
            {
                HttpContext.Current.Response.Cookies.Add(new HttpCookie("language_name", "vi-VN"));
            }
            else
            {
                language_name = cookie.Value;
            }
            return language_name;
        }
        public static ResourceManager GetResourceManager()
        {
            switch (GetCurrentLanguageName())
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