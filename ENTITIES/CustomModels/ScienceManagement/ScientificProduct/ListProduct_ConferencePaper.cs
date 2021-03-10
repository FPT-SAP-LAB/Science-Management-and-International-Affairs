using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.ScienceManagement.ScientificProduct
{
    public class ListProduct_ConferencePaper
    {
        public string name { get; set; }
        public string author { get; set; }
        public string journal_name { get; set; }
        public string quality { get; set; }
        public string vol { get; set; }
        public string page { get; set; }
        public string link_doi { get; set; }
        public string link_scholar { get; set; }
    }
}
