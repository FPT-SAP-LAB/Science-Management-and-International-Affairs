using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace User.Models
{
    public class PageTree
    {
        public string Name { get; set; }
        public string URL { get; set; }
        public PageTree() { }
        public PageTree(string name, string url)
        {
            this.Name = name;
            this.URL = url;
        }
    }
}