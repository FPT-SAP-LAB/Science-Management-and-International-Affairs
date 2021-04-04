using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding.MOU;
using ENTITIES.CustomModels.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding.MOUPartner;
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
    public class PartnerMOURepo
    {
        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        public List<ListMOUPartner> listAllMOUPartner(string partner_name, string nation_name, string specialization_name, int mou_id)
        {
            try
            {
                string sql_mouPartnerList =
                    @"select t2.mou_partner_id, t1.mou_code, t3.partner_name, t6.specialization_name,
                        t3.website, t2.contact_point_name, t2.contact_point_phone, t2.contact_point_email,
                        t2.mou_start_date, t8.scope_abbreviation,t1.mou_id,t4.country_name
                        from IA_Collaboration.MOU t1 left join 
                        IA_Collaboration.MOUPartner t2 
                        on t1.mou_id = t2.mou_id
                        left join IA_Collaboration.Partner t3
                        on t3.partner_id = t2.partner_id
                        left join General.Country t4 
                        on t4.country_id = t3.country_id
                        left join IA_Collaboration.MOUPartnerSpecialization t5
                        on t5.mou_partner_id = t2.mou_partner_id
                        left join General.Specialization t6 
                        on t6.specialization_id = t5.specialization_id
                        left join (
                        select partner_id,scope_id from IA_Collaboration.MOUPartnerScope a
                        inner join IA_Collaboration.PartnerScope b
                        on a.partner_scope_id = b.partner_scope_id
                        where mou_bonus_id is null and mou_id = @mou_id) t7 
                        on t7.partner_id = t3.partner_id
                        left join IA_Collaboration.CollaborationScope t8
                        on t8.scope_id = t7.scope_id
                        where t1.mou_id = @mou_id
                        and t4.country_name like @nation_name
                        and t3.partner_name like @partner_name
                        and t6.specialization_name like @specialization_name
                        order by t2.mou_partner_id";
                List<ListMOUPartner> mouList = db.Database.SqlQuery<ListMOUPartner>(sql_mouPartnerList,
                    new SqlParameter("mou_id", mou_id),
                    new SqlParameter("nation_name", '%' + nation_name + '%'),
                    new SqlParameter("partner_name", '%' + partner_name + '%'),
                    new SqlParameter("specialization_name", '%' + specialization_name + '%')).ToList();
                handlingPartnerListData(mouList);
                return mouList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void handlingPartnerListData(List<ListMOUPartner> mouList)
        {
            //spe_name
            //sco_abbre
            ListMOUPartner previousItem = null;
            foreach (ListMOUPartner item in mouList.ToList())
            {
                if (previousItem == null) //first record
                {
                    previousItem = item;
                    previousItem.mou_start_date_string = item.mou_start_date.ToString("dd'/'MM'/'yyyy");
                }
                else
                {
                    if (item.mou_partner_id.Equals(previousItem.mou_partner_id))
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
                        mouList.Remove(item);
                    }
                    else
                    {
                        previousItem = item;
                        previousItem.mou_start_date_string = item.mou_start_date.ToString("dd'/'MM'/'yyyy");
                    }
                }
            }
            return;
        }
        public void addMOUPartner(PartnerInfo input, int mou_id, BLL.Authen.LoginRepo.User user)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    List<PartnerScope> totalRelatedPS = new List<PartnerScope>();
                    int partner_id_item = 0;
                    //new partner
                    if (input.partner_id == 0)
                    {
                        //add Article.
                        //add ArticleVersion.
                        //add Partner.
                        Article a = db.Articles.Add(new Article
                        {
                            need_approved = false,
                            article_status_id = 2,
                            account_id = user is null ? 1 : user.account.account_id,
                        });
                        ArticleVersion av = db.ArticleVersions.Add(new ArticleVersion
                        {
                            publish_time = DateTime.Now,
                            version_title = "",
                            article_id = a.article_id,
                            language_id = 1
                        });
                        db.Partners.Add(new ENTITIES.Partner
                        {
                            partner_name = input.partnername_add,
                            website = input.website_add,
                            address = input.address_add,
                            country_id = input.nation_add,
                            article_id = a.article_id
                        });
                        //checkpoint 2
                        db.SaveChanges();
                        ENTITIES.Partner objPartner = db.Partners.Where(x => x.partner_name == input.partnername_add).First();
                        partner_id_item = objPartner.partner_id;
                    }
                    else //old partner
                    {
                        partner_id_item = input.partner_id;
                    }
                    //add to MOUPartner via each partner of MOU
                    db.MOUPartners.Add(new ENTITIES.MOUPartner
                    {
                        mou_id = mou_id,
                        partner_id = partner_id_item,
                        mou_start_date = DateTime.ParseExact(input.sign_date_mou_add, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        contact_point_name = input.represent_add,
                        contact_point_email = input.email_add,
                        contact_point_phone = input.phone_add
                    });
                    //PartnerScope and MOUPartnerScope
                    foreach (int tokenScope in input.coop_scope_add.ToList())
                    {
                        PartnerScope objPS = db.PartnerScopes.Where(x => x.partner_id == partner_id_item && x.scope_id == tokenScope).FirstOrDefault();
                        int partner_scope_id = 0;
                        if (objPS == null)
                        {
                            db.PartnerScopes.Add(new PartnerScope
                            {
                                partner_id = partner_id_item,
                                scope_id = tokenScope,
                                reference_count = 1
                            });
                            //checkpoint 3
                            db.SaveChanges();
                            PartnerScope newObjPS = db.PartnerScopes.Where(x => x.partner_id == partner_id_item && x.scope_id == tokenScope).FirstOrDefault();
                            partner_scope_id = newObjPS.partner_scope_id;
                            totalRelatedPS.Add(newObjPS);
                        }
                        else
                        {
                            objPS.reference_count += 1;
                            db.Entry(objPS).State = EntityState.Modified;
                            partner_scope_id = objPS.partner_scope_id;
                            totalRelatedPS.Add(objPS);
                        }
                        db.MOUPartnerScopes.Add(new MOUPartnerScope
                        {
                            partner_scope_id = partner_scope_id,
                            mou_id = mou_id
                        });
                    }
                    //checkpoint 4
                    db.SaveChanges();
                    //MOUPartnerSpe
                    MOUPartner objMOUPartner = db.MOUPartners.Where(x => (x.mou_id == mou_id && x.partner_id == partner_id_item)).First();
                    foreach (int tokenSpe in input.specialization_add.ToList())
                    {
                        db.MOUPartnerSpecializations.Add(new MOUPartnerSpecialization
                        {
                            mou_partner_id = objMOUPartner.mou_partner_id,
                            specialization_id = tokenSpe,
                        });
                    }
                    db.SaveChanges();
                    transaction.Commit();

                    //change status corressponding MOU/MOA
                    using (DbContextTransaction dbContext = db.Database.BeginTransaction())
                    {
                        try
                        {
                            List<int> listPS = totalRelatedPS.Select(x => x.partner_scope_id).ToList();
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

        public void editMOUPartner(PartnerInfo input, int mou_id, int mou_partner_id, BLL.Authen.LoginRepo.User user)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    List<PartnerScope> totalRelatedPS = new List<PartnerScope>();
                    db.Configuration.LazyLoadingEnabled = false;
                    db.Configuration.ProxyCreationEnabled = false;
                    int partner_id_item = 0;
                    //new partner
                    if (input.partner_id == 0)
                    {
                        //add Article.
                        //add ArticleVersion.
                        //add Partner.
                        Article a = db.Articles.Add(new Article
                        {
                            need_approved = false,
                            article_status_id = 2,
                            account_id = user is null ? 1 : user.account.account_id,
                        });
                        ArticleVersion av = db.ArticleVersions.Add(new ArticleVersion
                        {
                            publish_time = DateTime.Now,
                            version_title = "",
                            article_id = a.article_id,
                            language_id = 1
                        });
                        db.Partners.Add(new ENTITIES.Partner
                        {
                            partner_name = input.partnername_add,
                            website = input.website_add,
                            address = input.address_add,
                            country_id = input.nation_add,
                            article_id = a.article_id
                        });
                        //checkpoint 2
                        db.SaveChanges();
                        ENTITIES.Partner objPartner = db.Partners.Where(x => x.partner_name == input.partnername_add).First();
                        partner_id_item = objPartner.partner_id;
                    }
                    else //old partner
                    {
                        partner_id_item = input.partner_id;
                    }

                    //edit MOUPartner
                    ENTITIES.MOUPartner moup = db.MOUPartners.Where(x => x.mou_partner_id == mou_partner_id).First();
                    moup.mou_id = mou_id;
                    moup.partner_id = partner_id_item;
                    moup.mou_start_date = DateTime.ParseExact(input.sign_date_mou_add, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    moup.contact_point_name = input.represent_add;
                    moup.contact_point_email = input.email_add;
                    moup.contact_point_phone = input.phone_add;
                    db.Entry(moup).State = EntityState.Modified;
                    //checkpoint 3
                    db.SaveChanges();

                    //remove old records of MOUPartnerSpecialization.
                    db.MOUPartnerSpecializations.RemoveRange(db.MOUPartnerSpecializations.Where(x => x.mou_partner_id == mou_partner_id).ToList());
                    //checkpoint 4
                    db.SaveChanges();

                    //add new records of MOUPartnerSpecialization.
                    foreach (int tokenSpe in input.specialization_add.ToList())
                    {
                        db.MOUPartnerSpecializations.Add(new MOUPartnerSpecialization
                        {
                            mou_partner_id = mou_partner_id,
                            specialization_id = tokenSpe,
                        });
                    }
                    //checkpoint 5
                    db.SaveChanges();

                    //get old records of PartnerScope in MOU
                    string sql_old_partnerScope = @"select t1.partner_scope_id,t1.partner_id,scope_id,reference_count from IA_Collaboration.PartnerScope t1 inner join 
                        IA_Collaboration.MOUPartnerScope t2 on
                        t1.partner_scope_id = t2.partner_scope_id
                        inner join IA_Collaboration.MOUPartner t3 on
                        t3.mou_id = t2.mou_id and t3.partner_id = t1.partner_id
                        where t3.mou_partner_id = @mou_partner_id and t2.mou_bonus_id is null";
                    List<PartnerScope> listOldPS = db.Database.SqlQuery<PartnerScope>(sql_old_partnerScope,
                        new SqlParameter("mou_partner_id", mou_partner_id)).ToList();

                    //reset old records of scopes of partner.
                    foreach (PartnerScope token in listOldPS.ToList())
                    {
                        PartnerScope objPS = db.PartnerScopes.Where(x => x.partner_scope_id == token.partner_scope_id).First();
                        //decrese refer_count in PartnerScope
                        objPS.reference_count -= 1;
                        db.Entry(objPS).State = EntityState.Modified;

                        //delete record of MOUPartnerScope.
                        db.MOUPartnerScopes.Remove(db.MOUPartnerScopes.Where(x => x.partner_scope_id == token.partner_scope_id && x.mou_id == mou_id).First());
                        totalRelatedPS.Add(objPS);
                    }

                    //add new records of scopes of partner.
                    foreach (int tokenScope in input.coop_scope_add.ToList())
                    {
                        PartnerScope objPS = db.PartnerScopes.Where(x => x.partner_id == partner_id_item && x.scope_id == tokenScope).FirstOrDefault();
                        int partner_scope_id = 0;
                        if (objPS == null) //add new to PartnerScope
                        {
                            db.PartnerScopes.Add(new PartnerScope
                            {
                                partner_id = partner_id_item,
                                scope_id = tokenScope,
                                reference_count = 1
                            });
                            //checkpoint 6
                            db.SaveChanges();
                            PartnerScope newObjPS = db.PartnerScopes.Where(x => x.partner_id == partner_id_item && x.scope_id == tokenScope).FirstOrDefault();
                            partner_scope_id = newObjPS.partner_scope_id;
                            totalRelatedPS.Add(newObjPS);
                        }
                        else
                        {
                            objPS.reference_count += 1;
                            db.Entry(objPS).State = EntityState.Modified;
                            partner_scope_id = objPS.partner_scope_id;
                            totalRelatedPS.Add(objPS);
                            db.SaveChanges();
                        }
                        db.MOUPartnerScopes.Add(new MOUPartnerScope
                        {
                            partner_scope_id = partner_scope_id,
                            mou_id = mou_id
                        });
                        db.SaveChanges();
                    }
                    //checkpoint 7
                    db.SaveChanges();
                    transaction.Commit();

                    //change status corressponding MOU/MOA
                    using (DbContextTransaction dbContext = db.Database.BeginTransaction())
                    {
                        try
                        {
                            List<int> listPS = totalRelatedPS.Select(x => x.partner_scope_id).ToList();
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
        public void deleteMOUPartner(int mou_id, int mou_partner_id)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    List<PartnerScope> totalRelatedPS = new List<PartnerScope>();
                    //get partner of moupartner deleted.
                    int partner_id_item = db.MOUPartners.Find(mou_partner_id).partner_id;

                    //get all scopes of partner in MOU.
                    string sql_old_partnerScope = @"select t1.partner_scope_id,partner_id,scope_id,reference_count from IA_Collaboration.PartnerScope t1 inner join 
                        IA_Collaboration.MOUPartnerScope t2 on
                        t1.partner_scope_id = t2.partner_scope_id
                        where mou_id = @mou_id and partner_id = @partner_id";
                    List<PartnerScope> listPS = db.Database.SqlQuery<PartnerScope>(sql_old_partnerScope,
                        new SqlParameter("mou_id", mou_id),
                        new SqlParameter("partner_id", partner_id_item)).ToList();
                    totalRelatedPS.AddRange(listPS);

                    foreach (PartnerScope partnerScope in listPS.ToList())
                    {
                        PartnerScope ps = db.PartnerScopes.Find(partnerScope.partner_scope_id);
                        ps.reference_count -= 1;
                        db.Entry(ps).State = EntityState.Modified;
                        db.MOUPartnerScopes.Remove(db.MOUPartnerScopes.Where(x => x.partner_scope_id == partnerScope.partner_scope_id && x.mou_id == mou_id).First());
                    }
                    //checkpoint 1
                    db.SaveChanges();

                    db.MOUPartnerSpecializations.RemoveRange(db.MOUPartnerSpecializations.Where(x => x.mou_partner_id == mou_partner_id).ToList());
                    db.MOUPartners.Remove(db.MOUPartners.Find(mou_partner_id));

                    //checkpoint 2
                    db.SaveChanges();
                    transaction.Commit();

                    //change status corressponding MOU/MOA
                    using (DbContextTransaction dbContext = db.Database.BeginTransaction())
                    {
                        try
                        {
                            List<int> listObjPS = totalRelatedPS.Select(x => x.partner_scope_id).ToList();
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

        public bool checkLastPartner(int mou_id)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    return db.MOUPartners.Where(x => x.mou_id == mou_id).Count() == 1 ? true : false;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }
        public bool CheckMOAPartnerExistedInMOU(int mou_partner_id)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    string sql_check = @"select t3.* from IA_Collaboration.MOA t1
                        inner join IA_Collaboration.MOAPartner t2
                        on t2.moa_id = t1.moa_id
                        inner join IA_Collaboration.MOUPartner t3
                        on t3.partner_id = t2.partner_id
                        where mou_partner_id = @mou_partner_id and t1.is_deleted = 0";
                    List<MOUPartner> listExisted = db.Database.SqlQuery<MOUPartner>(sql_check,
                        new SqlParameter("mou_partner_id", mou_partner_id)).ToList();
                    return listExisted.Count() > 0 ? true : false;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }
        public bool IsMOUBonusExisted(int mou_partner_id)
        {
            try
            {
                string sql_check = @"select t1.* from IA_Collaboration.MOUBonus t1
                    inner join IA_Collaboration.MOUPartner t2
                    on t2.mou_id = t1.mou_id
                    inner join IA_Collaboration.MOUPartnerScope t3
                    on t3.mou_id = t1.mou_id and t3.mou_bonus_id = t1.mou_bonus_id
                    inner join IA_Collaboration.PartnerScope t4
                    on t4.partner_scope_id = t3.partner_scope_id and t4.partner_id = t2.partner_id
                    where mou_partner_id = @mou_partner_id";
                List<MOUBonu> exMOUList = db.Database.SqlQuery<MOUBonu>(sql_check,
                    new SqlParameter("mou_partner_id", mou_partner_id)).ToList();
                return exMOUList.Count == 0 ? false : true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PartnerHistory> listMOUPartnerHistory(int mou_partner_id)
        {
            string sql_history =
                    @"WITH b AS(
                        SELECT mou.mou_id, mou.mou_code, moup.mou_start_date, mou.mou_end_date, moup.mou_partner_id, ps.partner_id, ps.scope_id, ps.partner_scope_id
                        FROM IA_Collaboration.MOU mou
                        join IA_Collaboration.MOUPartner moup on mou.mou_id = moup.mou_id
                        join IA_Collaboration.MOUPartnerScope moups on moups.mou_id = mou.mou_id
                        join IA_Collaboration.PartnerScope ps on ps.partner_scope_id = moups.partner_scope_id 
                        where mou.is_deleted = 0 and moup.mou_partner_id = @mou_partner_id)
                        SELECT N'Tổ chức hoạt động học thuật' 'content', ac.full_name, ap.add_time
                        FROM SMIA_AcademicActivity.AcademicActivity aa 
                        INNER JOIN SMIA_AcademicActivity.ActivityPartner ap ON aa.activity_id = ap.activity_id 
                        INNER JOIN b ON b.partner_scope_id = ap.partner_scope_id
                        INNER JOIN General.Account ac on ac.account_id = ap.account_id
                        and b.mou_start_date <= aa.activity_date_start and aa.activity_date_end <= b.mou_end_date
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
                        and b.mou_start_date <= ac.plan_study_start_date and ac.plan_study_end_date <= b.mou_end_date
                        UNION ALL
                        SELECT DISTINCT N'Ký kết biên bản ghi nhớ' 'content', acc.full_name, mou.add_time
                        FROM b INNER JOIN IA_Collaboration.MOUPartner mp ON b.partner_id = mp.partner_id and mp.mou_partner_id = b.mou_partner_id
                        INNER JOIN IA_Collaboration.MOU mou ON mou.mou_id = mp.mou_id
                        INNER JOIN General.Account acc on acc.account_id = mou.account_id
                        UNION ALL
                        SELECT DISTINCT N'Ký kết biên bản thỏa thuận' 'content', acc.full_name, moa.add_time
                        FROM b INNER JOIN IA_Collaboration.MOAPartner mp ON b.partner_id = mp.partner_id
                        INNER JOIN IA_Collaboration.MOA moa ON moa.moa_id = mp.moa_id
                        LEFT JOIN IA_Collaboration.MOU mou on mou.mou_id = moa.mou_id
                        LEFT JOIN IA_Collaboration.MOUPartner moup on moup.mou_id = mou.mou_id and moup.mou_partner_id = b.mou_partner_id
                        LEFT JOIN General.Account acc on acc.account_id = moa.account_id
                        UNION ALL
                        SELECT DISTINCT N'Ký kết biên bản ghi nhớ bổ sung' 'content', acc.full_name, mb.add_time
                        FROM b INNER JOIN IA_Collaboration.MOUPartner mp ON b.partner_id = mp.partner_id and mp.mou_partner_id = b.mou_partner_id
                        INNER JOIN IA_Collaboration.MOUBonus mb ON mp.mou_id = mb.mou_id 
                        LEFT JOIN IA_Collaboration.MOuPartnerScope mps ON mps.mou_bonus_id = mb.mou_bonus_id
                        LEFT JOIN General.Account acc on acc.account_id = mb.account_id
                        WHERE (mb.mou_bonus_end_date IS NOT NULL) OR (mps.partner_scope_id IS NOT NULL)
                        UNION ALL
                        SELECT DISTINCT N'Ký kết biên bản thỏa thuận bổ sung' 'content', acc.full_name, mb.add_time
                        FROM b INNER JOIN IA_Collaboration.MOAPartner mp ON b.partner_id = mp.partner_id
                        INNER JOIN IA_Collaboration.MOABonus mb ON mp.moa_id = mb.moa_id 
                        LEFT JOIN IA_Collaboration.MOAPartnerScope mps ON mps.moa_bonus_id = mb.moa_bonus_id
                        LEFT JOIN IA_Collaboration.MOA moa on moa.moa_id = mb.moa_id
                        LEFT JOIN IA_Collaboration.MOU mou on mou.mou_id = moa.mou_id
                        LEFT JOIN IA_Collaboration.MOUPartner moup on moup.mou_id = mou.mou_id and moup.mou_partner_id = b.mou_partner_id
                        LEFT JOIN General.Account acc on acc.account_id = mb.account_id
                        WHERE (mb.moa_bonus_end_date IS NOT NULL) OR (mps.partner_scope_id IS NOT NULL)
                        ORDER BY add_time ASC";
            List<PartnerHistory> historyList = db.Database.SqlQuery<PartnerHistory>(sql_history,
                new SqlParameter("mou_partner_id", mou_partner_id)).ToList();
            return historyList;
        }
        public List<ENTITIES.Partner> GetPartners(int mou_id)
        {
            try
            {
                string sql_partnerList = @"select * from IA_Collaboration.Partner
                    where partner_id not in (
                    select partner_id from IA_Collaboration.MOUPartner
                    where mou_id = @mou_id)";
                List<ENTITIES.Partner> partnerList = db.Database.SqlQuery<ENTITIES.Partner>(sql_partnerList
                    , new SqlParameter("mou_id", mou_id)).ToList();
                return partnerList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<Specialization> getPartnerMOUSpe()
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
        public List<CollaborationScope> getPartnerMOUScope(int mou_id)
        {
            try
            {
                string sql_scopeList = @"select * from IA_Collaboration.CollaborationScope
                    where scope_id not in (
                    select distinct t2.scope_id from IA_Collaboration.MOUPartnerScope t1 left join
                    IA_Collaboration.PartnerScope t2 on 
                    t1.partner_scope_id = t2.partner_scope_id
                    where mou_id = @mou_id and mou_bonus_id is not null)";
                List<CollaborationScope> scopeList = db.Database.SqlQuery<CollaborationScope>(sql_scopeList,
                    new SqlParameter("mou_id", mou_id)).ToList();
                return scopeList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ListMOUPartner getMOUPartnerDetail(int mou_partner_id)
        {
            try
            {
                string sql_mouPartnerList =
                    @"select t1.mou_partner_id,t2.partner_name,t4.specialization_name,
                        t2.website,t1.contact_point_name,t1.contact_point_phone,
                        t1.contact_point_email,t1.mou_start_date,t5.scope_abbreviation,
                        t1.mou_id
                        from IA_Collaboration.MOUPartner t1
                        left join IA_Collaboration.Partner t2
                        on t2.partner_id = t1.partner_id 
                        left join IA_Collaboration.MOUPartnerSpecialization t3
                        on t3.mou_partner_id = t1.mou_partner_id
                        left join General.Specialization t4
                        on t4.specialization_id = t3.specialization_id
                        left join 
                        (select mou_id,partner_id,scope_abbreviation from IA_Collaboration.PartnerScope a 
                        inner join IA_Collaboration.MOUPartnerScope b on
                        b.partner_scope_id = a.partner_scope_id
                        inner join IA_Collaboration.CollaborationScope c on
                        c.scope_id = a.scope_id) t5 on
                        t5.partner_id = t2.partner_id and t5.mou_id = t1.mou_id
                        left join General.Country t6 on 
                        t6.country_id = t2.country_id
                        where t1.mou_partner_id = @mou_partner_id";
                List<ListMOUPartner> mouList = db.Database.SqlQuery<ListMOUPartner>(sql_mouPartnerList,
                    new SqlParameter("mou_partner_id", mou_partner_id)).ToList();
                handlingPartnerListData(mouList);
                ListMOUPartner obj = mouList.First();
                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string CheckPartnerExistedInMOU(int mou_id, string partner_name)
        {
            try
            {
                string sql = @"select partner_name from IA_Collaboration.MOUPartner t1 left join
                    IA_Collaboration.Partner t2 on
                    t1.partner_id = t2.partner_id
                    where t1.mou_id = @mou_id and t2.partner_name like @partner_name";
                string result = db.Database.SqlQuery<string>(sql,
                    new SqlParameter("partner_name", '%' + partner_name + '%'),
                    new SqlParameter("mou_id", mou_id)).FirstOrDefault();
                return result is null ? "" : result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string CheckPartnerExistedInEditMOU(int mou_partner_id, string partner_name)
        {
            try
            {
                string sql = @"select partner_name from IA_Collaboration.MOUPartner t1 left join
                    IA_Collaboration.Partner t2 on
                    t1.partner_id = t2.partner_id
                    where t1.mou_partner_id != @mou_partner_id and t2.partner_name like @partner_name";
                string result = db.Database.SqlQuery<string>(sql,
                    new SqlParameter("partner_name", '%' + partner_name + '%'),
                    new SqlParameter("mou_partner_id", mou_partner_id)).FirstOrDefault();
                return result is null ? "" : result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
