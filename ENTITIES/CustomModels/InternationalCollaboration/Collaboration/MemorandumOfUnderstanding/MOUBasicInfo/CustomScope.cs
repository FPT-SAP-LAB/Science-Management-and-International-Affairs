using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding.MOUBasicInfo
{
    public class CustomScope
    {
        public CustomScope(int scope_id, string scope_name)
        {
            this.scope_id = scope_id;
            this.scope_name = scope_name;
        }
        public CustomScope() { }
        public int scope_id { get; set; }
        public string scope_name { get; set; }
    }
}
