using ENTITIES;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding
{
    class PartnerMOURepo
    {
        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        public List<ListMOUPartner> listAllMOUPartner(int mou_id)
        {
            try
            {
                string sql_mouPartnerList =
                    @"select tb1.mou_partner_id,tb2.partner_name,tb4.specialization_name
                        ,tb2.website, tb1.contact_point_name, tb1.contact_point_phone
                        ,tb1.contact_point_email, tb1.mou_start_date, tb6.scope_abbreviation, tb1.mou_id
                        from IA_Collaboration.MOUPartner tb1 inner join IA_Collaboration.Partner tb2
                        on tb1.partner_id = tb2.partner_id 
                        inner join IA_Collaboration.MOUPartnerSpecialization tb3 on
                        tb1.mou_partner_id = tb3.mou_partner_id 
                        inner join General.Specialization tb4 on 
                        tb3.specialization_id = tb4.specialization_id
                        inner join IA_Collaboration.MOUPartnerScope tb5 on 
                        tb5.mou_id = tb1.mou_id and tb5.partner_id = tb1.partner_id
                        inner join IA_MasterData.CollaborationScope tb6 on 
                        tb6.scope_id = tb5.scope_id
                        where tb1.mou_id = @mou_id";
                List<ListMOUPartner> mouList = db.Database.SqlQuery<ListMOUPartner>(sql_mouPartnerList,
                    new SqlParameter("mou_id", mou_id)).ToList();
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
                    }
                }
            }
            return;
        }
        public void addMOUPartner(MOUPartnerAdd input)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    //mouPartner
                    db.MOUPartners.Add(new MOUPartner
                    {
                        mou_id = input.mou_id,
                        partner_id = input.partner_id,
                        mou_start_date = input.mou_start_date,
                        contact_point_name = input.contact_point_name,
                        contact_point_phone = input.contact_point_phone,
                        contact_point_email= input.contact_point_email
                    });
                    MOUPartner mp = db.MOUPartners.Where(x => x.mou_id == input.mou_id && x.partner_id == input.partner_id).First();
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
        public void editMOUPartner(MOUPartnerAdd input)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    //edit mouPartner
                    MOUPartner mp = db.MOUPartners.Where(x => x.mou_id == input.mou_id && x.partner_id == input.partner_id).First();
                    mp.contact_point_email = input.contact_point_email;
                    mp.contact_point_name = input.contact_point_name;
                    mp.contact_point_phone = input.contact_point_phone;
                    mp.mou_start_date = input.mou_start_date;

                    //remove old spe.
                    List<MOUPartnerSpecialization> mpsList = db.MOUPartnerSpecializations.Where(x => x.mou_partner_id == mp.mou_partner_id).ToList();
                    foreach (MOUPartnerSpecialization item in mpsList.ToList())
                    {
                        db.MOUPartnerSpecializations.Remove(item);
                    }
                    //spe
                    foreach (int spe_id in input.list_spe)
                    {
                        db.MOUPartnerSpecializations.Add(new MOUPartnerSpecialization
                        {
                            mou_partner_id = input.mou_partner_id,
                            specialization_id = spe_id
                        });
                    }

                    //remove old scope.
                    List<MOUPartnerScope> mscList = db.MOUPartnerScopes.Where(x => x.mou_id == input.mou_id && x.partner_id == input.partner_id).ToList();
                    foreach (MOUPartnerScope item in mscList.ToList())
                    {
                        db.MOUPartnerScopes.Remove(item);
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
        public void deleteMOUPartner(int mou_id, int partner_id, int mou_partner_id)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    //remove old spe.
                    List<MOUPartnerSpecialization> mpsList = db.MOUPartnerSpecializations.Where(x => x.mou_partner_id == mou_partner_id).ToList();
                    foreach (MOUPartnerSpecialization item in mpsList.ToList())
                    {
                        db.MOUPartnerSpecializations.Remove(item);
                    }
                    //remove old scope.
                    List<MOUPartnerScope> mscList = db.MOUPartnerScopes.Where(x => x.mou_id == mou_id && x.partner_id == partner_id).ToList();
                    foreach (MOUPartnerScope item in mscList.ToList())
                    {
                        db.MOUPartnerScopes.Remove(item);
                    }
                    MOUPartner m = db.MOUPartners.Where(x => x.mou_partner_id == mou_partner_id).First();
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
        public List<ENTITIES.Partner> GetPartners()
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
        public ENTITIES.Partner getMOUPartnerById(int partner_id)
        {
            try
            {
                ENTITIES.Partner p = db.Partners.Find(partner_id);
                return p;
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
        public List<CollaborationScope> getPartnerMOUScope()
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
        public List<ListMOUPartner> getMOUPartnerDetail(int mou_partner_id)
        {
            try
            {
                string sql_mouPartnerList =
                    @"select tb1.mou_partner_id,tb2.partner_name,tb4.specialization_name
                        ,tb2.website, tb1.contact_point_name, tb1.contact_point_phone
                        ,tb1.contact_point_email, tb1.mou_start_date, tb6.scope_abbreviation, tb1.mou_id
                        from IA_Collaboration.MOUPartner tb1 inner join IA_Collaboration.Partner tb2
                        on tb1.partner_id = tb2.partner_id 
                        inner join IA_Collaboration.MOUPartnerSpecialization tb3 on
                        tb1.mou_partner_id = tb3.mou_partner_id 
                        inner join General.Specialization tb4 on 
                        tb3.specialization_id = tb4.specialization_id
                        inner join IA_Collaboration.MOUPartnerScope tb5 on 
                        tb5.mou_id = tb1.mou_id and tb5.partner_id = tb1.partner_id
                        inner join IA_MasterData.CollaborationScope tb6 on 
                        tb6.scope_id = tb5.scope_id
                        where tb1.mou_partner_id = @mou_partner_id";
                List<ListMOUPartner> mouList = db.Database.SqlQuery<ListMOUPartner>(sql_mouPartnerList,
                    new SqlParameter("mou_partner_id", mou_partner_id)).ToList();
                handlingPartnerListData(mouList);
                return mouList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public class ListMOUPartner
        {
            public ListMOUPartner() { }
            public int mou_partner_id { get; set; }
            public int mou_id { get; set; }
            public string partner_name { get; set; }
            public string specialization_name { get; set; }
            public string specialization_abbreviation { get; set; }
            public string website { get; set; }
            public string contact_point_name { get; set; }
            public string contact_point_phone { get; set; }
            public string contact_point_email { get; set; }
            public string mou_start_date_string { get; set; }
            public DateTime mou_start_date { get; set; }
            public string scope_abbreviation { get; set; }
        }
        public class MOUPartnerAdd : ListMOUPartner
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
