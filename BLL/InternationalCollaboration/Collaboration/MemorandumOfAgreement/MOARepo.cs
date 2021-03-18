using ENTITIES;
using ENTITIES.CustomModels.InternationalCollaboration.Collaboration.MemorandumOfAgreement.MOA;
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
    public class MOARepo
    {
        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        public List<ListMOA> listAllMOA(string partner_name, string moa_code, string mou_id)
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
        public CustomPartnerMOA CheckPartner(int mou_id, string partner_name)
        {
            try
            {
                string sql = @"select t4.country_name,t3.website,t3.address,
                    t2.contact_point_name,t2.contact_point_email,t2.contact_point_phone,
                    t6.specialization_name,t2.partner_name
                    from IA_Collaboration.MOA t1 inner join
                    IA_Collaboration.MOUPartner t2 on 
                    t1.mou_id = t2.mou_id
                    inner join IA_Collaboration.Partner t3 on
                    t3.partner_id = t2.partner_id
                    inner join General.Country t4 on 
                    t4.country_id = t3.country_id
                    inner join IA_Collaboration.MOUPartnerSpecialization t5 on 
                    t5.mou_partner_id = t2.mou_partner_id
                    inner join General.Specialization t6 on
                    t6.specialization_id = t5.specialization_id
                    where t2.mou_id = @mou_id and t3.partner_name like @partner_name";
                CustomPartnerMOA p = db.Database.SqlQuery<CustomPartnerMOA>(sql,
                    new SqlParameter("mou_id", mou_id),
                    new SqlParameter("partner_name", partner_name)).FirstOrDefault();
                return p;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void addMOA(MOAAdd input, int mou_id)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    //add MOA
                    //add MOAPartner => 
                    //update PartnerScope =>
                    //add MOAPartnerScope
                    //add MOAStatusHistory
                    DateTime moa_end_date = DateTime.ParseExact(input.MOABasicInfo.moa_end_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    MOA m = db.MOAs.Add(new MOA
                    {
                        moa_code = input.MOABasicInfo.moa_code,
                        moa_end_date = moa_end_date,
                        moa_note = input.MOABasicInfo.moa_note,
                        mou_id = mou_id,
                        account_id = 1,
                        add_time = DateTime.Now,
                        is_deleted = false
                    });
                    //checkpoint 1
                    db.SaveChanges();

                    foreach (MOAPartnerInfo item in input.MOAPartnerInfo.ToList())
                    {
                        db.MOAPartners.Add(new MOAPartner
                        {
                            moa_id = m.moa_id,
                            moa_start_date = DateTime.ParseExact(item.sign_date_moa_add, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            partner_id = item.partner_id
                        });
                        foreach (int scopeItem in item.coop_scope_add.ToList())
                        {
                            PartnerScope ps = db.PartnerScopes.Where(x => x.partner_id == item.partner_id && x.scope_id == scopeItem).First();
                            ps.reference_count -= 1;

                            db.Entry(ps).State = EntityState.Modified;
                            db.MOAPartnerScopes.Add(new MOAPartnerScope
                            {
                                partner_scope_id = ps.partner_scope_id,
                                moa_id = m.moa_id
                            });
                        }
                        //checkpoint 2
                        db.SaveChanges();
                    }
                    db.MOAStatusHistories.Add(new MOAStatusHistory
                    {
                        datetime = DateTime.Now,
                        reason = input.MOABasicInfo.reason,
                        moa_id = m.moa_id,
                        mou_status_id = 2
                    });
                    //checkpoint 3
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
        public List<CustomScopesMOA> getMOAScope(int mou_id, string partner_name)
        {
            try
            {
                string sql_scopeList = @"select distinct t3.scope_id,t3.scope_abbreviation from IA_Collaboration.MOUPartnerScope t1
                    inner join IA_Collaboration.PartnerScope t2 on
                    t1.partner_scope_id = t2.partner_scope_id
                    inner join IA_Collaboration.CollaborationScope t3 on
                    t3.scope_id = t2.scope_id
                    inner join IA_Collaboration.Partner t4 on
                    t4.partner_id = t2.partner_id
                    where t1.mou_id = @mou_id and t4.partner_name like @partner_name
                    order by t3.scope_id";
                List<CustomScopesMOA> scopeList = db.Database.SqlQuery<CustomScopesMOA>(sql_scopeList
                    , new SqlParameter("mou_id", mou_id)
                    , new SqlParameter("partner_name", '%' + partner_name + '%')
                    ).ToList();
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
                    new SqlParameter("mou_id", mou_id)).ToList();
                return partnerList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
