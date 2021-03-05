using ENTITIES;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BLL.InternationalCollaboration.Collaboration.MemorandumOfAgreement.BasicInfoMOARepo.ExtraMOA;

namespace BLL.InternationalCollaboration.Collaboration.MemorandumOfAgreement
{
    class BasicInfoMOARepo
    {
        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        public MOABasicInfo getBasicInfoMOA(int moa_id)
        {
            try
            {
                string sql_moaBasicInfo =
                    @"select 
                        t1.moa_id,t1.moa_code,t7.office_abbreviation,t1.moa_end_date,
                        t5.mou_status_name,t4.reason,t1.evidence,t1.moa_note,t5.mou_status_id
                        from IA_Collaboration.MOA t1
                        inner join
                        (select max([datetime]) as 'maxdate',mou_status_id, mou_id
                        from IA_Collaboration.MOUStatusHistory 
                        group by mou_status_id, mou_id) t3 on t3.mou_id = t1.mou_id
                        inner join IA_Collaboration.MOUStatusHistory t4 on 
                        t4.datetime = t3.maxdate and t4.mou_id = t4.mou_id and t4.mou_status_id = t3.mou_status_id
                        inner join IA_Collaboration.MOUStatus t5 on
                        t5.mou_status_id = t3.mou_status_id
                        inner join IA_Collaboration.MOU t6 on
                        t6.mou_id = t1.mou_id
                        inner join General.Office t7 on
                        t7.office_id = t6.office_id
                        where t1.mou_id = @mou_id ";
                string sql_mouStartDateAndScopes =
                    @"";
                MOABasicInfo basicInfo = db.Database.SqlQuery<MOABasicInfo>(sql_moaBasicInfo,
                    new SqlParameter("moa_id", moa_id)).First();
                MOABasicInfo dateAndScopes = db.Database.SqlQuery<MOABasicInfo>(sql_mouStartDateAndScopes,
                    new SqlParameter("moa_id", moa_id)).First();
                handlingMOAData(basicInfo, dateAndScopes);
                return basicInfo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<ExtraMOA> listAllExtraMOA()
        {
            try
            {
                string sql_moaExList =
                    @"select t1.mou_bonus_code, t1.mou_bonus_decision_date,t1.mou_bonus_end_date,
                        t3.partner_name,t4.scope_abbreviation,t1.evidence,t1.mou_id
                        from IA_Collaboration.MOUBonus t1 left join 
                        IA_Collaboration.MOUPartnerScope t2 on 
                        t1.mou_bonus_id = t2.mou_bonus_id inner join 
                        IA_Collaboration.Partner t3 on t3.partner_id = t2.partner_id
                        inner join IA_MasterData.CollaborationScope t4 on t4.scope_id = t2.scope_id";
                List<ExtraMOA> moaExList = db.Database.SqlQuery<ExtraMOA>(sql_moaExList).ToList();
                handlingExMOAListData(moaExList);
                return moaExList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void handlingExMOAListData(List<ExtraMOA> moaExList)
        {
            ExtraMOA previousItem = null;
            foreach (ExtraMOA item in moaExList.ToList())
            {
                if (previousItem == null) //first record
                {
                    previousItem = item;
                    previousItem.moa_bonus_decision_date_string = item.moa_bonus_decision_date.ToString("dd'/'MM'/'yyyy");
                    previousItem.moa_bonus_end_date_string = item.moa_bonus_end_date.ToString("dd'/'MM'/'yyyy");
                }
                else
                {
                    if (item.moa_id.Equals(previousItem.moa_id))
                    {
                        if (!previousItem.scope_abbreviation.Contains(item.scope_abbreviation))
                        {
                            previousItem.scope_abbreviation = previousItem.scope_abbreviation + "," + item.scope_abbreviation;
                        }
                        //then remove current object
                        moaExList.Remove(item);
                    }
                    else
                    {
                        previousItem = item;
                    }
                }
            }
        }
        private void handlingMOAData(MOABasicInfo basicInfo, MOABasicInfo dateAndScopes)
        {
            //handle date display
            basicInfo.moa_end_date_string = basicInfo.moa_end_date.ToString("dd'/'MM'/'yyyy");
            basicInfo.moa_start_date_string = dateAndScopes.moa_start_date.ToString("dd'/'MM'/'yyyy");
            basicInfo.scopes = dateAndScopes.scopes;
        }
        public void editMOABasicInfo(int moa_id, MOABasicInfo newBasicInfo)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    string sql_maxDate = @"select t2.* from
                    (
                    select max([datetime]) as 'maxdate',mou_status_id, moa_id
                    from IA_Collaboration.MOAStatusHistory 
                    where moa_id = @moa_id
                    group by mou_status_id, moa_id) t1 
                    inner join IA_Collaboration.MOAStatusHistory t2
                    on t2.moa_id = t1.moa_id and t2.mou_status_id = t1.mou_status_id";
                    //update basicInfo
                    MOA moa = db.MOAs.Find(moa_id);
                    moa.moa_code = newBasicInfo.moa_code;
                    moa.moa_end_date = newBasicInfo.moa_end_date;
                    moa.moa_note = newBasicInfo.moa_note;
                    moa.evidence = newBasicInfo.evidence;
                    db.Entry(moa).State = EntityState.Modified;

                    MOAStatusHistory oldInfo = db.Database.SqlQuery<MOAStatusHistory>(sql_maxDate,
                        new SqlParameter("moa_id", moa_id)).First();
                    if (oldInfo.reason is null)
                    {
                        oldInfo.reason = "";
                    }
                    if (!(oldInfo.reason.Equals(newBasicInfo.reason) && oldInfo.mou_status_id == newBasicInfo.moa_status_id))
                    {
                        MOAStatusHistory m = new MOAStatusHistory();
                        m.mou_status_id = newBasicInfo.moa_status_id;
                        m.reason = newBasicInfo.reason;
                        m.moa_id = newBasicInfo.moa_id;
                        m.date = DateTime.Now;
                        db.MOAStatusHistories.Add(m);
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
        public void addExtraMOA(ExtraMOA input)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    //add MOUBonus
                    MOABonu mb = new MOABonu();
                    mb.moa_bonus_code = input.moa_bonus_code;
                    mb.moa_bonus_decision_date = input.moa_bonus_decision_date;
                    mb.moa_bonus_end_date = input.moa_bonus_end_date;
                    mb.moa_id = input.moa_id;
                    mb.evidence = input.evidence;
                    db.MOABonus.Add(mb);
                    MOABonu addObj = db.MOABonus.Where(x => x.moa_bonus_code.Equals(mb.moa_bonus_code)).First();

                    //add MOuPartnerScope
                    foreach (CustomPartner cp in input.ListPartnerExMOA.ToList())
                    {
                        foreach (CustomScope cs in cp.ListScopeExMOU.ToList())
                        {
                            MOUPartnerScope m = new MOUPartnerScope();
                            m.mou_id = input.moa_id;
                            m.partner_id = cp.partner_id;
                            m.scope_id = cs.scope_id;
                            m.mou_bonus_id = addObj.moa_bonus_id;
                            db.MOUPartnerScopes.Add(m);
                        }
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
        public void editExtraMOA(ExtraMOA input, int moa_bonus_id)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    //edit MOUBonus
                    MOABonu mb = db.MOABonus.Find(moa_bonus_id);
                    mb.moa_bonus_code = input.moa_bonus_code;
                    mb.moa_bonus_decision_date = input.moa_bonus_decision_date;
                    mb.moa_bonus_end_date = input.moa_bonus_end_date;
                    mb.moa_id = input.moa_id;
                    mb.evidence = input.evidence;
                    db.Entry(mb).State = EntityState.Modified;

                    //finding old exScope of exMOU.
                    List<MOAPartnerScope> exList = db.MOAPartnerScopes.Where(x => x.moa_bonus_id == moa_bonus_id).ToList();
                    exList.Clear();
                    db.Entry(exList).State = EntityState.Modified;

                    //add new record of MOuPartnerScope
                    foreach (CustomPartner cp in input.ListPartnerExMOA.ToList())
                    {
                        foreach (CustomScope cs in cp.ListScopeExMOU.ToList())
                        {
                            MOUPartnerScope m = new MOUPartnerScope();
                            m.mou_id = input.moa_id;
                            m.partner_id = cp.partner_id;
                            m.scope_id = cs.scope_id;
                            m.mou_bonus_id = moa_bonus_id;
                            db.MOUPartnerScopes.Add(m);
                        }
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
        public void deleteExtraMOA(int moa_bonus_id)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    //finding old exScope of exMOU.
                    List<MOAPartnerScope> exList = db.MOAPartnerScopes.Where(x => x.moa_bonus_id == moa_bonus_id).ToList();
                    exList.Clear();
                    db.Entry(exList).State = EntityState.Modified;

                    //add new record of MOuPartnerScope
                    MOABonu m = db.MOABonus.Find(moa_bonus_id);
                    db.MOABonus.Remove(m);

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
        public string getNewExMOACode(int mou_id)
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
        public List<Specialization> getMOASpecialization()
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
        public List<CollaborationScope> getExMOACollabScope()
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
        public class MOABasicInfo
        {
            public MOABasicInfo() { }
            public int moa_id { get; set; }
            public string moa_code { get; set; }
            public string evidence { get; set; }
            public string scopes { get; set; }
            public string reason { get; set; }
            public DateTime moa_end_date { get; set; }
            public DateTime moa_start_date { get; set; }
            public string moa_end_date_string { get; set; }
            public string moa_start_date_string { get; set; }
            public string office_abbreviation { get; set; }
            public string moa_status_name { get; set; }
            public int office_id { get; set; }
            public int moa_status_id { get; set; }
            public string moa_note { get; set; }
        }
        public class ExtraMOA
        {
            public string moa_bonus_code { get; set; }
            public string moa_bonus_decision_date_string { get; set; }
            public string moa_bonus_end_date_string { get; set; }
            public DateTime moa_bonus_end_date { get; set; }
            public DateTime moa_bonus_decision_date { get; set; }
            public string partner_name { get; set; }
            public string scope_abbreviation { get; set; }
            public string evidence { get; set; }
            public int moa_id { get; set; }
            public int partner_id { get; set; }
            public int scope_id { get; set; }
            public List<CustomPartner> ListPartnerExMOA { get; set; }
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
        }
    }
}
