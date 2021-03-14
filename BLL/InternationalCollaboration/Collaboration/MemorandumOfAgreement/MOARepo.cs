using ENTITIES;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.InternationalCollaboration.Collaboration.MemorandumOfAgreement
{
    public class MOARepo
    {
        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        public List<ListMOA> listAllMOA(string partner_name, string moa_code,string mou_id)
        {
            try
            {
                string sql_mouList =
                    @"select t1.moa_id,t1.moa_code,t3.partner_name,t1.evidence,t2.moa_start_date,
                        t1.moa_end_date,t5.office_name,t8.scope_abbreviation,t9.mou_status_id
                        from IA_Collaboration.MOA t1
                        left join IA_Collaboration.MOAPartner t2 
                        on t2.moa_id = t1.moa_id 
                        left join IA_Collaboration.Partner t3
                        on t3.partner_id = t2.partner_id
                        left join IA_Collaboration.MOU t4 
                        on t4.mou_id = t1.mou_id
                        left join General.Office t5 
                        on t5.office_id = t4.office_id
                        left join IA_Collaboration.MOAPartnerScope t6
                        on t6.moa_id = t1.moa_id
                        left join IA_Collaboration.PartnerScope t7
                        on t7.partner_scope_id = t6.partner_scope_id
                        left join IA_Collaboration.CollaborationScope t8 
                        on t8.scope_id = t7.scope_id
                        left join (
                        select max([datetime]) as 'maxdate',mou_status_id, moa_id
                        from IA_Collaboration.MOAStatusHistory 
                        group by mou_status_id, moa_id) t9
                        on t9.moa_id = t1.moa_id
                        where t1.mou_id = @mou_id
                        and t3.partner_name like @partner_name
                        and t1.moa_code like @moa_code";
                List<ListMOA> moaList = db.Database.SqlQuery<ListMOA>(sql_mouList,
                    new SqlParameter("mou_id", mou_id),
                    new SqlParameter("partner_name", '%' + partner_name + '%'),
                    new SqlParameter("moa_code", '%' + moa_code + '%')).ToList();
                handlingMOAListData(moaList);
                return moaList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void handlingMOAListData(List<ListMOA> moaList)
        {
            ListMOA previousItem = null;
            foreach (ListMOA item in moaList.ToList())
            {
                if (previousItem == null) //first record
                {
                    previousItem = item;
                    previousItem.moa_start_date_string = item.moa_start_date.ToString("dd'/'MM'/'yyyy");
                    previousItem.moa_end_date_string = item.moa_end_date.ToString("dd'/'MM'/'yyyy");
                }
                else
                {
                    if (item.moa_partner_id.Equals(previousItem.moa_partner_id))
                    {
                        if (!previousItem.scope_abbreviation.Contains(item.scope_abbreviation))
                        {
                            previousItem.scope_abbreviation = previousItem.scope_abbreviation + "," + item.scope_abbreviation;
                        }
                        //then remove current object
                        moaList.Remove(item);
                    }
                    else
                    {
                        previousItem = item;
                    }
                }
            }
            return;
        }
        //public void addMOA(MOAAdd input)
        //{
        //    using (DbContextTransaction transaction = db.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            db.MOAs.Add(input.MOA);
        //            db.MOAStatusHistories.Add(input.MOAStatusHistory);
        //            foreach (MOAPartner item in input.listMOAPartner)
        //            {
        //                db.MOAPartners.Add(item);
        //            }
        //            foreach (MOAPartnerSpecialization item in input.listMOAPartnerSpe)
        //            {
        //                db.MOAPartnerSpecializations.Add(item);
        //            }
        //            foreach (MOAPartnerScope item in input.listMOAPartnerScope)
        //            {
        //                db.MOAPartnerScopes.Add(item);
        //            }
        //            db.SaveChanges();
        //            transaction.Commit();
        //        }
        //        catch (Exception ex)
        //        {
        //            transaction.Rollback();
        //            throw ex;
        //        }
        //    }
        //}
        public void deleteMOA(int moa_id)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    MOA moa = db.MOAs.Find(moa_id);
                    moa.is_deleted = true;
                    db.Entry(moa).State = EntityState.Modified;
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
        public bool partnerIsExistedMOA(int partner_id)
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
        public string getSuggestedMOACode(int moa_id)
        {
            try
            {
                string sql_countInYear = @"select count(*) from IA_Collaboration.MOU where mou_code like @year";
                string sql_checkDup = @"select count(*) from IA_Collaboration.MOABonus where moa_bonus_code = @newCode";
                bool isDuplicated = false;
                string newCode = "";
                int countInYear = db.Database.SqlQuery<int>(sql_countInYear,
                        new SqlParameter("year", '%' + DateTime.Now.Year + '%')).First();
                int countInMOA = db.MOABonus.Where(x => x.moa_id == moa_id).Count();

                //fix duplicate mou_code:
                countInYear++;
                do
                {
                    countInMOA++;
                    newCode = DateTime.Now.Year + "/" + countInYear + "_MOA/" + countInMOA;
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
        public List<CollaborationScope> getMOAScope()
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
        public List<ENTITIES.Partner> GetMOAPartners(int mou_id)
        {
            try
            {
                string sql_partnerList = @"select t2.* from IA_Collaboration.MOUPartner t1
                    inner join IA_Collaboration.Partner t2 
                    on t2.partner_id = t1.partner_id
                    where t1.mou_id = @mou_id
                    order by t2.partner_id";
                List<ENTITIES.Partner> partnerList = db.Database.SqlQuery<ENTITIES.Partner>(sql_partnerList,
                    new SqlParameter("mou_id",mou_id)).ToList();
                return partnerList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public class ListMOA
        {
            public ListMOA() { }
            public string moa_code { get; set; }
            public int moa_partner_id { get; set; }
            public int moa_id { get; set; }
            public string partner_name { get; set; }
            public string evidence { get; set; }
            public DateTime moa_start_date { get; set; }
            public DateTime moa_end_date { get; set; }
            public string moa_start_date_string { get; set; }
            public string moa_end_date_string { get; set; }
            public string office_name { get; set; }
            public string scope_abbreviation { get; set; }
            public string mou_status_name { get; set; }
            public int mou_status_id { get; set; }
        }
    }
}
