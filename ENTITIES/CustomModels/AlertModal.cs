using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels
{
    public class AlertModal<T>
    {
        public bool success { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public T obj { get; set; }

        public AlertModal(T obj, bool success, string title, string content)
        {
            this.obj = obj;
            this.success = success;
            this.title = title;
            this.content = content;
        }
    }
}
