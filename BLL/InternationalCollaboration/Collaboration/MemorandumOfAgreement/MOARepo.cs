using BLL.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding;
using ENTITIES;
using ENTITIES.CustomModels.InternationalCollaboration.Collaboration.MemorandumOfAgreement.MOA;
using ENTITIES.CustomModels.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding.MOU;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

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
                    @"select t1.moa_id,t1.moa_code,t3.partner_name,t8.link as evidence,t2.moa_start_date,
                        t1.moa_end_date,t5.office_name,t6.scope_abbreviation,t7.mou_status_id,t2.moa_partner_id
                        from IA_Collaboration.MOA t1
                        left join IA_Collaboration.MOAPartner t2 
                        on t2.moa_id = t1.moa_id 
                        left join IA_Collaboration.Partner t3
                        on t3.partner_id = t2.partner_id
                        left join IA_Collaboration.MOU t4 
                        on t4.mou_id = t1.mou_id 
                        left join General.Office t5 
                        on t5.office_id = t4.office_id
                        left join 
						(select moa_id,partner_id,b.scope_id,scope_abbreviation
						from IA_Collaboration.MOAPartnerScope a
						inner join IA_Collaboration.PartnerScope b on
						a.partner_scope_id = b.partner_scope_id
						inner join IA_Collaboration.CollaborationScope c on
						c.scope_id = b.scope_id) t6 on 
						t6.moa_id = t1.moa_id and t6.partner_id = t2.partner_id
                        left join (
                        select a.moa_id,a.mou_status_id from IA_Collaboration.MOAStatusHistory a
                        inner join 
                        (select max(datetime) as max_date,moa_id from IA_Collaboration.MOAStatusHistory
                        group by moa_id) b on
                        a.datetime = b.max_date and a.moa_id = b.moa_id) t7
                        on t7.moa_id = t1.moa_id
						left join General.[File] t8 on
						t8.file_id = t1.evidence
                        where t1.mou_id = @mou_id
                        and t3.partner_name like @partner_name
                        and t1.moa_code like @moa_code
                        and t1.is_deleted = 0";
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
                        previousItem.moa_start_date_string = item.moa_start_date.ToString("dd'/'MM'/'yyyy");
                        previousItem.moa_end_date_string = item.moa_end_date.ToString("dd'/'MM'/'yyyy");
                    }
                }
            }
            return;
        }
        public CustomPartnerMOA CheckPartner(int mou_id, int partner_id)
        {
            try
            {
                string sql = @"select t3.country_name,t2.website,t2.address,
                    t1.contact_point_name,t1.contact_point_email,t1.contact_point_phone,
                    t5.specialization_name,t2.partner_name
                    from IA_Collaboration.MOUPartner t1
                    left join IA_Collaboration.Partner t2 on
                    t1.partner_id = t2.partner_id
                    left join General.Country t3 on 
                    t3.country_id = t2.country_id
                    left join IA_Collaboration.MOUPartnerSpecialization t4 on
                    t4.mou_partner_id = t1.mou_partner_id
                    left join General.Specialization t5 on
                    t5.specialization_id = t4.specialization_id
					where t1.partner_id = @partner_id and t1.mou_id = @mou_id";
                List<CustomPartnerMOA> pList = db.Database.SqlQuery<CustomPartnerMOA>(sql,
                    new SqlParameter("mou_id", mou_id),
                    new SqlParameter("partner_id", partner_id)).ToList();
                handlingMOAPartnerData(pList);
                return pList[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void handlingMOAPartnerData(List<CustomPartnerMOA> pList)
        {
            CustomPartnerMOA previousItem = null;
            foreach (CustomPartnerMOA item in pList.ToList())
            {
                if (previousItem == null) //first record
                {
                    previousItem = item;
                    previousItem.specializations = item.specialization_name;
                }
                else
                {
                    if (item.partner_name.Equals(previousItem.partner_name))
                    {
                        previousItem.specializations += ',' + item.specialization_name;
                        //then remove current object
                        pList.Remove(item);
                    }
                    else
                    {
                        previousItem = item;
                    }
                }
            }
            return;
        }
        public void addMOA(MOAAdd input, int mou_id, BLL.Authen.LoginRepo.User user, HttpPostedFileBase evidence)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    //add MOA
                    //add MOAPartner => 
                    //add MOAPartnerScope
                    //add MOAStatusHistory

                    //File handling.
                    Google.Apis.Drive.v3.Data.File f = new Google.Apis.Drive.v3.Data.File();
                    if (evidence != null)
                    {
                        f = new MOURepo().uploadEvidenceFile(evidence, db.MOUs.Find(mou_id).mou_code, 3, false);
                    }
                    File evidence_file = new MOURepo().saveFile(f, evidence);
                    int? evidence_value;
                    if (evidence_file.file_id == 0)
                    {
                        evidence_value = null;
                    }
                    else
                    {
                        evidence_value = evidence_file.file_id;
                    }

                    List<PartnerScope> totalRelatedPS = new List<PartnerScope>();
                    DateTime moa_end_date = DateTime.ParseExact(input.MOABasicInfo.moa_end_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    MOA m = db.MOAs.Add(new MOA
                    {
                        moa_code = input.MOABasicInfo.moa_code,
                        moa_end_date = moa_end_date,
                        moa_note = input.MOABasicInfo.moa_note,
                        mou_id = mou_id,
                        account_id = user is null ? 1 : user.account.account_id,
                        add_time = DateTime.Now,
                        is_deleted = false,
                        evidence = evidence_value
                    });
                    //checkpoint 1
                    db.SaveChanges();

                    foreach (MOAPartnerInfo item in input.MOAPartnerInfo.ToList())
                    {
                        Partner p = db.Partners.Where(x => x.partner_name == item.partnername_add).First();
                        db.MOAPartners.Add(new MOAPartner
                        {
                            moa_id = m.moa_id,
                            moa_start_date = DateTime.ParseExact(item.sign_date_moa_add, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            partner_id = p.partner_id
                        });
                        db.SaveChanges();
                        foreach (int scopeItem in item.coop_scope_add.ToList())
                        {
                            PartnerScope ps = db.PartnerScopes.Where(x => x.partner_id == p.partner_id && x.scope_id == scopeItem).First();
                            //ps.reference_count += 1;
                            //db.Entry(ps).State = EntityState.Modified;
                            totalRelatedPS.Add(ps);
                            db.MOAPartnerScopes.Add(new MOAPartnerScope
                            {
                                partner_scope_id = ps.partner_scope_id,
                                moa_id = m.moa_id
                            });
                            db.SaveChanges();
                        }
                        //checkpoint 2
                        db.SaveChanges();
                    }
                    db.MOAStatusHistories.Add(new MOAStatusHistory
                    {
                        datetime = DateTime.Now,
                        reason = input.MOABasicInfo.reason,
                        moa_id = m.moa_id,
                        mou_status_id = input.MOABasicInfo.mou_status_id
                    });
                    //checkpoint 3
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
        public void deleteMOA(int moa_id)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    //delete partner_scope_id 
                    //delete from ExMOA => MOA
                    //string sql_moa = @"select t2.* from IA_Collaboration.MOAPartnerScope t1
                    //    inner join IA_Collaboration.PartnerScope t2 on 
                    //    t1.partner_scope_id = t2.partner_scope_id
                    //    where t1.moa_id = @moa_id";
                    //string sql_ex_moa = @"select t3.* from IA_Collaboration.MOABonus t1
                    //    inner join IA_Collaboration.MOAPartnerScope t2
                    //    on t2.moa_bonus_id = t1.moa_bonus_id
                    //    inner join IA_Collaboration.PartnerScope t3
                    //    on t3.partner_scope_id = t2.partner_scope_id
                    //    where t1.moa_id = @moa_id";
                    //List<PartnerScope> ex_moa_list = db.Database.SqlQuery<PartnerScope>(sql_ex_moa,
                    //    new SqlParameter("moa_id", moa_id)).ToList();
                    //List<PartnerScope> moa_list = db.Database.SqlQuery<PartnerScope>(sql_moa,
                    //    new SqlParameter("moa_id", moa_id)).ToList();

                    //if (ex_moa_list != null)
                    //{
                    //    foreach (PartnerScope item in ex_moa_list)
                    //    {
                    //        db.PartnerScopes.Find(item.partner_scope_id).reference_count -= 1;
                    //    }
                    //}
                    //if (moa_list != null)
                    //{
                    //    foreach (PartnerScope item in moa_list)
                    //    {
                    //        db.PartnerScopes.Find(item.partner_scope_id).reference_count -= 1;
                    //    }
                    //}
                    //db.SaveChanges();

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
        public string getSuggestedMOACode(int mou_id)
        {
            try
            {

                bool is_duplicated = false;
                string new_code = "";

                string old_mou_code = db.MOUs.Find(mou_id).mou_code;
                int count_moa = db.MOAs.Where(x => x.mou_id == mou_id).Count();

                //fix duplicate mou_code:
                do
                {
                    count_moa++;
                    new_code = old_mou_code + "_MOA/" + count_moa;
                    is_duplicated = db.MOAs.Where(x => x.moa_code.Equals(new_code)).FirstOrDefault() is null ? false : true;
                } while (is_duplicated);
                return new_code;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CustomScopesMOA> getMOAScope(int mou_id, int partner_id)
        {
            try
            {
                string sql_scopeList = @"select distinct t3.* from IA_Collaboration.MOUPartnerScope t1
                    inner join IA_Collaboration.PartnerScope t2 on
                    t1.partner_scope_id = t2.partner_scope_id
                    inner join IA_Collaboration.CollaborationScope t3 on
                    t3.scope_id = t2.scope_id
                    inner join IA_Collaboration.Partner t4 on
                    t4.partner_id = t2.partner_id
                    where t1.mou_id = @mou_id and t4.partner_id = @partner_id
                    order by t3.scope_id";
                List<CustomScopesMOA> scopeList = db.Database.SqlQuery<CustomScopesMOA>(sql_scopeList
                    , new SqlParameter("mou_id", mou_id)
                    , new SqlParameter("partner_id", partner_id)
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
        public bool getMOACodeCheck(string moa_code)
        {
            try
            {
                MOA obj = db.MOAs.Where(x => x.moa_code == moa_code && !x.is_deleted).FirstOrDefault();
                return obj == null ? false : true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool checkDuplicatePartnersMOA(List<MOAPartnerInfo> PartnerInfo, int mou_id)
        {
            string partner_id_para = "";
            foreach (MOAPartnerInfo item in PartnerInfo)
            {
                partner_id_para += (item.partner_id + ",");
            }
            partner_id_para = partner_id_para.Remove(partner_id_para.Length - 1);
            string query = @"select count(*) as num_check ,t1.moa_id,t1.mou_id from IA_Collaboration.MOA t1 inner join 
                IA_Collaboration.MOAPartner t2 on
                t1.moa_id = t2.moa_id
                where t2.partner_id in (" + partner_id_para + @") and t1.is_deleted = 0 and t1.mou_id = @mou_id
                group by t1.moa_id,t1.mou_id
                having count(*) = @partner_count
                order by moa_id";
            List<DuplicatePartnersMOA> obj = db.Database.SqlQuery<DuplicatePartnersMOA>(query,
                    new SqlParameter("partner_count", PartnerInfo.Count),
                    new SqlParameter("mou_id", mou_id)).ToList();
            return obj.Count() > 0 ? true : false;
        }
    }
}
