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
            var output = (from a in db.RequestConferences
                          join b in db.Conferences on a.conference_id equals b.conference_id
                          join c in db.Countries on b.country_id equals c.country_id
                          join d in db.Files on a.invitation_file_id equals d.file_id
                          join e in db.Files on a.paper_id equals e.file_id
                          select new
                          {
                              a.attendance_end,
                              a.attendance_start,
                              a.conference_id,
                              a.editable,
                              InvitationLink = d.link,
                              PaperLink = e.link
                          }).FirstOrDefault();
            return JsonConvert.SerializeObject(output);
        }
    }
}
