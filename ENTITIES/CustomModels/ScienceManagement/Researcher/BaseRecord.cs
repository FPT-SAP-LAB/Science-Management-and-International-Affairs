using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.ScienceManagement.Researcher
{
    public class BaseRecord<T>
    {
        public int index { get; set; }
        public T records { get; set; }
    }
}
