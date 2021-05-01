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

        public HomeData GetHomeData()
        {
            try
            {
                PartnerRepo partnerRepo = new PartnerRepo(db);

                int partner = partnerRepo.GetPartnerWidget();
                List<string> images = (from x in db.ImageHomePages
                                       join b in db.Files on x.file_id equals b.file_id
                                       where x.is_active && !x.is_wallpaper
                                       select b.file_drive_id).ToList();

                return new HomeData(partner, images);
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
