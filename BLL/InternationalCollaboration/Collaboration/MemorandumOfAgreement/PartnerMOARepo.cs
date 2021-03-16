using ENTITIES;
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
        public List<ListMOAPartner> listAllMOAPartner(string partner_name, string nation, string specialization,int moa_id)
        {
            try
            {
                string sql_moaPartnerList =
                    @"select 
                        moap.moa_partner_id,moap.moa_id, pa.partner_id, pa.partner_name, co.country_name, spec.specialization_name, pa.website,
                        moup.contact_point_name, moup.contact_point_phone, moup.contact_point_email,
                        moap.moa_start_date, cs.scope_abbreviation, moa.moa_code
                        from IA_Collaboration.MOAPartner moap
                        inner join IA_Collaboration.MOA moa on moa.moa_id = moap.moa_id
                        inner join IA_Collaboration.MOUPartner moup on moup.mou_id = moa.mou_id and moup.partner_id = moap.partner_id
                        inner join IA_Collaboration.MOUPartnerSpecialization moups on moup.mou_partner_id = moups.mou_partner_id
                        inner join General.Specialization spec on spec.specialization_id = moups.specialization_id
                        inner join IA_Collaboration.[Partner] pa on pa.partner_id = moap.partner_id
                        inner join General.Country co on co.country_id = pa.country_id
                        inner join IA_Collaboration.MOAPartnerScope moaps on moaps.moa_id = moap.moa_id
                        inner join IA_Collaboration.PartnerScope ps on ps.partner_scope_id = moaps.partner_scope_id
                        inner join IA_Collaboration.CollaborationScope cs on cs.[scope_id] = ps.[scope_id]
                        where moap.moa_id = @moa_id
                        and pa.partner_name like @partner_name
                        and co.country_name like @nation
                        and spec.specialization_name like @specialization
                        order by moap.moa_start_date";
                List<ListMOAPartner> moaList = db.Database.SqlQuery<ListMOAPartner>(sql_moaPartnerList,
                    new SqlParameter("moa_id", moa_id),
                    new SqlParameter("partner_name", '%'+ partner_name + '%'),
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
                    }
                }
            }
            return;
        }
        public void addMOAPartner(MOAPartnerInfo input,int moa_id)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    DateTime moa_start_date = DateTime.ParseExact(input.sign_date_moa_add, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    //add MOAPartner
                    db.MOAPartners.Add(new MOAPartner
                    {
                        moa_id = moa_id,
                        moa_start_date = moa_start_date,
                        partner_id = input.partner_id
                    });

                    foreach (int itemScope in input.coop_scope_add.ToList())
                    {
                        PartnerScope psCheck = db.PartnerScopes.Where(x => x.partner_id == input.partner_id && x.scope_id == itemScope).FirstOrDefault();
                        if (psCheck != null)
                        {
                            psCheck.reference_count += 1;
                            db.MOAPartnerScopes.Add(new MOAPartnerScope
                            {
                                partner_scope_id = psCheck.partner_scope_id,
                                moa_id = moa_id
                            });
                        } else
                        {
                            PartnerScope psAdded = db.PartnerScopes.Add(new PartnerScope
                            {
                                partner_id = input.partner_id,
                                scope_id = itemScope,
                                reference_count = 1
                            });
                            db.MOAPartnerScopes.Add(new MOAPartnerScope
                            {
                                partner_scope_id = psAdded.partner_scope_id,
                                moa_id = moa_id
                            });
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

        public void editMOAPartner(MOAPartnerEdited input, int moa_id)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    DateTime sign_date = DateTime.ParseExact(input.sign_date_string, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    //edit MOAPartner
                    MOAPartner mp = db.MOAPartners.Where(x => x.moa_partner_id == input.moa_partner_id).First();
                    mp.moa_start_date = sign_date;
                    mp.partner_id = db.Partners.Where(x => x.partner_name == input.partner_name).First().partner_id;
                    db.Entry(mp).State = EntityState.Modified;
                    db.SaveChanges();

                    //get old records of PartnerScope in MOA
                    string sql_old_partnerScope = @"select t1.partner_scope_id,partner_id,scope_id from IA_Collaboration.PartnerScope t1 inner join 
                        IA_Collaboration.MOAPartnerScope t2 on
                        t1.partner_scope_id = t2.partner_scope_id
                        where moa_id = @moa_id";
                    List<PartnerScope> listOldPS = db.Database.SqlQuery<PartnerScope>(sql_old_partnerScope,
                        new SqlParameter("moa_id", moa_id)).ToList();

                    //reset old records of scopes of partner.
                    foreach (PartnerScope token in listOldPS.ToList())
                    {
                        PartnerScope objPS = db.PartnerScopes.Where(x => x.partner_scope_id == token.partner_scope_id).First();
                        //decrese refer_count in PartnerScope
                        objPS.reference_count -= 1;
                        db.Entry(objPS).State = EntityState.Modified;

                        //delete record of MOUPartnerScope.
                        db.MOAPartnerScopes.Remove(db.MOAPartnerScopes.Where(x => x.partner_scope_id == token.partner_scope_id).First());
                    }

                    //add new records of scopes of partner.
                    foreach (int tokenScope in input.scopes.ToList())
                    {
                        PartnerScope objPS = db.PartnerScopes.Where(x => x.partner_id == mp.partner_id && x.scope_id == tokenScope).FirstOrDefault();
                        int partner_scope_id = 0;
                        if (objPS == null) //add new to PartnerScope
                        {
                            db.PartnerScopes.Add(new PartnerScope
                            {
                                partner_id = mp.partner_id,
                                scope_id = tokenScope,
                                reference_count = 0
                            });
                            //checkpoint 3
                            db.SaveChanges();
                            PartnerScope newObjPS = db.PartnerScopes.Where(x => x.partner_id == mp.partner_id && x.scope_id == tokenScope).FirstOrDefault();
                            partner_scope_id = newObjPS.partner_scope_id;
                        }
                        else
                        {
                            objPS.reference_count += 1;
                            db.Entry(objPS).State = EntityState.Modified;
                            partner_scope_id = objPS.partner_scope_id;
                        }
                        db.MOAPartnerScopes.Add(new MOAPartnerScope
                        {
                            partner_scope_id = partner_scope_id,
                            moa_id = moa_id
                        });
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
        public void deleteMOAPartner(int moa_id,int moa_partner_id)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    //get partner of moupartner deleted.
                    int partner_id_item = db.MOAPartners.Find(moa_partner_id).partner_id;

                    //get all scopes of partner in MOU.
                    string sql_old_partnerScope = @"select t1.partner_scope_id,partner_id,scope_id from IA_Collaboration.PartnerScope t1 inner join 
                        IA_Collaboration.MOAPartnerScope t2 on
                        t1.partner_scope_id = t2.partner_scope_id
                        where moa_id = @moa_id and partner_id = @partner_id";
                    List<PartnerScope> listPS = db.Database.SqlQuery<PartnerScope>(sql_old_partnerScope,
                        new SqlParameter("moa_id", moa_id),
                        new SqlParameter("partner_id", partner_id_item)).ToList();

                    foreach (PartnerScope partnerScope in listPS.ToList())
                    {
                        PartnerScope ps = db.PartnerScopes.Find(partnerScope.partner_scope_id);
                        ps.reference_count -= 1;
                        db.Entry(ps).State = EntityState.Modified;
                        db.MOAPartnerScopes.Remove(db.MOAPartnerScopes.Where(x => x.partner_scope_id == partnerScope.partner_scope_id && x.moa_id == moa_id).First());
                    }
                    //checkpoint 1
                    db.SaveChanges();

                    //checkpoint 2
                    db.MOAPartners.Remove(db.MOAPartners.Find(moa_partner_id));
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
        public List<PartnerHistory> listMOUPartnerHistory(int mou_partner_id)
        {
            string sql_history =
                    @"select main.content,acc.full_name,main.add_time from (
                    select 
                    a1.mou_partner_id,a2.account_id,a2.add_time,
                    N'Ký kết biên bản ghi nhớ' as content
                    from IA_Collaboration.MOUPartner a1
                    inner join IA_Collaboration.MOU a2 on a1.mou_id = a2.mou_id
                    union all
                    select c2.mou_partner_id,c1.account_id,c1.add_time,
                    N'Ký kết biên bản thỏa thuận' as content from IA_Collaboration.MOA c1
                    inner join IA_Collaboration.MOUPartner c2 on c1.mou_id = c2.mou_id
                    ) as main
                    left join General.Account acc on main.account_id = acc.account_id
                    where mou_partner_id = @mou_partner_id
                    order by add_time ";
            List<PartnerHistory> historyList = db.Database.SqlQuery<PartnerHistory>(sql_history,
                new SqlParameter("mou_partner_id", mou_partner_id)).ToList();
            foreach (PartnerHistory p in historyList)
            {
                p.add_time_string = p.add_time.ToString("dd'/'MM'/'yyyy");
            }
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
        public List<ENTITIES.Partner> getPartnerMOA(int moa_id)
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
        public CustomPartnerMOA getPartnerDetail(int moa_partner_id)
        {
            try
            {
                string sql_partnerList_1 = @"select t5.country_name, t3.website, t3.address, 
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
                        where c.moa_partner_id = @moa_partner_id";
                CustomPartnerMOA obj1 = db.Database.SqlQuery<CustomPartnerMOA>(sql_partnerList_1,
                    new SqlParameter("moa_partner_id", moa_partner_id)).First();
                List<ScopeAndStartDate> obj2 = db.Database.SqlQuery<ScopeAndStartDate>(sql_partnerList_2,
                    new SqlParameter("moa_partner_id", moa_partner_id)).ToList();
                obj1.scopes = new List<int>();
                foreach (ScopeAndStartDate item in obj2.ToList())
                {
                    obj1.scopes.Add(item.scope_id);
                }
                obj1.moa_start_date_string = obj2[0].moa_start_date.ToString("dd'/'MM'/'yyyy");
                return obj1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public class ListMOAPartner
        {
            public ListMOAPartner() { }
            public int moa_partner_id { get; set; }
            public int moa_id { get; set; }
            public string moa_code { get; set; }
            public string partner_name { get; set; }
            public string country_name { get; set; }
            public int partner_id { get; set; }
            public string specialization_name { get; set; }
            public string specialization_abbreviation { get; set; }
            public string website { get; set; }
            public string contact_point_name { get; set; }
            public string contact_point_phone { get; set; }
            public string contact_point_email { get; set; }
            public string moa_start_date_string { get; set; }
            public DateTime moa_start_date { get; set; }
            public string scope_abbreviation { get; set; }
            
        }
        public class MOAPartnerAdd : ListMOAPartner
        {
            public string address { get; set; }
            public int partner_id { get; set; }
            public List<int> list_spe { get; set; }
            public List<int> list_scope { get; set; }
        }
        public class PartnerHistory
        {
            public string full_name { get; set; }
            public string content { get; set; }
            public DateTime add_time { get; set; }
            public string add_time_string { get; set; }
        }
        public class ScopeAndStartDate
        {
            public int scope_id { get; set; }
            public DateTime moa_start_date { get; set; }
        }
        public class MOAPartnerEdited
        {
            public List<int> scopes { get; set; }
            public string partner_name { get; set; }
            public string sign_date_string { get; set; }
            public int moa_partner_id { get; set; }
        }
    }
}
