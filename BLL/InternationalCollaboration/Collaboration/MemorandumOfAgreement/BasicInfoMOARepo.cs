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
                        moah.mou_status_name, moah.reason, moaps.scope_abbreviation, moa.evidence, moa.moa_note
                        from IA_Collaboration.MOA moa
                        inner join
	                        (select moa_id, max(moa_start_date) 'moa_start_date'
	                        from IA_Collaboration.MOAPartner
	                        where moa_id = @moa_id
	                        group by moa_id) as moap on moap.moa_id = moa.moa_id
                        inner join
	                        (select moah1.moa_id, cs.mou_status_name, moah2.reason, moah1.[datetime]  
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
                MOABasicInfo basicInfo = db.Database.SqlQuery<MOABasicInfo>(sql_moaBasicInfo,
                        new SqlParameter("moa_id", moa_id)).First();
                //MOABasicInfo dateAndScopes = db.Database.SqlQuery<MOABasicInfo>(sql_mouStartDateAndScopes,
                //    new SqlParameter("moa_id", moa_id)).First();
                handlingMOAData(basicInfo);
                return basicInfo;
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
                        inner join IA_Collaboration.MOAPartnerScope moaps on moab.moa_bonus_id = moaps.moa_bonus_id and moab.moa_id = moaps.moa_id
                        inner join IA_Collaboration.PartnerScope ps on ps.partner_scope_id = moaps.partner_scope_id
                        inner join IA_Collaboration.[Partner] pa on pa.partner_id = ps.partner_id
                        inner join IA_Collaboration.CollaborationScope cs on cs.[scope_id] = ps.[scope_id]
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
                    }
                }
            }
        }
        private void handlingMOAData(MOABasicInfo basicInfo)
        {
            //handle date display
            basicInfo.moa_end_date_string = basicInfo.moa_end_date.ToString("dd'/'MM'/'yyyy");
            basicInfo.moa_start_date_string = basicInfo.moa_start_date.ToString("dd'/'MM'/'yyyy");
        }
        public void editMOABasicInfo(int moa_id, MOABasicInfo newBasicInfo)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    //update basicInfo
                    MOA moa = db.MOAs.Find(moa_id);
                    moa.moa_code = newBasicInfo.moa_code;
                    moa.moa_end_date = newBasicInfo.moa_end_date;
                    moa.moa_note = newBasicInfo.moa_note;
                    moa.evidence = newBasicInfo.evidence;
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
        public List<CollaborationScope> GetScopesExMOA(int moa_id, int mou_id)
        {
            try
            {
                string sql_scopeList = @"select tb3.* from(
					select * from(
					select distinct t2.scope_id from IA_Collaboration.MOUPartnerScope t1 left join
                    IA_Collaboration.PartnerScope t2 on 
                    t1.partner_scope_id = t2.partner_scope_id
                    where mou_id = @mou_id
					) tb1 where scope_id not in(
					select distinct scope_id from IA_Collaboration.MOAPartnerScope t1 left join
                    IA_Collaboration.PartnerScope t2 on 
                    t1.partner_scope_id = t2.partner_scope_id
					where moa_id = @moa_id)) tb2 inner join IA_Collaboration.CollaborationScope tb3
					on tb3.scope_id = tb2.scope_id";
                List<CollaborationScope> scopeList = db.Database.SqlQuery<CollaborationScope>(sql_scopeList,
                    new SqlParameter("mou_id", mou_id),
                    new SqlParameter("moa_id", moa_id)).ToList();
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
        public void addExtraMOA(ExMOAAdd input, int moa_id)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    DateTime sign_date = DateTime.ParseExact(input.ExMOABasicInfo.ex_moa_sign_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    DateTime end_date = DateTime.ParseExact(input.ExMOABasicInfo.ex_moa_end_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    //add MOABonus
                    MOABonu objMOABonusAdded = db.MOABonus.Add(new MOABonu
                    {
                        moa_bonus_code = input.ExMOABasicInfo.ex_moa_code,
                        moa_bonus_decision_date = sign_date,
                        moa_bonus_end_date = end_date,
                        moa_id = moa_id,
                        evidence = ""
                    });
                    db.SaveChanges();
                    //check PartnerScope and add MOAPartnerScope.
                    foreach (PartnerScopeInfoMOA psMOA in input.PartnerScopeInfoMOA.ToList())
                    {
                        foreach (int scopeItem in psMOA.scopes_id.ToList())
                        {
                            int partner_scope_id_item = 0;
                            PartnerScope psCheck = db.PartnerScopes.Where(x => x.partner_id == psMOA.partner_id && x.scope_id == scopeItem).First();
                            if (psCheck == null)
                            {
                                PartnerScope psAdded = db.PartnerScopes.Add(new PartnerScope
                                {
                                    partner_id = psMOA.partner_id,
                                    scope_id = scopeItem,
                                    reference_count = 1
                                });
                                partner_scope_id_item = psAdded.partner_scope_id;
                            }
                            else
                            {
                                partner_scope_id_item = psCheck.partner_scope_id;
                                psCheck.reference_count += 1;
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
        public void editExtraMOA(ExMOAAdd input, int moa__id)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    //edit MOABonus
                    //MOABonu mb = db.MOABonus.Find(moa_bonus_id);
                    //mb.moa_bonus_code = input.moa_bonus_code;
                    //mb.moa_bonus_decision_date = input.moa_bonus_decision_date;
                    //mb.moa_bonus_end_date = input.moa_bonus_end_date;
                    //mb.moa_id = input.moa_id;
                    //mb.evidence = input.evidence;
                    //db.Entry(mb).State = EntityState.Modified;

                    //finding old exScope of exMOA.
                    //List<MOAPartnerScope> exList = db.MOAPartnerScopes.Where(x => x.moa_bonus_id == moa_bonus_id).ToList();
                    //exList.Clear();
                    //db.Entry(exList).State = EntityState.Modified;

                    //add new record of MOAPartnerScope
                    //foreach (CustomPartner cp in input.ListPartnerExMOA.ToList())
                    //{
                    //    foreach (CustomScope cs in cp.ListScopeExMOU.ToList())
                    //    {
                    //        MOAPartnerScope m = new MOUPartnerScope();
                    //        m.mou_id = input.moa_id;
                    //        m.partner_id = cp.partner_id;
                    //        m.scope_id = cs.scope_id;
                    //        m.mou_bonus_id = moa_bonus_id;
                    //        db.MOUPartnerScopes.Add(m);
                    //    }
                    //}

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
        public string getNewExMOACode(int moa_id)
        {
            try
            {
                string sql_countInYear = @"select count(*) from IA_Collaboration.MOA where moa_code like @year";
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
                    newCode = DateTime.Now.Year + "/" + countInYear + "_BS/" + countInMOA;
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
        public ExMOAAdd getExtraMOADetail(int moa_id, int moa_bonus_id)
        {
            try
            {
                string sql_moaEx =
                    @"select t1.moa_bonus_code, t1.moa_bonus_decision_date,t1.moa_bonus_end_date,
                        t4.partner_name,t5.scope_abbreviation,t1.evidence,t1.moa_id,t1.moa_bonus_id,
                        t5.scope_id,t4.partner_id
                        from IA_Collaboration.MOABonus t1 left join 
                        IA_Collaboration.MOAPartnerScope t2 on 
                        t1.moa_bonus_id = t2.moa_bonus_id inner join 
                        IA_Collaboration.PartnerScope t3 on
                        t3.partner_scope_id = t2.partner_scope_id
                        inner join 
                        IA_Collaboration.Partner t4 on t4.partner_id = t3.partner_id
                        inner join IA_Collaboration.CollaborationScope t5 on t5.scope_id = t3.scope_id
                        where t1.moa_id = @moa_id and t1.moa_bonus_id = @moa_bonus_id order by partner_id";
                List<ExtraMOA> moaExList = db.Database.SqlQuery<ExtraMOA>(sql_moaEx
                    , new SqlParameter("moa_id", moa_id)
                    , new SqlParameter("moa_bonus_id", moa_bonus_id)).ToList();
                ExMOAAdd moaEx = handlingExMOADetailData(moaExList);
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
            return newObj;
        }
    }
}
