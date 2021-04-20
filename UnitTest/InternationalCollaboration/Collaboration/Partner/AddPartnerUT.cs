using BLL.InternationalCollaboration.Collaboration.PartnerRepo;
using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.InternationalCollaboration.Collaboration.PartnerEntity;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UnitTest.InternationalCollaboration.Collaboration.Partner
{
    [TestFixture]
    public class AddPartnerUT
    {
        private PartnerRepo partnerRepo;
        [TestCase]
        public void AddPartnerUT_1()
        {
            partnerRepo = new PartnerRepo();
            int count = 0;
            List<ENTITIES.Partner> listTest = new List<ENTITIES.Partner>();
            SearchPartner searchPartner = new SearchPartner();
            do
            {
                searchPartner.partner_name = "Unit_Test_" + count;
                listTest = new ScienceAndInternationalAffairsEntities()
                    .Partners.Where(x => x.partner_name.Equals(searchPartner.partner_name)).ToList();
                count++;
            } while (listTest.Count != 0);
            Random ran = new Random();
            string partner_name = searchPartner.partner_name;
            string content = @"<h1 class='title-detail' style='margin-right: 0px; margin-bottom: 15px; margin-left: 0px; padding: 0px; text-rendering: optimizelegibility; line-height: 48px; font-size: 32px; font-weight: 700; font-feature-settings: &quot;pnum&quot;, &quot;lnum&quot;; font-family: Merriweather, serif; color: rgb(34, 34, 34); background-color: rgb(252, 250, 246);'>Chuẩn bị kịch bản lập bệnh viện dã chiến ở Hà Tiên</h1><p class='description' style='margin-right: 0px; margin-bottom: 15px; margin-left: 0px; padding: 0px; text-rendering: optimizelegibility; font-size: 18px; line-height: 28.8px; color: rgb(34, 34, 34); font-family: arial; background-color: rgb(252, 250, 246);'>Bộ trưởng Y tế Nguyễn Thanh Long đánh giá khu vực biên giới tại thành phố Hà Tiên là điểm nóng, tình trạng nhập cảnh cả chính ngạch và trái phép phức tạp, cần chuẩn bị kịch bản trong tình huống xấu hơn.</p><article class='fck_detail ' style='margin: 0px; padding: 0px; text-rendering: optimizelegibility; width: 670px; float: left; position: relative; font-variant-numeric: normal; font-variant-east-asian: normal; font-stretch: normal; font-size: 18px; line-height: 28.8px; font-family: arial; color: rgb(34, 34, 34); background-color: rgb(252, 250, 246);'><p class='Normal' style='margin-right: 0px; margin-bottom: 1em; margin-left: 0px; padding: 0px; text-rendering: optimizespeed; line-height: 28.8px;'>'Bộ Y tế chỉ đạo Hà Tiên lập bệnh viện dã chiến, có khu cấp cứu đạt mức độ cao trong tình huống dịch lan rộng, có những ca nặng cần điều trị ngay tại chỗ', ông Long nói khi thị sát công tác phòng chống dịch tại Kiên Giang, ngày 18/4.</p><p class='Normal' style='margin-right: 0px; margin-bottom: 1em; margin-left: 0px; padding: 0px; text-rendering: optimizespeed; line-height: 28.8px;'>Bệnh viện Chợ Rẫy sẽ phối hợp địa phương xây dựng các phương án thành lập những khu vực điều trị có thể tiếp nhận bệnh nhân nặng đến rất nặng. Nhân viên y tế địa phương cũng được tập huấn điều trị, chăm sóc bệnh nhân.</p><p class='Normal' style='margin-right: 0px; margin-bottom: 1em; margin-left: 0px; padding: 0px; text-rendering: optimizespeed; line-height: 28.8px;'>Ngoài thiết lập bệnh viện dã chiến, Bộ trưởng Long yêu cầu lãnh đạo tỉnh Kiên Giang xây dựng hệ thống xét nghiệm rộng, nhanh, đảm bảo trong tình huống dịch lan rộng. Viện Pasteur TP HCM hỗ trợ thiết lập các labo xét nghiệm đủ tiêu chuẩn tại Hà Tiên và Bệnh viện Đa khoa tỉnh Kiên Giang.</p><p class='Normal' style='margin-right: 0px; margin-bottom: 1em; margin-left: 0px; padding: 0px; text-rendering: optimizespeed; line-height: 28.8px;'>Kiên Giang là tỉnh giáp biên giới với Campuchia - vùng dịch đang bùng phát mạnh, với hơn 56 km biên giới trên đường bộ lẫn trên biển, cùng hơn 145 hòn đảo lớn nhỏ. Bộ Y tế nhận định Kiên Giang là địa phương có 'đường biên dài, vùng biển rộng nhưng khoảng cách nhỏ'. Do đó, nguy cơ dịch bệnh xâm nhập tại Kiên Giang, đặc biệt là khu vực biên giới tại thành phố Hà Tiên là rất lớn. Hiện những ca nhập cảnh được ghi nhận qua cửa khẩu này đều là ca cách ly ngay.</p><p class='Normal' style='margin-right: 0px; margin-bottom: 1em; margin-left: 0px; padding: 0px; text-rendering: optimizespeed; line-height: 28.8px;'>Bộ trưởng Long chỉ đạo ngành y tế chuẩn bị kịch bản trong tình huống xấu hơn, trong đó có kế hoạch thiết lập bệnh viện dã chiến. Ông kêu gọi người dân nên nhập cảnh đường chính ngạch, đặc biệt khi phát hiện người nhập cảnh trái phép cần báo chính quyền địa phương ngay.</p><p class='Normal' style='margin-right: 0px; margin-bottom: 1em; margin-left: 0px; padding: 0px; text-rendering: optimizespeed; line-height: 28.8px;'>'Người dân là tai là mắt, giám sát cộng đồng rất tốt', ông Long nói</p><p class='Normal' style='margin-right: 0px; margin-bottom: 1em; margin-left: 0px; padding: 0px; text-rendering: optimizespeed; line-height: 28.8px;'><img src='image_0' data-filename='Screenshot 2021-04-14 124736.png' style='width: 1203px;'><br></p></article>";
            int country_id = ran.Next(249) + 1;
            string website = @"https://docs.microsoft.com/en-us/dotnet/api/system.random?view=net-5.0";
            string address = "Khu công nghệ cao Hòa Lạc - Hà Nội '";
            Random ran_language = new Random();
            int partner_language_type = ran_language.Next(1, 2);
            PartnerArticle partner_article = new PartnerArticle
            {
                partner_name = partner_name,
                country_id = country_id,
                website = website,
                address = address,
                partner_language_type = partner_language_type
            };

            List<HttpPostedFileBase> files_request = new List<HttpPostedFileBase>();
            int list_before = new ScienceAndInternationalAffairsEntities().Partners.Count();
            AlertModal<string> check = partnerRepo.AddPartner(files_request, null, content,
                        partner_article, 0, 60);
            int list_after = new ScienceAndInternationalAffairsEntities().Partners.Count();
            ENTITIES.Partner partner = IsAdded();
            ENTITIES.Article article = IsAdded_Article(partner.article_id);
            ENTITIES.ArticleVersion articleVersion = IsAdded_AV(partner.article_id);
            if (list_after - list_before == 1)
            {
                if (partner.partner_name.Equals(partner_article.partner_name) &&
                   partner.address.Equals(address) &&
                   partner.country_id == country_id &&
                   partner.website.Equals(website) &&
                   article.account_id == 60 &&
                   articleVersion.language_id == partner_language_type &&
                   articleVersion.article_content.Equals(content))
                {
                    Assert.Pass();
                }
                else
                {
                    Assert.Fail();
                }
            }
            else
            {
                Assert.Fail();
            }
        }

        [TestCase]
        public void AddPartnerUT_2()
        {
            partnerRepo = new PartnerRepo();
            Random ran = new Random();
            string partner_name = "Deakin University";
            string content = @"<h1 class='title-detail' style='margin-right: 0px; margin-bottom: 15px; margin-left: 0px; padding: 0px; text-rendering: optimizelegibility; line-height: 48px; font-size: 32px; font-weight: 700; font-feature-settings: &quot;pnum&quot;, &quot;lnum&quot;; font-family: Merriweather, serif; color: rgb(34, 34, 34); background-color: rgb(252, 250, 246);'>Chuẩn bị kịch bản lập bệnh viện dã chiến ở Hà Tiên</h1><p class='description' style='margin-right: 0px; margin-bottom: 15px; margin-left: 0px; padding: 0px; text-rendering: optimizelegibility; font-size: 18px; line-height: 28.8px; color: rgb(34, 34, 34); font-family: arial; background-color: rgb(252, 250, 246);'>Bộ trưởng Y tế Nguyễn Thanh Long đánh giá khu vực biên giới tại thành phố Hà Tiên là điểm nóng, tình trạng nhập cảnh cả chính ngạch và trái phép phức tạp, cần chuẩn bị kịch bản trong tình huống xấu hơn.</p><article class='fck_detail ' style='margin: 0px; padding: 0px; text-rendering: optimizelegibility; width: 670px; float: left; position: relative; font-variant-numeric: normal; font-variant-east-asian: normal; font-stretch: normal; font-size: 18px; line-height: 28.8px; font-family: arial; color: rgb(34, 34, 34); background-color: rgb(252, 250, 246);'><p class='Normal' style='margin-right: 0px; margin-bottom: 1em; margin-left: 0px; padding: 0px; text-rendering: optimizespeed; line-height: 28.8px;'>'Bộ Y tế chỉ đạo Hà Tiên lập bệnh viện dã chiến, có khu cấp cứu đạt mức độ cao trong tình huống dịch lan rộng, có những ca nặng cần điều trị ngay tại chỗ', ông Long nói khi thị sát công tác phòng chống dịch tại Kiên Giang, ngày 18/4.</p><p class='Normal' style='margin-right: 0px; margin-bottom: 1em; margin-left: 0px; padding: 0px; text-rendering: optimizespeed; line-height: 28.8px;'>Bệnh viện Chợ Rẫy sẽ phối hợp địa phương xây dựng các phương án thành lập những khu vực điều trị có thể tiếp nhận bệnh nhân nặng đến rất nặng. Nhân viên y tế địa phương cũng được tập huấn điều trị, chăm sóc bệnh nhân.</p><p class='Normal' style='margin-right: 0px; margin-bottom: 1em; margin-left: 0px; padding: 0px; text-rendering: optimizespeed; line-height: 28.8px;'>Ngoài thiết lập bệnh viện dã chiến, Bộ trưởng Long yêu cầu lãnh đạo tỉnh Kiên Giang xây dựng hệ thống xét nghiệm rộng, nhanh, đảm bảo trong tình huống dịch lan rộng. Viện Pasteur TP HCM hỗ trợ thiết lập các labo xét nghiệm đủ tiêu chuẩn tại Hà Tiên và Bệnh viện Đa khoa tỉnh Kiên Giang.</p><p class='Normal' style='margin-right: 0px; margin-bottom: 1em; margin-left: 0px; padding: 0px; text-rendering: optimizespeed; line-height: 28.8px;'>Kiên Giang là tỉnh giáp biên giới với Campuchia - vùng dịch đang bùng phát mạnh, với hơn 56 km biên giới trên đường bộ lẫn trên biển, cùng hơn 145 hòn đảo lớn nhỏ. Bộ Y tế nhận định Kiên Giang là địa phương có 'đường biên dài, vùng biển rộng nhưng khoảng cách nhỏ'. Do đó, nguy cơ dịch bệnh xâm nhập tại Kiên Giang, đặc biệt là khu vực biên giới tại thành phố Hà Tiên là rất lớn. Hiện những ca nhập cảnh được ghi nhận qua cửa khẩu này đều là ca cách ly ngay.</p><p class='Normal' style='margin-right: 0px; margin-bottom: 1em; margin-left: 0px; padding: 0px; text-rendering: optimizespeed; line-height: 28.8px;'>Bộ trưởng Long chỉ đạo ngành y tế chuẩn bị kịch bản trong tình huống xấu hơn, trong đó có kế hoạch thiết lập bệnh viện dã chiến. Ông kêu gọi người dân nên nhập cảnh đường chính ngạch, đặc biệt khi phát hiện người nhập cảnh trái phép cần báo chính quyền địa phương ngay.</p><p class='Normal' style='margin-right: 0px; margin-bottom: 1em; margin-left: 0px; padding: 0px; text-rendering: optimizespeed; line-height: 28.8px;'>'Người dân là tai là mắt, giám sát cộng đồng rất tốt', ông Long nói</p><p class='Normal' style='margin-right: 0px; margin-bottom: 1em; margin-left: 0px; padding: 0px; text-rendering: optimizespeed; line-height: 28.8px;'><img src='image_0' data-filename='Screenshot 2021-04-14 124736.png' style='width: 1203px;'><br></p></article>";
            int country_id = ran.Next(249) + 1;
            string website = @"https://docs.microsoft.com/en-us/dotnet/api/system.random?view=net-5.0";
            string address = "Khu công nghệ cao Hòa Lạc - Hà Nội '";
            Random ran_language = new Random();
            int partner_language_type = ran_language.Next(1, 2);
            PartnerArticle partner_article = new PartnerArticle
            {
                partner_name = partner_name,
                country_id = country_id,
                website = website,
                address = address,
                partner_language_type = partner_language_type
            };

            List<HttpPostedFileBase> files_request = new List<HttpPostedFileBase>();
            int list_before = new ScienceAndInternationalAffairsEntities().Partners.Count();
            AlertModal<string> check = partnerRepo.AddPartner(files_request, null, content,
                        partner_article, 0, 60);
            int list_after = new ScienceAndInternationalAffairsEntities().Partners.Count();
            if (list_after - list_before == 0)
            {
                if (!(bool)check.success)
                {
                    Assert.Pass();
                }
                else
                {
                    Assert.Fail();
                }
            }
            else
            {
                Assert.Fail();
            }
        }
        public ENTITIES.Partner IsAdded()
        {
            return new ScienceAndInternationalAffairsEntities().Partners.OrderByDescending(x => x.partner_id).FirstOrDefault();
        }

        public ENTITIES.Article IsAdded_Article(int article_id)
        {
            return new ScienceAndInternationalAffairsEntities().Articles.Where(x => x.article_id == article_id).FirstOrDefault();
        }

        public ENTITIES.ArticleVersion IsAdded_AV(int article_id)
        {
            return new ScienceAndInternationalAffairsEntities().ArticleVersions.Where(x => x.article_id == article_id).FirstOrDefault();
        }
    }
}
