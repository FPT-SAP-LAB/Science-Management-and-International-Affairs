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
                string sql_mouList = $"";
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
                List<Partner> partner = db.Database.SqlQuery<Partner>(sql_partner).ToList();
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

        public List<Partner> GetPartners()
        {
            try
            {
                string sql_partnerList = $"";
                List<Partner> partnerList = db.Database.SqlQuery<Partner>(sql_partnerList).ToList();
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
            }
            catch (Exception ex)
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

        public class ListMOU { }

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
