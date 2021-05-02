using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ENTITIES;
using System.Threading.Tasks;
using BLL.InternationalCollaboration.Collaboration.PartnerRepo;
using BLL.ModelDAL;
using Newtonsoft.Json;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.ScienceManagement.ArticlePolicy;
using BLL.ScienceManagement.ArticlePolicy;

namespace BLL
{
    public class HomeRepo
    {
        private readonly ScienceAndInternationalAffairsEntities db;

        public HomeRepo()
        {
            db = new ScienceAndInternationalAffairsEntities();
        }

        public HomeRepo(ScienceAndInternationalAffairsEntities db)
        {
            this.db = db;
        }

        public HomeData GetHomeData(int language_id)
        {
            try
            {
                PartnerRepo partnerRepo = new PartnerRepo(db);

                int partner = partnerRepo.GetPartnerWidget();
                List<string> images = (from x in db.ImageHomePages
                                       join b in db.Files on x.file_id equals b.file_id
                                       where x.is_active && !x.is_wallpaper
                                       select b.file_drive_id).ToList();
                int invention = (from a in db.Inventions
                                 join b in db.RequestInventions on a.invention_id equals b.invention_id
                                 where b.status_id == 2
                                 select a.invention_id).Count();
                int scopusISI = db.Papers.Where(x => x.is_verified == true).Count();
                int researcher = db.Profiles.Where(x => x.profile_page_active).Count();

                ArticlePolicyIndexRepo policyRepo = new ArticlePolicyIndexRepo(db);
                List<ArticlePolicyIndex> articlePolicies = policyRepo.List(language_id);
                return new HomeData(partner, images, invention, scopusISI, researcher, articlePolicies);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return null;
        }

        public string GetBackground()
        {
            try
            {
                string image = (from x in db.ImageHomePages
                                join b in db.Files on x.file_id equals b.file_id
                                where x.is_active && x.is_wallpaper
                                select b.file_drive_id).FirstOrDefault();
                return image;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return null;
        }
    }
}
