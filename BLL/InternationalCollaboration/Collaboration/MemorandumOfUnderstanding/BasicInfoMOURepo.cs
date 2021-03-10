using ENTITIES;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding
{
    public class BasicInfoMOURepo
    {
        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        public MOUBasicInfo getBasicInfoMOU(int mou_id)
        {
            try
            {
                string sql_mouBasicInfo =
                    @"select 
                        t1.mou_id,t1.mou_code,t2.office_abbreviation,t1.mou_end_date,
                        t5.mou_status_name,t4.reason,t1.evidence,t1.mou_note,
                        t1.office_id,t5.mou_status_id
                        from IA_Collaboration.MOU t1
                        inner join General.Office t2 on t1.office_id = t2.office_id
                        inner join
                        (select max([datetime]) as 'maxdate',mou_status_id, mou_id
                        from IA_Collaboration.MOUStatusHistory 
                        group by mou_status_id, mou_id) t3 on t3.mou_id = t1.mou_id
                        inner join IA_Collaboration.MOUStatusHistory t4 on 
                        t4.datetime = t3.maxdate and t4.mou_id = t4.mou_id and t4.mou_status_id = t3.mou_status_id
                        inner join IA_Collaboration.CollaborationStatus t5 on
                        t5.mou_status_id = t3.mou_status_id
                        where t1.mou_id = @mou_id ";
                string sql_mouStartDateAndScopes =
                    @"select t2.*,t3.mou_start_date from
                        (select mou_id, STRING_AGG(scope_abbreviation,', ') as scopes from
                        (select distinct mou_id,scope_abbreviation from 
                        (select mou_id,partner_id, scope_id
                        from IA_Collaboration.MOUPartnerScope t1a left join 
                        IA_Collaboration.PartnerScope t2a 
                        on t2a.partner_scope_id = t1a.partner_scope_id) tb1a
                        left join IA_MasterData.CollaborationScope tb1b on
                        tb1a.scope_id = tb1b.scope_id
                        where mou_id = @mou_id) t1
                        group by mou_id) t2
                        left join 
                        (select max(mou_start_date) as mou_start_date,mou_id
                        from IA_Collaboration.MOUPartner
                        where mou_id = @mou_id
                        group by mou_id) t3
                        on t3.mou_id = t2.mou_id ";
                MOUBasicInfo basicInfo = db.Database.SqlQuery<MOUBasicInfo>(sql_mouBasicInfo,
                    new SqlParameter("mou_id", mou_id)).First();
                MOUBasicInfo dateAndScopes = db.Database.SqlQuery<MOUBasicInfo>(sql_mouStartDateAndScopes,
                    new SqlParameter("mou_id", mou_id)).First();
                handlingMOUData(basicInfo, dateAndScopes);
                return basicInfo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void handlingMOUData(MOUBasicInfo basicInfo, MOUBasicInfo dateAndScopes)
        {
            //handle date display
            basicInfo.mou_end_date_string = basicInfo.mou_end_date.ToString("dd'/'MM'/'yyyy");
            basicInfo.mou_start_date_string = dateAndScopes.mou_start_date.ToString("dd'/'MM'/'yyyy");
            basicInfo.scopes = dateAndScopes.scopes;
        }
        public List<ExtraMOU> listAllExtraMOU(int mou_id)
        {
            try
            {
                string sql_mouExList =
                    @"select t1.mou_bonus_code, t1.mou_bonus_decision_date,t1.mou_bonus_end_date,
                        t4.partner_name,t5.scope_abbreviation,t1.evidence,t1.mou_id,t1.mou_bonus_id
                        from IA_Collaboration.MOUBonus t1 left join 
                        IA_Collaboration.MOUPartnerScope t2 on 
                        t1.mou_bonus_id = t2.mou_bonus_id inner join 
                        IA_Collaboration.PartnerScope t3 on
                        t3.partner_scope_id = t2.partner_scope_id
                        inner join 
                        IA_Collaboration.Partner t4 on t4.partner_id = t3.partner_id
                        inner join IA_MasterData.CollaborationScope t5 on t5.scope_id = t3.scope_id
                        where t1.mou_id = @mou_id";
                List<ExtraMOU> mouExList = db.Database.SqlQuery<ExtraMOU>(sql_mouExList,
                    new SqlParameter("mou_id", mou_id)).ToList();
                handlingExMOUListData(mouExList);
                return mouExList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void handlingExMOUListData(List<ExtraMOU> mouExList)
        {
            ExtraMOU previousItem = null;
            foreach (ExtraMOU item in mouExList.ToList())
            {
                if (previousItem == null) //first record
                {
                    previousItem = item;
                    previousItem.mou_bonus_decision_date_string = item.mou_bonus_decision_date.ToString("dd'/'MM'/'yyyy");
                    previousItem.mou_bonus_end_date_string = item.mou_bonus_end_date.ToString("dd'/'MM'/'yyyy");
                }
                else
                {
                    if (item.mou_id.Equals(previousItem.mou_id))
                    {
                        if (!previousItem.scope_abbreviation.Contains(item.scope_abbreviation))
                        {
                            previousItem.scope_abbreviation = previousItem.scope_abbreviation + "," + item.scope_abbreviation;
                        }
                        //then remove current object
                        mouExList.Remove(item);
                    }
                    else
                    {
                        previousItem = item;
                    }
                }
            }
            return;
        }
        public void editMOUBasicInfo(int mou_id, MOUBasicInfo newBasicInfo)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    DateTime mou_end_date = DateTime.ParseExact(newBasicInfo.mou_end_date_string, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    //update basicInfo
                    MOU mou = db.MOUs.Find(mou_id);
                    mou.mou_code = newBasicInfo.mou_code;
                    mou.mou_end_date = mou_end_date;
                    mou.mou_note = newBasicInfo.mou_note;
                    mou.evidence = newBasicInfo.evidence;
                    mou.office_id = newBasicInfo.office_id;
                    db.Entry(mou).State = EntityState.Modified;
                    db.SaveChanges();

                    //update MOUStatusHistory
                    MOUStatusHistory m = new MOUStatusHistory();
                    m.mou_status_id = newBasicInfo.mou_status_id;
                    m.reason = newBasicInfo.reason;
                    m.mou_id = mou_id;
                    m.datetime = DateTime.Now;
                    db.MOUStatusHistories.Add(m);
                    db.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
            return;
        }
        public ExMOUAdd getExtraMOUDetail(int mou_bonus_id, int mou_id)
        {
            try
            {
                string sql_mouEx =
                    @"select t1.mou_bonus_code, t1.mou_bonus_decision_date,t1.mou_bonus_end_date,
                        t4.partner_name,t5.scope_abbreviation,t1.evidence,t1.mou_id,t1.mou_bonus_id,
                        t5.scope_id,t4.partner_id
                        from IA_Collaboration.MOUBonus t1 left join 
                        IA_Collaboration.MOUPartnerScope t2 on 
                        t1.mou_bonus_id = t2.mou_bonus_id inner join 
                        IA_Collaboration.PartnerScope t3 on
                        t3.partner_scope_id = t2.partner_scope_id
                        inner join 
                        IA_Collaboration.Partner t4 on t4.partner_id = t3.partner_id
                        inner join IA_MasterData.CollaborationScope t5 on t5.scope_id = t3.scope_id
                        where t1.mou_id = @mou_id and t1.mou_bonus_id = @mou_bonus_id order by partner_id ";
                List<ExtraMOU> mouExList = db.Database.SqlQuery<ExtraMOU>(sql_mouEx
                    , new SqlParameter("mou_id", mou_id)
                    , new SqlParameter("mou_bonus_id", mou_bonus_id)).ToList();
                ExMOUAdd mouEx = handlingExMOUDetailData(mouExList);
                return mouEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private ExMOUAdd handlingExMOUDetailData(List<ExtraMOU> mouExList)
        {
            ExMOUAdd newObj = new ExMOUAdd();
            //Partner
            foreach (ExtraMOU item in mouExList.ToList())
            {
                newObj.ExBasicInfo.ex_mou_code = item.mou_bonus_code;
                newObj.ExBasicInfo.ex_mou_end_date = item.mou_bonus_end_date.ToString("dd'/'MM'/'yyyy");
                newObj.ExBasicInfo.ex_mou_sign_date = item.mou_bonus_decision_date.ToString("dd'/'MM'/'yyyy");
                PartnerScopeInfo p = newObj.PartnerScopeInfo.Find(x => x.partner_id == item.partner_id);
                if (p == null)
                {
                    PartnerScopeInfo obj = new PartnerScopeInfo();
                    obj.partner_id = item.partner_id;
                    obj.scopes_id.Add(item.scope_id);
                    newObj.PartnerScopeInfo.Add(obj);
                }
                else
                {
                    p.scopes_id.Add(item.scope_id);
                }
            }
            return newObj;
        }

        public void addExtraMOU(ExMOUAdd input, int mou_id)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    DateTime sign_date = DateTime.ParseExact(input.ExBasicInfo.ex_mou_sign_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    DateTime end_date = DateTime.ParseExact(input.ExBasicInfo.ex_mou_end_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    //add MOUBonus
                    MOUBonu mb = new MOUBonu();
                    mb.mou_bonus_code = input.ExBasicInfo.ex_mou_code;
                    mb.mou_bonus_decision_date = sign_date;
                    mb.mou_bonus_end_date = end_date;
                    mb.mou_id = mou_id;
                    mb.evidence = "";
                    db.MOUBonus.Add(mb);
                    //checkpoint 1
                    db.SaveChanges();
                    MOUBonu addObj = db.MOUBonus.Where(x => x.mou_bonus_code.Equals(mb.mou_bonus_code)).First();

                    foreach (PartnerScopeInfo partnerScopeItem in input.PartnerScopeInfo.ToList())
                    {
                        foreach (int scopeItem in partnerScopeItem.scopes_id.ToList())
                        {
                            db.MOUPartnerScopes.Add(new MOUPartnerScope
                            {
                                mou_id = mou_id,
                                mou_bonus_id = addObj.mou_bonus_id
                            });
                        }
                    }
                    //checkpoint 2
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

        //public void editExtraMOU(ExtraMOU input, int mou_bonus_id)
        //{
        //    using (DbContextTransaction transaction = db.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            //edit MOUBonus
        //            MOUBonu mb = db.MOUBonus.Find(mou_bonus_id);
        //            mb.mou_bonus_code = input.mou_bonus_code;
        //            mb.mou_bonus_decision_date = input.mou_bonus_decision_date;
        //            mb.mou_bonus_end_date = input.mou_bonus_end_date;
        //            mb.mou_id = input.mou_id;
        //            mb.evidence = input.evidence;
        //            db.Entry(mb).State = EntityState.Modified;

        //            //finding old exScope of exMOU.
        //            List<MOUPartnerScope> exList = db.MOUPartnerScopes.Where(x => x.mou_bonus_id == mou_bonus_id).ToList();
        //            exList.Clear();
        //            db.Entry(exList).State = EntityState.Modified;

        //            //add new record of MOuPartnerScope
        //            foreach (CustomPartner cp in input.ListPartnerExMOU.ToList())
        //            {
        //                foreach (CustomScope cs in cp.ListScopeExMOU.ToList())
        //                {
        //                    MOUPartnerScope m = new MOUPartnerScope();
        //                    m.mou_id = input.mou_id;
        //                    m.partner_id = cp.partner_id;
        //                    m.scope_id = cs.scope_id;
        //                    m.mou_bonus_id = mou_bonus_id;
        //                    db.MOUPartnerScopes.Add(m);
        //                }
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
        public void deleteExtraMOU(int mou_bonus_id)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    //finding old exScope of exMOU.
                    List<MOUPartnerScope> exList = db.MOUPartnerScopes.Where(x => x.mou_bonus_id == mou_bonus_id).ToList();
                    exList.Clear();
                    db.Entry(exList).State = EntityState.Modified;

                    //add new record of MOuPartnerScope
                    MOUBonu m = db.MOUBonus.Find(mou_bonus_id);
                    db.MOUBonus.Remove(m);

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
        public string getNewExtraMOUCode(int mou_id)
        {
            try
            {
                string sql_countInYear = @"select count(*) from IA_Collaboration.MOU where mou_code like @year";
                string sql_checkDup = @"select count(*) from IA_Collaboration.MOUBonus where mou_bonus_code = @newCode";
                bool isDuplicated = false;
                string newCode = "";
                int countInYear = db.Database.SqlQuery<int>(sql_countInYear,
                        new SqlParameter("year", '%' + DateTime.Now.Year + '%')).First();
                int countInMOU = db.MOUBonus.Where(x => x.mou_id == mou_id).Count();

                //fix duplicate mou_code:
                countInYear++;
                do
                {
                    countInMOU++;
                    newCode = DateTime.Now.Year + "/" + countInYear + "_BS/" + countInMOU;
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
        public List<Office> GetOffice()
        {
            try
            {
                string sql_unitList = @"select * from General.Office";
                List<Office> unitList = db.Database.SqlQuery<Office>(sql_unitList).ToList();
                return unitList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<ENTITIES.Partner> getPartnerExMOU(int mou_id)
        {
            try
            {
                string sql_partnerList = @"select t2.*  
                    from IA_Collaboration.MOUPartner t1 left join 
                    IA_Collaboration.Partner t2 on t2.partner_id = t1.partner_id
                    where t1.mou_id = @mou_id";
                List<ENTITIES.Partner> partnerList = db.Database.SqlQuery<ENTITIES.Partner>(sql_partnerList,
                    new SqlParameter("mou_id", mou_id)).ToList();
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
        public List<CollaborationScope> GetScopesExMOU(int mou_id)
        {
            try
            {
                string sql_scopeList = @"select * from IA_MasterData.CollaborationScope
                    where scope_id not in (
                    select distinct t2.scope_id from IA_Collaboration.MOUPartnerScope t1 left join
                    IA_Collaboration.PartnerScope t2 on 
                    t1.partner_scope_id = t2.partner_scope_id
                    where mou_id = @mou_id)";
                List<CollaborationScope> scopeList = db.Database.SqlQuery<CollaborationScope>(sql_scopeList,
                    new SqlParameter("mou_id", mou_id)).ToList();
                return scopeList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public class MOUBasicInfo
        {
            public MOUBasicInfo() { }
            public int mou_id { get; set; }
            public string mou_code { get; set; }
            public string evidence { get; set; }
            public string scopes { get; set; }
            public string reason { get; set; }
            public DateTime mou_end_date { get; set; }
            public DateTime mou_start_date { get; set; }
            public string mou_end_date_string { get; set; }
            public string mou_start_date_string { get; set; }
            public string mou_note { get; set; }
            public string office_abbreviation { get; set; }
            public string mou_status_name { get; set; }
            public int office_id { get; set; }
            public int mou_status_id { get; set; }
        }
        public class ExtraMOU
        {
            public string mou_bonus_code { get; set; }
            public int mou_bonus_id { get; set; }
            public string mou_bonus_decision_date_string { get; set; }
            public string mou_bonus_end_date_string { get; set; }
            public DateTime mou_bonus_end_date { get; set; }
            public DateTime mou_bonus_decision_date { get; set; }
            public string partner_name { get; set; }
            public string scope_abbreviation { get; set; }
            public string evidence { get; set; }
            public int mou_id { get; set; }
            public int partner_id { get; set; }
            public int scope_id { get; set; }
            public List<CustomPartner> ListPartnerExMOU { get; set; }
        }
        public class CustomPartner
        {
            public CustomPartner(int partner_id, string partner_name)
            {
                this.partner_id = partner_id;
                this.partner_name = partner_name;
            }
            public CustomPartner() { }
            public List<CustomScope> ListScopeExMOU { get; set; }
            public int partner_id { get; set; }
            public string partner_name { get; set; }
        }
        public class CustomScope
        {
            public CustomScope(int scope_id, string scope_name)
            {
                this.scope_id = scope_id;
                this.scope_name = scope_name;
            }
            public CustomScope() { }
            public int scope_id { get; set; }
            public string scope_name { get; set; }
        }
        public class ExMOUAdd
        {
            public ExMOUAdd() { }
            public ExBasicInfo ExBasicInfo = new ExBasicInfo();
            public List<PartnerScopeInfo> PartnerScopeInfo = new List<PartnerScopeInfo>();
        }
        public class ExBasicInfo
        {
            public string ex_mou_code { get; set; }
            public string ex_mou_sign_date { get; set; }
            public string ex_mou_end_date { get; set; }
        }
        public class PartnerScopeInfo
        {
            public List<int> scopes_id = new List<int>();
            public int partner_id { get; set; }
        }
    }
}
