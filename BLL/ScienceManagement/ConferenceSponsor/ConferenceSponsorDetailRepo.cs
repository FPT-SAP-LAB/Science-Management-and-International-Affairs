using ENTITIES;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ScienceManagement.ConferenceSponsor
{
    public class ConferenceSponsorDetailRepo
    {
        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        public string GetDetailPageGuest(int request_id, int account_id, int language_id)
        {
            BaseRequest request = db.BaseRequests.Where(x => x.account_id == account_id && x.request_id == request_id).FirstOrDefault();
            RequestConference requestConference = request.RequestConference;
            if (request == null || request.RequestConference == null)
                return null;

            db.Configuration.LazyLoadingEnabled = false;
            return JsonConvert.SerializeObject(new { request, requestConference });
        }
    }
}
