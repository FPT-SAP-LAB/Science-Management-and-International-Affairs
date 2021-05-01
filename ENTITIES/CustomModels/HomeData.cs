using System.Collections.Generic;

namespace ENTITIES.CustomModels
{
    public class HomeData
    {
        public int Partner { get; set; }
        public List<string> Images { get; set; }

        public HomeData()
        {
        }

        public HomeData(int partner, List<string> images)
        {
            Partner = partner;
            Images = images;
        }
    }
}
