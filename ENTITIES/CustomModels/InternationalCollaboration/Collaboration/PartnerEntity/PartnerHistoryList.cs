using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.InternationalCollaboration.Collaboration.PartnerEntity
{
    public class PartnerHistoryList<T> : PartnerHistory
    {
        public List<T> Data { get; set; }
        public string Partner_name { get; set; }
        public PartnerHistoryList(List<T> data, string partner_name)
        {
            Data = data;
            Partner_name = partner_name;
        }
    }
}
