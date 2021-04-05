using ENTITIES;
using ENTITIES.CustomModels.InternationalCollaboration.Collaboration.MemorandumOfAgreement.MOA;
using ENTITIES.CustomModels.InternationalCollaboration.Collaboration.MemorandumOfAgreement.MOAPartner;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BLL.InternationalCollaboration.Collaboration.MemorandumOfAgreement.MOARepo;

namespace BLL.InternationalCollaboration.Collaboration.MemorandumOfAgreement
{
    public class PartnerMOARepo
    {
        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        public List<ListMOAPartner> listAllMOAPartner(string partner_name, string nation, string specialization, int moa_id)
        {
            try
            {
                string sql_moaPartnerList =
                    @"select t2.moa_partner_id,t1.moa_id,t4.partner_id,t4.partner_name,
                        t5.country_name,t7.specialization_name,t4.website,t3.contact_point_name,
                        t3.contact_point_phone, t3.contact_point_email, t2.moa_start_date,
                        t9.scope_abbreviation,t1.moa_code
                        from IA_Collaboration.MOA t1 
                        left join IA_Collaboration.MOAPartner t2 on
                        t2.moa_id = t1.moa_id
                        left join IA_Collaboration.MOUPartner t3 on
                        t3.mou_id = t1.mou_id and t2.partner_id = t3.partner_id
                        left join IA_Collaboration.[Partner] t4 on
                        t4.partner_id = t2.partner_id
                        left join General.Country t5 on
                        t5.country_id = t4.country_id 
                        left join IA_Collaboration.MOUPartnerSpecialization t6 on
                        t6.mou_partner_id = t3.mou_partner_id
                        left join General.Specialization t7 on 
                        t7.specialization_id = t6.specialization_id
                        left join (
                        select partner_id,scope_id from IA_Collaboration.MOAPartnerScope a
                        inner join IA_Collaboration.PartnerScope b
                        on a.partner_scope_id = b.partner_scope_id
                        where moa_bonus_id is null and moa_id = @moa_id
                        ) t8 on t8.partner_id = t2.partner_id
                        left join IA_Collaboration.CollaborationScope t9
                        on t9.scope_id = t8.scope_id
                        where t1.moa_id = @moa_id
                        and t4.partner_name like @partner_name
                        and t5.country_name like @nation
                        and t7.specialization_name like @specialization
                        order by t2.moa_partner_id";
                List<ListMOAPartner> moaList = db.Database.SqlQuery<ListMOAPartner>(sql_moaPartnerList,
                    new SqlParameter("moa_id", moa_id),
                    new SqlParameter("partner_name", '%' + partner_name + '%'),
                    new SqlParameter("nation", '%' + nation + '%'),
                    new SqlParameter("specialization", '%' + specialization + '%')).ToList();
                handlingPartnerListData(moaList);
                return moaList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CollaborationScope> getMOAScope(int moa_id, int partner_id, int mou_id)
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
					where moa_id = @moa_id and moa_bonus_id is not null and partner_id = @partner_id)) tb2 inner join IA_Collaboration.CollaborationScope tb3
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
        public void handlingPartnerListData(List<ListMOAPartner> moaList)
        {
            //spe_name
            //sco_abbre
            ListMOAPartner previousItem = null;
            foreach (ListMOAPartner item in moaList.ToList())
            {
                if (previousItem == null) //first record
                {
                    previousItem = item;
                    previousItem.moa_start_date_string = item.moa_start_date.ToString("dd'/'MM'/'yyyy");
                }
                else
                {
                    if (item.moa_partner_id.Equals(previousItem.moa_partner_id))
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
                        moaList.Remove(item);
                    }
                    else
                    {
                        previousItem = item;
                        previousItem.moa_start_date_string = item.moa_start_date.ToString("dd'/'MM'/'yyyy");
                    }
                }
            }
            return;
        }
        public void addMOAPartner(MOAPartnerInfo input, int moa_id)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    List<PartnerScope> totalRelatedPS = new List<PartnerScope>();
                    DateTime moa_start_date = DateTime.ParseExact(input.sign_date_moa_add, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    //get partner_id
                    //int partner_id = db.Partners.Where(x => x.partner_name.Equals(input.partnername_add)).First().partner_id;
                    //add MOAPartner
                    db.MOAPartners.Add(new MOAPartner
                    {
                        moa_id = moa_id,
                        moa_start_date = moa_start_date,
                        partner_id = input.partner_id
                    });
                    db.SaveChanges();
                    foreach (int itemScope in input.coop_scope_add.ToList())
                    {
                        PartnerScope psCheck = db.PartnerScopes.Where(x => x.partner_id == input.partner_id && x.scope_id == itemScope).FirstOrDefault();
                        if (psCheck != null)
                        {
                            //psCheck.reference_count += 1;
                            db.MOAPartnerScopes.Add(new MOAPartnerScope
                            {
                                partner_scope_id = psCheck.partner_scope_id,
                                moa_id = moa_id
                            });
                            totalRelatedPS.Add(psCheck);
                        }
                        //else
                        //{
                        //    PartnerScope psAdded = db.PartnerScopes.Add(new PartnerScope
                        //    {
                        //        partner_id = input.partner_id,
                        //        scope_id = itemScope,
                        //        reference_count = 1
                        //    });
                        //    db.SaveChanges();
                        //    db.MOAPartnerScopes.Add(new MOAPartnerScope
                        //    {
                        //        partner_scope_id = psAdded.partner_scope_id,
                        //        moa_id = moa_id
                        //    });
                        //}
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
        public void editMOAPartner(MOAPartnerEdited input, int moa_id)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    List<PartnerScope> totalRelatedPS = new List<PartnerScope>();
                    DateTime sign_date = DateTime.ParseExact(input.sign_date_string, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    //edit MOAPartner
                    MOAPartner mp = db.MOAPartners.Where(x => x.moa_partner_id == input.moa_partner_id).First();
                    mp.moa_start_date = sign_date;
                    mp.partner_id = input.partner_id;
                    db.Entry(mp).State = EntityState.Modified;
                    db.SaveChanges();

                    //get old records of PartnerScope in MOA of each partner.
                    string sql_old_partnerScope = @"select t1.partner_scope_id,partner_id,scope_id,reference_count  from IA_Collaboration.PartnerScope t1 inner join 
                        IA_Collaboration.MOAPartnerScope t2 on
                        t1.partner_scope_id = t2.partner_scope_id
                        where moa_id = @moa_id and partner_id = @partner_id";
                    List<PartnerScope> listOldPS = db.Database.SqlQuery<PartnerScope>(sql_old_partnerScope,
                        new SqlParameter("moa_id", moa_id),
                        new SqlParameter("partner_id", input.partner_id)).ToList();
                    totalRelatedPS.AddRange(listOldPS);
                    //reset old records of scopes of partner.
                    foreach (PartnerScope token in listOldPS.ToList())
                    {
                        //delete record of MOUPartnerScope.
                        db.MOAPartnerScopes.Remove(db.MOAPartnerScopes.Where(x => x.partner_scope_id == token.partner_scope_id && x.moa_id == moa_id && x.moa_bonus_id == null).First());

                        //PartnerScope objPS = db.PartnerScopes.Where(x => x.partner_scope_id == token.partner_scope_id).First();
                        //decrese refer_count in PartnerScope
                        //objPS.reference_count -= 1;
                        //db.Entry(objPS).State = EntityState.Modified;
                    }
                    db.SaveChanges();

                    //add new records of scopes of partner.
                    foreach (int tokenScope in input.scopes.ToList())
                    {
                        PartnerScope objPS = db.PartnerScopes.Where(x => x.partner_id == input.partner_id && x.scope_id == tokenScope).FirstOrDefault();
                        int partner_scope_id = 0;
                        if (objPS == null) //add new to PartnerScope
                        {
                            //db.PartnerScopes.Add(new PartnerScope
                            //{
                            //    partner_id = input.partner_id,
                            //    scope_id = tokenScope,
                            //    reference_count = 1
                            //});
                            ////checkpoint 3
                            //db.SaveChanges();
                            //PartnerScope newObjPS = db.PartnerScopes.Where(x => x.partner_id == input.partner_id && x.scope_id == tokenScope).FirstOrDefault();
                            //partner_scope_id = newObjPS.partner_scope_id;
                        }
                        else
                        {
                            //objPS.reference_count += 1;
                            //db.Entry(objPS).State = EntityState.Modified;
                            partner_scope_id = objPS.partner_scope_id;
                            totalRelatedPS.Add(objPS);
                        }
                        db.SaveChanges();
                        db.MOAPartnerScopes.Add(new MOAPartnerScope
                        {
                            partner_scope_id = partner_scope_id,
                            moa_id = moa_id
                        });
                        db.SaveChanges();
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
        public void deleteMOAPartner(int moa_id, int moa_partner_id)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    List<PartnerScope> totalRelatedPS = new List<PartnerScope>();
                    //get partner of moupartner deleted.
                    int partner_id_item = db.MOAPartners.Find(moa_partner_id).partner_id;

                    //get all scopes of partner in MOU.
                    string sql_old_partnerScope = @"select t1.partner_scope_id,partner_id,scope_id,reference_count from IA_Collaboration.PartnerScope t1 inner join 
                        IA_Collaboration.MOAPartnerScope t2 on
                        t1.partner_scope_id = t2.partner_scope_id
                        where moa_id = @moa_id and partner_id = @partner_id";
                    List<PartnerScope> listPS = db.Database.SqlQuery<PartnerScope>(sql_old_partnerScope,
                        new SqlParameter("moa_id", moa_id),
                        new SqlParameter("partner_id", partner_id_item)).ToList();
                    totalRelatedPS.AddRange(listPS);
                    foreach (PartnerScope partnerScope in listPS.ToList())
                    {
                        PartnerScope ps = db.PartnerScopes.Find(partnerScope.partner_scope_id);
                        //ps.reference_count -= 1;
                        //db.Entry(ps).State = EntityState.Modified;
                        db.MOAPartnerScopes.Remove(db.MOAPartnerScopes.Where(x => x.partner_scope_id == partnerScope.partner_scope_id && x.moa_id == moa_id).First());
                    }
                    //checkpoint 1
                    db.SaveChanges();

                    //checkpoint 2
                    db.MOAPartners.Remove(db.MOAPartners.Find(moa_partner_id));
                    db.SaveChanges();
                    transaction.Commit();

                    //change status corressponding MOU/MOA
                    using (DbContextTransaction dbContext = db.Database.BeginTransaction())
                    {
                        try
                        {
                            List<int> listObjPS = totalRelatedPS.Select(x => x.partner_scope_id).Distinct().ToList();
                            new AutoActiveInactive().changeStatusMOUMOA(listObjPS, db);
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
        public List<PartnerHistory> listMOAPartnerHistory(int moa_partner_id)
        {
            string sql_history =
                    @"WITH b AS (
                SELECT moa.moa_id, moa.moa_code, moap.moa_start_date, moa.moa_end_date, moap.moa_partner_id, ps.partner_id, ps.scope_id, ps.partner_scope_id
                FROM IA_Collaboration.MOA moa
                JOIN IA_Collaboration.MOAPartner moap on moap.moa_id = moa.moa_id
                JOIN IA_Collaboration.MOAPartnerScope moaps on moaps.moa_id = moa.moa_id
                JOIN IA_Collaboration.PartnerScope ps on ps.partner_scope_id = moaps.partner_scope_id
                where moap.moa_partner_id = @moa_partner_id)
                SELECT N'Tổ chức hoạt động học thuật' 'content', ac.full_name, ap.add_time
                FROM SMIA_AcademicActivity.AcademicActivity aa 
                INNER JOIN SMIA_AcademicActivity.ActivityPartner ap ON aa.activity_id = ap.activity_id 
                INNER JOIN b ON b.partner_scope_id = ap.partner_scope_id
                INNER JOIN General.Account ac on ac.account_id = ap.account_id
                and b.moa_start_date <= aa.activity_date_start and aa.activity_date_end <= b.moa_end_date
                UNION ALL
                SELECT N'Hợp tác học thuật' 'content', acc.full_name, csh.change_time 'add_time'
                FROM IA_AcademicCollaboration.AcademicCollaboration ac 
                INNER JOIN b ON ac.partner_scope_id = b.partner_scope_id
                INNER JOIN
                (select csh1.collab_id, csh1.change_time, csh2.account_id
                from
	                (select collab_id, MAX(change_date) 'change_time'
	                from IA_AcademicCollaboration.CollaborationStatusHistory
	                group by collab_id) as csh1
	                INNER JOIN
	                (select *
	                from IA_AcademicCollaboration.CollaborationStatusHistory) as csh2 
	                on csh1.collab_id = csh2.collab_id and csh1.change_time = csh2.change_date) as csh on csh.collab_id = ac.collab_id
                INNER JOIN General.Account acc on acc.account_id = csh.account_id
                and b.moa_start_date <= ac.plan_study_start_date and ac.plan_study_end_date <= b.moa_end_date
                UNION ALL
                SELECT DISTINCT N'Ký kết biên bản thỏa thuận' 'content', acc.full_name, moa.add_time
                FROM b INNER JOIN IA_Collaboration.MOAPartner mp ON b.partner_id = mp.partner_id and mp.moa_partner_id = b.moa_partner_id
                INNER JOIN IA_Collaboration.MOA moa ON moa.moa_id = mp.moa_id
                LEFT JOIN General.Account acc on acc.account_id = moa.account_id
                UNION ALL
                SELECT DISTINCT N'Ký kết biên bản thỏa thuận bổ sung' 'content', acc.full_name, mb.add_time
                FROM b INNER JOIN IA_Collaboration.MOAPartner mp ON b.partner_id = mp.partner_id and mp.moa_partner_id = b.moa_partner_id
                INNER JOIN IA_Collaboration.MOABonus mb ON mp.moa_id = mb.moa_id 
                LEFT JOIN IA_Collaboration.MOAPartnerScope mps ON mps.moa_bonus_id = mb.moa_bonus_id
                LEFT JOIN IA_Collaboration.MOA moa on moa.moa_id = mb.moa_id
                LEFT JOIN General.Account acc on acc.account_id = mb.account_id
                WHERE (mb.moa_bonus_end_date IS NOT NULL) OR (mps.partner_scope_id IS NOT NULL)
                ORDER BY add_time ASC";
            List<PartnerHistory> historyList = db.Database.SqlQuery<PartnerHistory>(sql_history,
                new SqlParameter("moa_partner_id", moa_partner_id)).ToList();
            return historyList;
        }

        public List<CollaborationScope> getPartnerMOAScope(int moa_id, int mou_id)
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
        public List<ENTITIES.Partner> getPartnerMOA(int mou_id)
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
        public CustomPartnerMOA getPartnerDetail(int moa_partner_id)
        {
            try
            {
                string sql_partnerList_1 = @"select t3.partner_id,t5.country_name, t3.website, t3.address, 
                        t4.contact_point_name, t4.contact_point_email, t4.contact_point_phone,
                        t7.specialization_name, t3.partner_name
                        from IA_Collaboration.MOAPartner t1
                        inner join IA_Collaboration.MOA t2 on t1.moa_id = t2.moa_id
                        inner join IA_Collaboration.Partner t3 on t3.partner_id = t1.partner_id
                        inner join IA_Collaboration.MOUPartner t4 
                        on t4.mou_id = t2.mou_id and t4.partner_id = t1.partner_id
                        inner join General.Country t5 on t5.country_id = t3.country_id
                        inner join IA_Collaboration.MOUPartnerSpecialization t6 on
                        t6.mou_partner_id = t4.mou_partner_id
                        inner join General.Specialization t7 on t7.specialization_id = t6.specialization_id
                        where t1.moa_partner_id = @moa_partner_id";
                string sql_partnerList_2 = @"select scope_id,moa_start_date from IA_Collaboration.PartnerScope a inner join
                        IA_Collaboration.MOAPartnerScope b on
                        a.partner_scope_id = b.partner_scope_id inner join 
                        IA_Collaboration.MOAPartner c 
                        on c.moa_id = b.moa_id and c.partner_id = a.partner_id
                        where c.moa_partner_id = @moa_partner_id and b.moa_bonus_id is null";
                List<CustomPartnerMOA> obj1 = db.Database.SqlQuery<CustomPartnerMOA>(sql_partnerList_1,
                    new SqlParameter("moa_partner_id", moa_partner_id)).ToList();
                List<ScopeAndStartDate> obj2 = db.Database.SqlQuery<ScopeAndStartDate>(sql_partnerList_2,
                    new SqlParameter("moa_partner_id", moa_partner_id)).ToList();

                CustomPartnerMOA MOAPartnerDetail = handlingMOAPartnerDetailData(obj1);
                MOAPartnerDetail.scopes = new List<int>();
                foreach (ScopeAndStartDate item in obj2.ToList())
                {
                    MOAPartnerDetail.scopes.Add(item.scope_id);
                }
                MOAPartnerDetail.moa_start_date_string = obj2[0].moa_start_date.ToString("dd'/'MM'/'yyyy");
                return MOAPartnerDetail;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public CustomPartnerMOA handlingMOAPartnerDetailData(List<CustomPartnerMOA> list)
        {
            //spe_name
            //sco_abbre
            CustomPartnerMOA previousItem = null;
            foreach (CustomPartnerMOA item in list.ToList())
            {
                if (previousItem == null) //first record
                {
                    previousItem = item;
                    previousItem.moa_start_date_string = item.moa_start_date.ToString("dd'/'MM'/'yyyy");
                }
                else
                {
                    if (item.partner_id.Equals(previousItem.partner_id))
                    {
                        if (!previousItem.specialization_name.Contains(item.specialization_name))
                        {
                            previousItem.specialization_name = previousItem.specialization_name + "," + item.specialization_name;
                        }
                        //then remove current object
                        list.Remove(item);
                    }
                    else
                    {
                        previousItem = item;
                        previousItem.moa_start_date_string = item.moa_start_date.ToString("dd'/'MM'/'yyyy");
                    }
                }
            }
            return list[0];
        }
        public bool MOAPartnerDateIsInvalid(int mou_id, int partner_id, string start_date_string)
        {
            try
            {
                DateTime start_date = DateTime.ParseExact(start_date_string, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                MOUPartner obj = db.MOUPartners.Where(x => x.mou_id == mou_id && x.partner_id == partner_id && x.mou_start_date < start_date).FirstOrDefault();
                return obj is null ? true : false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool IsMOABonusExisted(int moa_partner_id)
        {
            try
            {
                string sql_check = @"select t1.* from IA_Collaboration.MOABonus t1
                    inner join IA_Collaboration.MOAPartner t2
                    on t2.moa_id = t1.moa_id
                    inner join IA_Collaboration.MOAPartnerScope t3
                    on t3.moa_id = t1.moa_id and t3.moa_bonus_id = t1.moa_bonus_id
                    inner join IA_Collaboration.PartnerScope t4
                    on t4.partner_scope_id = t3.partner_scope_id and t4.partner_id = t2.partner_id
                    where moa_partner_id = @moa_partner_id";
                List<MOABonu> exMOAList = db.Database.SqlQuery<MOABonu>(sql_check,
                    new SqlParameter("moa_partner_id", moa_partner_id)).ToList();
                return exMOAList.Count == 0 ? false : true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string CheckPartnerExistedInMOA(int moa_id, int partner_id)
        {
            try
            {
                string sql = @"select partner_name from IA_Collaboration.MOAPartner t1 left join
                    IA_Collaboration.Partner t2 on
                    t1.partner_id = t2.partner_id
                    where t1.moa_id = @moa_id and t2.partner_id = @partner_id";
                string result = db.Database.SqlQuery<string>(sql,
                    new SqlParameter("partner_id", partner_id),
                    new SqlParameter("moa_id", moa_id)).FirstOrDefault();
                return result is null ? "" : result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string CheckPartnerExistedInEditMOA(int moa_partner_id, string partner_name)
        {
            try
            {
                string sql = @"select partner_name from IA_Collaboration.MOAPartner t1 left join
                    IA_Collaboration.Partner t2 on
                    t1.partner_id = t2.partner_id
                    where t1.moa_partner_id != @moa_partner_id and t2.partner_name like @partner_name";
                string result = db.Database.SqlQuery<string>(sql,
                    new SqlParameter("partner_name", '%' + partner_name + '%'),
                    new SqlParameter("moa_partner_id", moa_partner_id)).FirstOrDefault();
                return result is null ? "" : result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
