using ENTITIES;
using ENTITIES.CustomModels.InternationalCollaboration.Collaboration.MemorandumOfAgreement.MOABasicInfo;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.InternationalCollaboration.Collaboration.MemorandumOfAgreement
{
    public class BasicInfoMOARepo
    {
        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        public MOABasicInfo getBasicInfoMOA(int moa_id)
        {
            try
            {
                string sql_moaBasicInfo =
                    @"select 
                        moa.moa_id, moa.moa_code, mou.office_abbreviation, moap.moa_start_date, moa.moa_end_date,
                        moah.mou_status_id as moa_status_id, moah.reason, moaps.scope_abbreviation, moa.evidence, moa.moa_note
                        from IA_Collaboration.MOA moa
                        inner join
	                        (select moa_id, max(moa_start_date) 'moa_start_date'
	                        from IA_Collaboration.MOAPartner
	                        where moa_id = @moa_id
	                        group by moa_id) as moap on moap.moa_id = moa.moa_id
                        inner join
	                        (select moah1.moa_id, cs.mou_status_id, moah2.reason, moah1.[datetime]  
	                        from
	                        (select moa_id, max([datetime]) 'datetime'
	                        from IA_Collaboration.MOAStatusHistory
	                        group by moa_id) as moah1
	                        left join
	                        IA_Collaboration.MOAStatusHistory moah2 on moah1.moa_id = moah2.moa_id and moah1.[datetime] = moah2.[datetime] 
	                        left join
	                        IA_Collaboration.CollaborationStatus cs on cs.mou_status_id = moah2.mou_status_id
	                        where moah1.moa_id = @moa_id) as moah on moah.moa_id = moa.moa_id
                        inner join
	                        (select moaps.moa_id, ps.partner_id, cs.scope_abbreviation
	                        from IA_Collaboration.MOAPartnerScope moaps
	                        inner join IA_Collaboration.PartnerScope ps on moaps.partner_scope_id = ps.partner_scope_id
	                        inner join IA_Collaboration.CollaborationScope cs on cs.[scope_id] = ps.[scope_id]
	                        where moaps.moa_id = @moa_id) as moaps on moaps.moa_id = moa.moa_id
                        inner join
	                        (select mou.mou_id, offi.office_abbreviation
	                        from IA_Collaboration.MOU mou
                            inner join General.Office offi on offi.office_id = mou.office_id) as mou on mou.mou_id = moa.mou_id ";
                List<MOABasicInfo> basicInfo = db.Database.SqlQuery<MOABasicInfo>(sql_moaBasicInfo,
                        new SqlParameter("moa_id", moa_id)).ToList();
                handlingMOAData(basicInfo);
                return basicInfo[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<ExtraMOA> listAllExtraMOA(int moa_id)
        {
            try
            {
                string sql_moaExList =
                    @"select 
                        moab.moa_bonus_id, moab.moa_bonus_code, moab.moa_bonus_decision_date, moab.moa_bonus_end_date,
                        pa.partner_name, cs.scope_abbreviation
                        from IA_Collaboration.MOABonus moab
                        left join IA_Collaboration.MOAPartnerScope moaps on moab.moa_bonus_id = moaps.moa_bonus_id and moab.moa_id = moaps.moa_id
                        left join IA_Collaboration.PartnerScope ps on ps.partner_scope_id = moaps.partner_scope_id
                        left join IA_Collaboration.[Partner] pa on pa.partner_id = ps.partner_id
                        left join IA_Collaboration.CollaborationScope cs on cs.[scope_id] = ps.[scope_id]
                        where moab.moa_id = @moa_id";
                List<ExtraMOA> moaExList = db.Database.SqlQuery<ExtraMOA>(sql_moaExList,
                    new SqlParameter("moa_id", moa_id)).ToList();
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
                        previousItem.moa_bonus_decision_date_string = item.moa_bonus_decision_date.ToString("dd'/'MM'/'yyyy");
                        previousItem.moa_bonus_end_date_string = item.moa_bonus_end_date.ToString("dd'/'MM'/'yyyy");
                    }
                }
            }
        }
        private void handlingMOAData(List<MOABasicInfo> basicInfo)
        {
            MOABasicInfo previousItem = null;
            foreach (MOABasicInfo item in basicInfo.ToList())
            {
                if (previousItem == null) //first record
                {
                    previousItem = item;
                    previousItem.moa_start_date_string = item.moa_start_date.ToString("dd'/'MM'/'yyyy");
                    previousItem.moa_end_date_string = item.moa_end_date.ToString("dd'/'MM'/'yyyy");
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
                        basicInfo.Remove(item);
                    }
                    else
                    {
                        previousItem = item;
                        previousItem.moa_start_date_string = item.moa_start_date.ToString("dd'/'MM'/'yyyy");
                        previousItem.moa_end_date_string = item.moa_end_date.ToString("dd'/'MM'/'yyyy");
                    }
                }
            }
        }
        public void editMOABasicInfo(int moa_id, MOABasicInfo newBasicInfo)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    DateTime end_date = DateTime.ParseExact(newBasicInfo.moa_end_date_string, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    //update basicInfo
                    MOA moa = db.MOAs.Find(moa_id);
                    moa.moa_code = newBasicInfo.moa_code;
                    moa.moa_end_date = end_date;
                    moa.moa_note = newBasicInfo.moa_note;
                    //moa.evidence = newBasicInfo.evidence;
                    db.Entry(moa).State = EntityState.Modified;
                    db.SaveChanges();

                    db.MOAStatusHistories.Add(new MOAStatusHistory
                    {
                        mou_status_id = newBasicInfo.moa_status_id,
                        reason = newBasicInfo.reason,
                        moa_id = moa_id,
                        datetime = DateTime.Now
                    });

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
        public List<CollaborationScope> GetScopesExMOA(int moa_id, int mou_id, int partner_id)
        {
            try
            {
                string sql_scopeList = @"select tb3.* from(
					select * from(
					select distinct scope_id from IA_Collaboration.MOUPartnerScope t1 left join
                    IA_Collaboration.PartnerScope t2 on 
                    t1.partner_scope_id = t2.partner_scope_id
                    where mou_id = @mou_id and partner_id = @partner_id
					) tb1 where scope_id not in(
					select distinct scope_id from IA_Collaboration.MOAPartnerScope t1 left join
                    IA_Collaboration.PartnerScope t2 on 
                    t1.partner_scope_id = t2.partner_scope_id
					where moa_id = @moa_id and moa_bonus_id is null and partner_id = @partner_id)) tb2 inner join IA_Collaboration.CollaborationScope tb3
					on tb3.scope_id = tb2.scope_id";
                List<CollaborationScope> scopeList = db.Database.SqlQuery<CollaborationScope>(sql_scopeList,
                    new SqlParameter("mou_id", mou_id),
                    new SqlParameter("moa_id", moa_id),
                    new SqlParameter("partner_id", partner_id)).ToList();
                return scopeList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<ENTITIES.Partner> getPartnerExMOA(int moa_id)
        {
            try
            {
                string sql_partnerList = @"select t2.*  
                    from IA_Collaboration.MOAPartner t1 left join 
                    IA_Collaboration.Partner t2 on t2.partner_id = t1.partner_id
                    where t1.moa_id = @moa_id";
                List<ENTITIES.Partner> partnerList = db.Database.SqlQuery<ENTITIES.Partner>(sql_partnerList,
                    new SqlParameter("moa_id", moa_id)).ToList();
                return partnerList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void addExtraMOA(ExMOAAdd input, int moa_id, BLL.Authen.LoginRepo.User user)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    List<PartnerScope> totalRelatedPS = new List<PartnerScope>();
                    DateTime sign_date = DateTime.ParseExact(input.ExMOABasicInfo.ex_moa_sign_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    DateTime end_date = DateTime.ParseExact(input.ExMOABasicInfo.ex_moa_end_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    //add MOABonus
                    MOABonu objMOABonusAdded = db.MOABonus.Add(new MOABonu
                    {
                        moa_bonus_code = input.ExMOABasicInfo.ex_moa_code,
                        moa_bonus_decision_date = sign_date,
                        moa_bonus_end_date = end_date,
                        moa_id = moa_id,
                        account_id = user is null ? 1 : user.account.account_id,
                        add_time = DateTime.Now
                        //,
                        //evidence = ""
                    });
                    db.SaveChanges();
                    //check PartnerScope and add MOAPartnerScope.
                    if (input.PartnerScopeInfoMOA != null)
                    {
                        foreach (PartnerScopeInfoMOA psMOA in input.PartnerScopeInfoMOA.ToList())
                        {
                            foreach (int scopeItem in psMOA.scopes_id.ToList())
                            {
                                int partner_scope_id_item = 0;
                                PartnerScope psCheck = db.PartnerScopes.Where(x => x.partner_id == psMOA.partner_id && x.scope_id == scopeItem).First();
                                if (psCheck == null)
                                {
                                    //PartnerScope psAdded = db.PartnerScopes.Add(new PartnerScope
                                    //{
                                    //    partner_id = psMOA.partner_id,
                                    //    scope_id = scopeItem,
                                    //    reference_count = 1
                                    //});
                                    //partner_scope_id_item = psAdded.partner_scope_id;
                                }
                                else
                                {
                                    partner_scope_id_item = psCheck.partner_scope_id;
                                    //psCheck.reference_count += 1;
                                    totalRelatedPS.Add(psCheck);
                                }
                                db.SaveChanges();
                                //add to MOAPartnerScope
                                db.MOAPartnerScopes.Add(new MOAPartnerScope
                                {
                                    partner_scope_id = partner_scope_id_item,
                                    moa_id = moa_id,
                                    moa_bonus_id = objMOABonusAdded.moa_bonus_id
                                });
                                db.SaveChanges();
                            }
                        }
                    }
                    db.SaveChanges();
                    transaction.Commit();

                    //change status corressponding MOU/MOA
                    using (DbContextTransaction dbContext = db.Database.BeginTransaction())
                    {
                        try
                        {
                            List<int> listPS = totalRelatedPS.Select(x => x.partner_scope_id).Distinct().ToList();
                            new AutoActiveInactive().changeStatusMOUMOA(listPS, db);
                            dbContext.Commit();
                        }
                        catch (Exception e)
                        {
                            dbContext.Rollback();
                            throw e;
                        }
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }
        public void editExtraMOA(ExMOAAdd input, BLL.Authen.LoginRepo.User user)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    List<PartnerScope> totalRelatedPS = new List<PartnerScope>();
                    DateTime sign_date = DateTime.ParseExact(input.ExMOABasicInfo.ex_moa_sign_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    DateTime end_date = DateTime.ParseExact(input.ExMOABasicInfo.ex_moa_end_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    //edit MOABonus
                    MOABonu mb = db.MOABonus.Find(input.moa_bonus_id);
                    mb.moa_bonus_code = input.ExMOABasicInfo.ex_moa_code;
                    mb.moa_bonus_decision_date = sign_date;
                    mb.moa_bonus_end_date = end_date;
                    mb.account_id = user is null ? 1 : user.account.account_id;
                    mb.add_time = DateTime.Now;
                    db.Entry(mb).State = EntityState.Modified;

                    List<MOAPartnerScope> moaPSList = db.MOAPartnerScopes.Where(x => x.moa_bonus_id == input.moa_bonus_id).ToList();
                    foreach (MOAPartnerScope moaPSItem in moaPSList.ToList())
                    {
                        //decrese ref_count of old PartnerScope records.
                        PartnerScope oldPS = db.PartnerScopes.Find(moaPSItem.partner_scope_id);
                        //oldPS.reference_count -= 1;
                        //db.Entry(oldPS).State = EntityState.Modified;
                        totalRelatedPS.Add(oldPS);
                    }
                    //del records of MOUPartnerScope.
                    db.MOAPartnerScopes.RemoveRange(moaPSList);
                    db.SaveChanges();

                    //Check partnerScope existed and handle it.
                    //add data to MOUPartnerScope.
                    //check PartnerScope is null or not
                    if (input.PartnerScopeInfoMOA != null)
                    {
                        foreach (PartnerScopeInfoMOA psi in input.PartnerScopeInfoMOA.ToList())
                        {
                            foreach (int scope in psi.scopes_id.ToList())
                            {
                                PartnerScope psCheck = db.PartnerScopes.Where(x => x.partner_id == psi.partner_id && x.scope_id == scope).FirstOrDefault();
                                if (psCheck is null)
                                {
                                    //PartnerScope psAdded = db.PartnerScopes.Add(new PartnerScope
                                    //{
                                    //    partner_id = psi.partner_id,
                                    //    scope_id = scope,
                                    //    reference_count = 1
                                    //});
                                    //db.MOAPartnerScopes.Add(new MOAPartnerScope
                                    //{
                                    //    partner_scope_id = psAdded.partner_scope_id,
                                    //    moa_id = mb.moa_id,
                                    //    moa_bonus_id = input.moa_bonus_id
                                    //});
                                }
                                else
                                {
                                    //psCheck.reference_count += 1;
                                    db.MOAPartnerScopes.Add(new MOAPartnerScope
                                    {
                                        partner_scope_id = psCheck.partner_scope_id,
                                        moa_id = mb.moa_id,
                                        moa_bonus_id = input.moa_bonus_id
                                    });
                                    totalRelatedPS.Add(psCheck);
                                }
                            }
                        }
                    }

                    //checkpoint 2
                    db.SaveChanges();
                    transaction.Commit();

                    //change status corressponding MOU/MOA
                    using (DbContextTransaction dbContext = db.Database.BeginTransaction())
                    {
                        try
                        {
                            List<int> listPS = totalRelatedPS.Select(x => x.partner_scope_id).Distinct().ToList();
                            new AutoActiveInactive().changeStatusMOUMOA(listPS, db);
                            dbContext.Commit();
                        }
                        catch (Exception e)
                        {
                            dbContext.Rollback();
                            throw e;
                        }
                    }
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
                    List<int> listPS = exList.Select(x => x.partner_scope_id).Distinct().ToList();
                    db.MOAPartnerScopes.RemoveRange(exList);
                    db.SaveChanges();

                    //add new record of MOuPartnerScope
                    MOABonu m = db.MOABonus.Find(moa_bonus_id);
                    db.MOABonus.Remove(m);

                    db.SaveChanges();
                    transaction.Commit();

                    //change status corressponding MOU/MOA
                    using (DbContextTransaction dbContext = db.Database.BeginTransaction())
                    {
                        try
                        {
                            new AutoActiveInactive().changeStatusMOUMOA(listPS, db);
                            dbContext.Commit();
                        }
                        catch (Exception e)
                        {
                            dbContext.Rollback();
                            throw e;
                        }
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
            return;
        }
        public string getNewExMOACode(int moa_id)
        {
            try
            {
                bool isDuplicated = false;
                string newCode = "";
                string baseMOACode = db.MOAs.Find(moa_id).moa_code;
                int countInMOA = db.MOABonus.Where(x => x.moa_id == moa_id).Count();

                //fix duplicate moa_code:
                do
                {
                    countInMOA++;
                    newCode = baseMOACode + "_BS/" + countInMOA;
                    isDuplicated = db.MOABonus.Where(x => x.moa_bonus_code.Equals(newCode)).FirstOrDefault() is null ? false : true;
                } while (isDuplicated);
                return newCode;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ExMOAAdd getExtraMOADetail(int moa_id, int moa_bonus_id, int mou_id)
        {
            try
            {
                string sql_moaEx =
                    @"select t1.moa_bonus_code, t1.moa_bonus_decision_date,t1.moa_bonus_end_date,
                        t4.partner_name,t5.scope_abbreviation,t1.evidence,t1.moa_id,t1.moa_bonus_id,
                        isnull(t5.scope_id,0) as scope_id ,isnull(t4.partner_id,0) as partner_id
                        from IA_Collaboration.MOABonus t1 left join 
                        IA_Collaboration.MOAPartnerScope t2 on 
                        t1.moa_bonus_id = t2.moa_bonus_id left join 
                        IA_Collaboration.PartnerScope t3 on
                        t3.partner_scope_id = t2.partner_scope_id
                        left join 
                        IA_Collaboration.Partner t4 on t4.partner_id = t3.partner_id
                        left join IA_Collaboration.CollaborationScope t5 on t5.scope_id = t3.scope_id
                        where t1.moa_id = @moa_id and t1.moa_bonus_id = @moa_bonus_id order by partner_id";
                List<ExtraMOA> moaExList = db.Database.SqlQuery<ExtraMOA>(sql_moaEx
                    , new SqlParameter("moa_id", moa_id)
                    , new SqlParameter("moa_bonus_id", moa_bonus_id)).ToList();
                ExMOAAdd moaEx = handlingExMOADetailData(moaExList);
                if (moaEx.PartnerScopeInfoMOA != null)
                {
                    foreach (PartnerScopeInfoMOA item in moaEx.PartnerScopeInfoMOA.ToList())
                    {
                        item.total_scopes = new List<CollaborationScope>();
                        item.total_scopes = GetScopesExMOA(moa_id, mou_id, item.partner_id);
                    }
                }
                return moaEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private ExMOAAdd handlingExMOADetailData(List<ExtraMOA> mouExList)
        {
            ExMOAAdd newObj = new ExMOAAdd();
            newObj.ExMOABasicInfo = new ExMOABasicInfo();
            newObj.PartnerScopeInfoMOA = new List<PartnerScopeInfoMOA>();
            //Partner
            foreach (ExtraMOA item in mouExList.ToList())
            {
                newObj.ExMOABasicInfo.ex_moa_code = item.moa_bonus_code;
                newObj.ExMOABasicInfo.ex_moa_end_date = item.moa_bonus_end_date.ToString("dd'/'MM'/'yyyy");
                newObj.ExMOABasicInfo.ex_moa_sign_date = item.moa_bonus_decision_date.ToString("dd'/'MM'/'yyyy");
                PartnerScopeInfoMOA p = newObj.PartnerScopeInfoMOA.Find(x => x.partner_id == item.partner_id);
                if (p == null)
                {
                    PartnerScopeInfoMOA obj = new PartnerScopeInfoMOA();
                    obj.partner_id = item.partner_id;
                    obj.scopes_id = new List<int>();
                    obj.scopes_id.Add(item.scope_id);
                    obj.scopes_name = item.scope_abbreviation;
                    obj.partner_name = item.partner_name;
                    newObj.PartnerScopeInfoMOA.Add(obj);
                }
                else
                {
                    p.scopes_id.Add(item.scope_id);
                    p.scopes_name += "," + item.scope_abbreviation;
                }
            }
            //Case: remove PartnerScopeInfoMOA if null
            if (newObj.PartnerScopeInfoMOA.Count == 1)
            {
                if (newObj.PartnerScopeInfoMOA[0].partner_id == 0)
                {
                    newObj.PartnerScopeInfoMOA.RemoveAt(0);
                }
            }
            return newObj;
        }
    }
}
