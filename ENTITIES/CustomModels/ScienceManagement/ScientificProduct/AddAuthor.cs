using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ENTITIES.CustomModels.ScienceManagement.ScientificProduct
{
    public class AddAuthor : Person
    {
        public int contract_id { get; set; }
        public int title_id { get; set; }
        public string office_abbreviation { get; set; }
        public string mssv_msnv { get; set; }
        public string bank_branch { get; set; }
        public Int64 bank_number { get; set; }
        public Int64 tax_code { get; set; }
        public string identification_number { get; set; }
        //public int office_id { get; set; }
        public int paper_id { get; set; }
        public bool is_reseacher { get; set; }
        public string title_strring { get; set; }
        public string money_string { get; set; }
    }
}
