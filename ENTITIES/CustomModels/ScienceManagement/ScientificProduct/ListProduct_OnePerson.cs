using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.ScienceManagement.ScientificProduct
{
    public class ListProduct_OnePerson
    {
        public string name { get; set; }
        public DateTime date_request { get; set; }
        public int status_id { get; set; }
        public int paper_id { get; set; }
    }
}
