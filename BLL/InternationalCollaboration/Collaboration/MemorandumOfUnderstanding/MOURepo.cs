using ENTITIES;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding
{
    public class MOURepo
    {
        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        public List<ListMOU> listAllMOU()
        {
            try
            {
                string sql_mouList =
                    @"select tb2.mou_partner_id,
                    tb1.mou_code,tb3.partner_name,tb3.website,tb2.contact_point_name
                    ,tb2.contact_phone_email,tb2.contact_point_phone,tb1.evidence,
                    tb2.mou_start_date, tb1.mou_end_date, tb1.mou_note, tb10.unit_abbreviation, tb5.scope_name
                    ,tb7.specialization_name, tb9.mou_status_name
                    from IA_Collaboration.MOU tb1 inner join IA_Collaboration.MOUPartner tb2
                    on tb1.mou_id = tb2.mou_id inner join IA_Collaboration.Partner tb3 
                    on tb2.partner_id = tb3.partner_id inner join IA_Collaboration.MOUPartnerScope tb4
                    on tb4.mou_id = tb2.mou_id and tb4.partner_id = tb2.partner_id
                    inner join IA_MasterData.CollaborationScope tb5
                    on tb4.scope_id = tb5.scope_id inner join IA_Collaboration.MOUPartnerSpecialization tb6
                    on tb6.mou_partner_id = tb2.mou_partner_id 
                    inner join IA_Collaboration.Specialization tb7
                    on tb7.specialization_id = tb6.specialization_id
                    inner join IA_Collaboration.MOUStatusHistory tb8 on
                    tb8.mou_id = tb1.mou_id 
                    inner join IA_Collaboration.MOUStatus tb9 on
                    tb9.mou_status_id = tb8.mou_status_id
                    inner join IA_MasterData.InternalUnit tb10 on
                    tb10.unit_id = tb1.unit_id ";
                List<ListMOU> mouList = db.Database.SqlQuery<ListMOU>(sql_mouList).ToList();
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
                string sql_mouList_deleted = $"";
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
            //data of excel sheet??
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
                string sql_mouCode = $"";
                string newMOUCode = db.Database.SqlQuery<string>(sql_mouCode).First();
                return newMOUCode;
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
                string sql_partner = $"";
                List<ENTITIES.Partner> partner = db.Database.SqlQuery<ENTITIES.Partner>(sql_partner).ToList();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<InternalUnit> GetInternalUnit()
        {
            try
            {
                string sql_unitList = $"";
                List<InternalUnit> unitList = db.Database.SqlQuery<InternalUnit>(sql_unitList).ToList();
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
                string sql_partnerList = $"";
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
                string sql_speList = $"";
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
                string sql_scopeList = $"";
                List<CollaborationScope> scopeList = db.Database.SqlQuery<CollaborationScope>(sql_scopeList).ToList();
                return scopeList;
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
                } else
                {
                    if (item.mou_partner_id.Equals(previousItem.mou_partner_id))
                    {
                        if (!previousItem.specialization_name.Contains(item.specialization_name))
                        {
                            previousItem.specialization_name = previousItem.specialization_name + "," + item.specialization_name;
                        }
                        if (!previousItem.scope_name.Contains(item.scope_name))
                        {
                            previousItem.scope_name = previousItem.scope_name + "," + item.scope_name; 
                        }
                        //then remove current object
                        mouList.Remove(item);
                    } else
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
                string sql_inactive_number = $"";
                string sql_expired = $"";
                noti.InactiveNumber = db.Database.SqlQuery<int>(sql_inactive_number).First();
                noti.ExpiredMOUCode = db.Database.SqlQuery<string>(sql_expired).ToList();
                return noti;
            } catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateStatusMOU()
        {
            //get current date
            //get all expired ActiveMOU.
            //if number > 0: update status for MOU: Active => Inactive.
        }

        public class ListMOU 
        {
            public ListMOU() { }
            public string mou_code  { get; set; }
            public int mou_partner_id { get; set; }
            public string partner_name { get; set; }
            public string website { get; set; }
            public string contact_point_name { get; set; }
            public string contact_phone_email { get; set; }
            public string contact_point_phone { get; set; }
            public string evidence { get; set; }
            public DateTime mou_start_date { get; set; }
            public DateTime mou_end_date { get; set; }
            public string mou_start_date_string { get; set; }
            public string mou_end_date_string { get; set; }
            public string mou_note { get; set; }
            public string unit_abbreviation { get; set; }
            public string scope_name { get; set; }
            public string specialization_name { get; set; }
            public string mou_status_name { get; set; }
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
    }
}
