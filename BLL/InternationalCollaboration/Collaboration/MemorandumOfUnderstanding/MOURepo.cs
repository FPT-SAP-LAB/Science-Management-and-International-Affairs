using ENTITIES;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace BLL.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding
{
    public class MOURepo
    {
        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        public List<ListMOU> listAllMOU(string partner_name, string contact_point_name, string mou_code)
        {
            try
            {
                string sql_mouList =
                    @"select tb2.mou_partner_id,
                        tb1.mou_code,tb3.partner_name,tb11.country_name,tb3.website,tb2.contact_point_name
                        ,tb2.contact_point_email,tb2.contact_point_phone,tb1.evidence,
                        tb2.mou_start_date, tb1.mou_end_date, tb1.mou_note, tb10.office_abbreviation, tb5.scope_abbreviation
                        ,tb7.specialization_name, tb9.mou_status_id,tb1.mou_id
                        from IA_Collaboration.MOU tb1 inner join IA_Collaboration.MOUPartner tb2
                        on tb1.mou_id = tb2.mou_id inner join IA_Collaboration.Partner tb3 
                        on tb2.partner_id = tb3.partner_id inner join IA_Collaboration.MOUPartnerScope tb4
                        on tb4.mou_id = tb2.mou_id and tb4.partner_id = tb2.partner_id
                        inner join IA_MasterData.CollaborationScope tb5
                        on tb4.scope_id = tb5.scope_id inner join IA_Collaboration.MOUPartnerSpecialization tb6
                        on tb6.mou_partner_id = tb2.mou_partner_id 
                        inner join General.Specialization tb7
                        on tb7.specialization_id = tb6.specialization_id
                        inner join 
                        (select max([datetime]) as 'maxdate',mou_status_id, mou_id
                        from IA_Collaboration.MOUStatusHistory 
                        group by mou_status_id, mou_id) tb8 on
                        tb8.mou_id = tb1.mou_id
                        inner join IA_Collaboration.CollaborationStatus tb9 on
                        tb9.mou_status_id = tb8.mou_status_id
                        inner join General.Office tb10 on
                        tb10.office_id = tb1.office_id
                        inner join General.Country tb11 on 
                        tb11.country_id = tb3.country_id
                        where tb1.is_deleted = 0
                        and partner_name like @partner_name
                        and contact_point_name like @contact_point_name
                        and mou_code like @mou_code";
                List<ListMOU> mouList = db.Database.SqlQuery<ListMOU>(sql_mouList,
                    new SqlParameter("partner_name", '%' + partner_name + '%'),
                    new SqlParameter("contact_point_name", '%' + contact_point_name + '%'),
                    new SqlParameter("mou_code", '%' + mou_code + '%')).ToList();
                handlingMOUListData(mouList);
                return mouList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<ListMOU> listAllMOUDeleted()
        {
            try
            {
                string sql_mouList_deleted = @"select tb2.mou_partner_id,
                    tb1.mou_code,tb3.partner_name,tb11.country_name,tb3.website,tb2.contact_point_name
                    ,tb2.contact_point_email,tb2.contact_point_phone,tb1.evidence,
                    tb2.mou_start_date, tb1.mou_end_date, tb1.mou_note, tb10.office_abbreviation, tb5.scope_abbreviation
                    ,tb7.specialization_name, tb9.mou_status_id,tb1.mou_id
                    from IA_Collaboration.MOU tb1 inner join IA_Collaboration.MOUPartner tb2
                    on tb1.mou_id = tb2.mou_id inner join IA_Collaboration.Partner tb3 
                    on tb2.partner_id = tb3.partner_id inner join IA_Collaboration.MOUPartnerScope tb4
                    on tb4.mou_id = tb2.mou_id and tb4.partner_id = tb2.partner_id
                    inner join IA_MasterData.CollaborationScope tb5
                    on tb4.scope_id = tb5.scope_id inner join IA_Collaboration.MOUPartnerSpecialization tb6
                    on tb6.mou_partner_id = tb2.mou_partner_id 
                    inner join General.Specialization tb7
                    on tb7.specialization_id = tb6.specialization_id
                    inner join 
                    (select max([datetime]) as 'maxdate',mou_status_id, mou_id
                    from IA_Collaboration.MOUStatusHistory 
                    group by mou_status_id, mou_id) tb8 on
                    tb8.mou_id = tb1.mou_id
                    inner join IA_Collaboration.CollaborationStatus tb9 on
                    tb9.mou_status_id = tb8.mou_status_id
                    inner join General.Office tb10 on
                    tb10.office_id = tb1.office_id
                    inner join General.Country tb11 on 
                    tb11.country_id = tb3.country_id
                    where tb1.is_deleted = 1";
                List<ListMOU> mouList = db.Database.SqlQuery<ListMOU>(sql_mouList_deleted).ToList();
                handlingMOUListData(mouList);
                return mouList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void addMOU(MOUAdd input)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    db.MOUs.Add(input.MOU);
                    db.MOUStatusHistories.Add(input.MOUStatusHistory);
                    foreach (MOUPartner item in input.listMOUPartner)
                    {
                        db.MOUPartners.Add(item);
                    }
                    foreach (MOUPartnerSpecialization item in input.listMOUPartnerSpe)
                    {
                        db.MOUPartnerSpecializations.Add(item);
                    }
                    foreach (MOUPartnerScope item in input.listMOUPartnerScope)
                    {
                        db.MOUPartnerScopes.Add(item);
                    }
                    db.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }
        public void ExportMOUExcel()
        {
            string path = HostingEnvironment.MapPath("/Content/assets/excel/Collaboration/download/");
            string filename = "MOU.xlsx";
            FileInfo file = new FileInfo(path + filename);
            List<ListMOU> listMOU = listAllMOU("", "", "");

            using (ExcelPackage excelPackage = new ExcelPackage(file))
            {
                ExcelWorkbook excelWorkbook = excelPackage.Workbook;
                ExcelWorksheet excelWorksheet = excelWorkbook.Worksheets.First();
                int startRow = 3;
                for (int i = 0; i < listMOU.Count; i++)
                {
                    excelWorksheet.Cells[i + startRow, 1].Value = i + 1;
                    excelWorksheet.Cells[i + startRow, 2].Value = listMOU.ElementAt(i).mou_code;
                    excelWorksheet.Cells[i + startRow, 3].Value = listMOU.ElementAt(i).partner_name;
                    excelWorksheet.Cells[i + startRow, 4].Value = listMOU.ElementAt(i).country_name;
                    excelWorksheet.Cells[i + startRow, 5].Value = listMOU.ElementAt(i).website;
                    excelWorksheet.Cells[i + startRow, 6].Value = listMOU.ElementAt(i).specialization_name;
                    excelWorksheet.Cells[i + startRow, 7].Value = listMOU.ElementAt(i).contact_point_name;
                    excelWorksheet.Cells[i + startRow, 7].Value = listMOU.ElementAt(i).contact_phone_email;
                    excelWorksheet.Cells[i + startRow, 8].Value = listMOU.ElementAt(i).contact_point_phone;
                    excelWorksheet.Cells[i + startRow, 9].Value = listMOU.ElementAt(i).mou_start_date;
                    excelWorksheet.Cells[i + startRow, 10].Value = listMOU.ElementAt(i).mou_end_date;
                    excelWorksheet.Cells[i + startRow, 11].Value = listMOU.ElementAt(i).office_abbreviation;
                    excelWorksheet.Cells[i + startRow, 12].Value = listMOU.ElementAt(i).scope_abbreviation;
                    excelWorksheet.Cells[i + startRow, 13].Value = listMOU.ElementAt(i).mou_status_name;
                }
                //string Flocation = "/Content/assets/excel/CDVT/download/baocaohoatdong.xlsx";
                string Flocation = "/Content/assets/excel/Collaboration/download/MOU.xlsx";
                string savePath = HostingEnvironment.MapPath(Flocation);
                excelPackage.SaveAs(new FileInfo(HostingEnvironment.MapPath("/Content/assets/excel/Collaboration/download/MOU.xlsx")));
            }
        }
        public void deleteMOU(int mou_id)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    MOU mou = db.MOUs.Find(mou_id);
                    mou.is_deleted = true;
                    db.Entry(mou).State = EntityState.Modified;
                    db.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }
        public string getSuggestedMOUCode()
        {
            try
            {
                string sql_mouCode = @"select count(*) from IA_Collaboration.MOU where mou_code like @year";
                string sql_checkDup = @"select count(*) from IA_Collaboration.MOU where mou_code = @newCode";
                bool isDuplicated = false;
                string newCode = "";
                int countInYear = db.Database.SqlQuery<int>(sql_mouCode,
                        new SqlParameter("year", '%' + DateTime.Now.Year + '%')).First();
                //fix duplicate mou_code:
                do
                {
                    countInYear++;
                    newCode = DateTime.Now.Year + "/" + countInYear;
                    isDuplicated = db.Database.SqlQuery<int>(sql_checkDup,
                        new SqlParameter("newCode", newCode)).First() == 1 ? true : false;
                } while (isDuplicated);
                return newCode;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool partnerIsExisted(int partner_id)
        {
            try
            {
                string sql_partner = $"select * from IA_Collaboration.Partner where partner_id = @partner_id";
                ENTITIES.Partner partner = db.Database.SqlQuery<ENTITIES.Partner>(sql_partner,
                    new SqlParameter("partner_id", partner_id)).FirstOrDefault();
                return partner is null ? false : true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CustomOffice> GetOffice()
        {
            try
            {
                string sql_unitList = @"select office_id,office_abbreviation from General.Office";
                List<CustomOffice> unitList = db.Database.SqlQuery<CustomOffice>(sql_unitList).ToList();
                return unitList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<ENTITIES.Partner> GetPartners()
        {
            try
            {
                string sql_partnerList = @"select * from IA_Collaboration.Partner";
                List<ENTITIES.Partner> partnerList = db.Database.SqlQuery<ENTITIES.Partner>(sql_partnerList).ToList();
                return partnerList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<Specialization> GetSpecializations()
        {
            try
            {
                string sql_speList = @"select * from General.Specialization";
                List<Specialization> speList = db.Database.SqlQuery<Specialization>(sql_speList).ToList();
                return speList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CollaborationScope> GetCollaborationScopes()
        {
            try
            {
                string sql_scopeList = @"select * from IA_MasterData.CollaborationScope";
                List<CollaborationScope> scopeList = db.Database.SqlQuery<CollaborationScope>(sql_scopeList).ToList();
                return scopeList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public CustomPartner CheckPartner(string partner_name)
        {
            try
            {
                string sql = @"select t1.partner_id,t1.partner_name,t2.country_id,t2.country_name
                    ,t1.address,t1.website from IA_Collaboration.Partner t1
                    left join General.Country t2 on
                    t1.country_id = t2.country_id where t1.partner_name = @partner_name";
                CustomPartner p = db.Database.SqlQuery<CustomPartner>(sql,
                    new SqlParameter("partner_name", partner_name)).FirstOrDefault();
                //if (p != null)
                //{
                //    p.website = p.website is null ? "" : p.website;
                //    p.address = p.address is null ? "" : p.address;
                //}
                return p;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CustomPartner CheckPartnerEdit(string partner_name)
        {
            try
            {
                string sql = @"select t1.partner_id,t1.partner_name,t2.country_id,t2.country_name
                    ,t1.address,t1.website from IA_Collaboration.Partner t1
                    left join General.Country t2 on
                    t1.country_id = t2.country_id where t1.partner_name = @partner_name";
                CustomPartner p = db.Database.SqlQuery<CustomPartner>(sql,
                    new SqlParameter("partner_name", partner_name)).FirstOrDefault();
                return p;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void checkCollabStatus()
        {
            //?
        }
        private void handlingMOUListData(List<ListMOU> mouList)
        {
            //handling spe and scope data.
            //handling calender display
            ListMOU previousItem = null;
            foreach (ListMOU item in mouList.ToList())
            {
                if (previousItem == null) //first record
                {
                    previousItem = item;
                    previousItem.mou_start_date_string = item.mou_start_date.ToString("dd'/'MM'/'yyyy");
                    previousItem.mou_end_date_string = item.mou_end_date.ToString("dd'/'MM'/'yyyy");
                }
                else
                {
                    if (item.mou_partner_id.Equals(previousItem.mou_partner_id))
                    {
                        if (!previousItem.specialization_name.Contains(item.specialization_name))
                        {
                            previousItem.specialization_name = previousItem.specialization_name + "," + item.specialization_name;
                        }
                        if (!previousItem.scope_abbreviation.Contains(item.scope_abbreviation))
                        {
                            previousItem.scope_abbreviation = previousItem.scope_abbreviation + "," + item.scope_abbreviation;
                        }
                        //then remove current object
                        mouList.Remove(item);
                    }
                    else
                    {
                        previousItem = item;
                    }
                }
            }
        }
        public int getDuration()
        {
            DateTime today = DateTime.Today;
            DateTime end_date = new DateTime(2021, 05, 20);
            TimeSpan value = end_date.Subtract(today);
            return value.Duration().Days;
        }
        public NotificationInfo getNoti()
        {
            try
            {
                NotificationInfo noti = new NotificationInfo();
                DateTime nextMonth = DateTime.Now.AddMonths(1);
                DateTime next3Months = DateTime.Now.AddMonths(3);
                //Warning 1: end_date < next3Months && notiCount = 0
                //warning 2: end_date < nextMonths && notiCount = 1
                string sql_inactive_number
                    = @"select count(*) from IA_Collaboration.MOU tb1 left join 
                        (
                        select max([datetime]) as 'maxdate',mou_status_id, mou_id
                        from IA_Collaboration.MOUStatusHistory 
                        group by mou_status_id, mou_id) tb2 
                        on tb1.mou_id = tb2.mou_id
                        where tb1.is_deleted = 0";
                string sql_expired
                    = @"select * from IA_Collaboration.MOU
                        where (mou_end_date < @next3Months and noti_count = 0) or 
                        (mou_end_date < @nextMonth and noti_count = 1)";
                noti.InactiveNumber = db.Database.SqlQuery<int>(sql_inactive_number).First();
                noti.ExpiredMOUCode = db.Database.SqlQuery<string>(sql_expired).ToList();
                updateNotiCount(noti);
                return noti;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void updateNotiCount(NotificationInfo noti)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    foreach (string mouCode in noti.ExpiredMOUCode)
                    {
                        MOU mou = db.MOUs.Where(x => x.mou_code.Equals(mouCode)).First();
                        mou.noti_count += 1;
                        db.Entry(mou).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }
        public void UpdateStatusMOU()
        {
            //get current date
            //get all expired ActiveMOU.
            //if number > 0: update status for MOU: Active => Inactive.

            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    string sql_expired = @"select mou_id from IA_Collaboration.MOU tb1 where tb1.mou_end_date > @current_date";
                    List<int> mouIdList = db.Database.SqlQuery<int>(sql_expired,
                        new SqlParameter("current_date", DateTime.Now)).ToList();
                    if (mouIdList.Count > 0)
                    {
                        foreach (int id in mouIdList)
                        {
                            MOUStatusHistory mou = new MOUStatusHistory();
                            mou.mou_id = id;
                            mou.mou_status_id = 2;
                            mou.datetime = DateTime.Now;
                            db.MOUStatusHistories.Add(mou);
                            db.SaveChanges();
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }
        public class ListMOU
        {
            public ListMOU() { }
            public string mou_code { get; set; }
            public int mou_partner_id { get; set; }
            public int mou_id { get; set; }
            public string partner_name { get; set; }
            public string website { get; set; }
            public string country_name { get; set; }
            public string contact_point_name { get; set; }
            public string contact_phone_email { get; set; }
            public string contact_point_phone { get; set; }
            public string evidence { get; set; }
            public DateTime mou_start_date { get; set; }
            public DateTime mou_end_date { get; set; }
            public string mou_start_date_string { get; set; }
            public string mou_end_date_string { get; set; }
            public string mou_note { get; set; }
            public string office_abbreviation { get; set; }
            public string scope_abbreviation { get; set; }
            public string specialization_name { get; set; }
            public string mou_status_name { get; set; }
            public int mou_status_id { get; set; }
        }
        public class MOUAdd
        {
            public MOUAdd() { }
            public MOU MOU { get; set; }
            public MOUStatusHistory MOUStatusHistory { get; set; }
            public List<MOUPartner> listMOUPartner { get; set; }
            public List<MOUPartnerSpecialization> listMOUPartnerSpe { get; set; }
            public List<MOUPartnerScope> listMOUPartnerScope { get; set; }
        }
        public class NotificationInfo
        {
            public NotificationInfo() { }
            public int InactiveNumber { get; set; }
            public List<string> ExpiredMOUCode { get; set; }
        }
        public class CustomOffice
        {
            public string office_abbreviation { get; set; }
            public int office_id { get; set; }
        }
        public class CustomPartner
        {
            public string website { get; set; }
            public int country_id { get; set; }
            public string country_name { get; set; }
            public int partner_id { get; set; }
            public string partner_name { get; set; }
            public string address { get; set; }
        }
    }
}
