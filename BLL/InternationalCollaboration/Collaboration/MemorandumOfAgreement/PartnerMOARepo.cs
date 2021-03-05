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
    class PartnerMOARepo
    {
        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        public List<ListMOAPartner> listAllMOAPartner(int moa_id)
        {
            try
            {
                string sql_moaPartnerList =
                    @"select t3.partner_name,t3.country_id,t8.country_name,t7.specialization_name,t3.website,
                        t2.contact_point_name,t2.contact_point_phone,t2.contact_point_email,
                        t1.moa_start_date,t5.scope_name
                        from IA_Collaboration.MOAPartner t1 inner join 
                        IA_Collaboration.MOUPartner t2 on t1.mou_partner_id = t2.mou_partner_id
                        inner join IA_Collaboration.Partner t3 on t2.partner_id = t3.partner_id
                        inner join IA_Collaboration.MOUPartnerScope t4 on t4.mou_id = t2.mou_id and t4.partner_id = t2.partner_id
                        inner join IA_MasterData.CollaborationScope t5 on t5.scope_id = t4.scope_id
                        inner join IA_Collaboration.MOUPartnerSpecialization t6 on t6.mou_partner_id = t2.mou_partner_id
                        inner join General.Specialization t7 on t7.specialization_id = t6.specialization_id
                        inner join General.Country t8 on t8.country_id = t3.country_id
                        where t1.moa_id = @moa_id";
                List<ListMOAPartner> moaList = db.Database.SqlQuery<ListMOAPartner>(sql_moaPartnerList,
                    new SqlParameter("moa_id", moa_id)).ToList();
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
        public void addMOAPartner(MOAPartnerAdd input)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    //mouPartner
                    db.MOAPartners.Add(new MOAPartner
                    {
                        moa_id = input.mou_id,
                        moa_start_date = input.moa_start_date,
                        mou_partner_id = input.mou_partner_id
                    });
                    MOAPartner mp = db.MOAPartners.Where(x => x.moa_id == input.moa_id && x.mou_partner_id == input.mou_partner_id).First();
                    //spe
                    foreach (int spe_id in input.list_spe)
                    {
                        db.MOUPartnerSpecializations.Add(new MOUPartnerSpecialization
                        {
                            mou_partner_id = input.mou_partner_id,
                            specialization_id = spe_id
                        });
                    }
                    //scope
                    foreach (int scope_id in input.list_scope)
                    {
                        db.MOUPartnerScopes.Add(new MOUPartnerScope
                        {
                            mou_id = input.mou_id,
                            partner_id = input.partner_id,
                            scope_id = scope_id
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
        public void editMOAPartner(MOAPartnerAdd input)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    //edit mouPartner
                    MOAPartner mp = db.MOAPartners.Where(x => x.moa_id == input.moa_id && x.mou_partner_id == input.mou_partner_id).First();
                    mp.moa_start_date = input.moa_start_date;

                    //remove old spe.
                    List<MOAPartnerSpecialization> mpsList = db.MOAPartnerSpecializations.Where(x => x.moa_partner_id == mp.moa_partner_id).ToList();
                    foreach (MOAPartnerSpecialization item in mpsList.ToList())
                    {
                        db.MOAPartnerSpecializations.Remove(item);
                    }
                    //spe
                    //foreach (int spe_id in input.list_spe)
                    //{
                    //    db.MOAPartnerSpecializations.Add(new MOAPartnerSpecialization
                    //    {
                    //        moa_partner_id = input.moa_partner_id,
                    //        specialization_id = spe_id
                    //    });
                    //}

                    //remove old scope.
                    List<MOUPartnerScope> mscList = db.MOUPartnerScopes.Where(x => x.mou_id == input.mou_id && x.partner_id == input.partner_id).ToList();
                    foreach (MOUPartnerScope item in mscList.ToList())
                    {
                        db.MOUPartnerScopes.Remove(item);
                    }
                    //scope
                    //foreach (int scope_id in input.list_scope)
                    //{
                    //    db.MOUPartnerScopes.Add(new MOUPartnerScope
                    //    {
                    //        mou_id = input.mou_id,
                    //        partner_id = input.partner_id,
                    //        scope_id = scope_id
                    //    });
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
        public void DeleteMOAPartner(int moa_id, int partner_id, int moa_partner_id)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    //remove old spe.
                    List<MOAPartnerSpecialization> mpsList = db.MOAPartnerSpecializations.Where(x => x.moa_partner_id == moa_partner_id).ToList();
                    foreach (MOAPartnerSpecialization item in mpsList.ToList())
                    {
                        db.MOAPartnerSpecializations.Remove(item);
                    }
                    //remove old scope.
                    List<MOAPartnerScope> mscList = db.MOAPartnerScopes.Where(x => x.moa_partner_id == moa_partner_id).ToList();
                    foreach (MOAPartnerScope item in mscList.ToList())
                    {
                        db.MOAPartnerScopes.Remove(item);
                    }
                    MOUPartner m = db.MOUPartners.Where(x => x.mou_partner_id == moa_partner_id).First();
                    db.MOUPartners.Remove(m);
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
            return historyList;
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

        public List<ENTITIES.Partner> getExMOAPartner()
        {
            try
            {
                string sql_partnerList = @"select * from IA_Collaboration.Partner";
                List<ENTITIES.Partner> partnerList = db.Database.SqlQuery<ENTITIES.Partner>(sql_partnerList).ToList();
                return partnerList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public class ListMOAPartner
        {
            public ListMOAPartner() { }
            public int mou_partner_id { get; set; }
            public int mou_id { get; set; }
            public int moa_id { get; set; }
            public string partner_name { get; set; }
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
    }
}
