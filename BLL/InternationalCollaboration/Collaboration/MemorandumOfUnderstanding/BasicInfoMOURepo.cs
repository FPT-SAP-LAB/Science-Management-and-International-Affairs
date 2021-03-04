using ENTITIES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding
{
    class BasicInfoMOURepo
    {
        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        public MOU getBasicInfoMOU()
        {
            try
            {
                string sql_mouBasicInfo =
                    @"select tb2.mou_partner_id,
                        tb1.mou_code,tb3.partner_name,tb3.website,tb2.contact_point_name
                        ,tb2.contact_point_email,tb2.contact_point_phone,tb1.evidence,
                        tb2.mou_start_date, tb1.mou_end_date, tb1.mou_note, tb10.office_abbreviation, tb5.scope_abbreviation
                        ,tb7.specialization_name, tb9.mou_status_name
                        from IA_Collaboration.MOU tb1 inner join IA_Collaboration.MOUPartner tb2
                        on tb1.mou_id = tb2.mou_id inner join IA_Collaboration.Partner tb3 
                        on tb2.partner_id = tb3.partner_id inner join IA_Collaboration.MOUPartnerScope tb4
                        on tb4.mou_id = tb2.mou_id and tb4.partner_id = tb2.partner_id
                        inner join IA_MasterData.CollaborationScope tb5
                        on tb4.scope_id = tb5.scope_id inner join IA_Collaboration.MOUPartnerSpecialization tb6
                        on tb6.mou_partner_id = tb2.mou_partner_id 
                        inner join General.Specialization tb7
                        on tb7.specialization_id = tb6.specialization_id
                        inner join 
                        (select max([datetime]) as 'maxdate',mou_status_id, mou_id
                        from IA_Collaboration.MOUStatusHistory 
                        group by mou_status_id, mou_id) tb8 on
                        tb8.mou_id = tb1.mou_id
                        inner join IA_Collaboration.MOUStatus tb9 on
                        tb9.mou_status_id = tb8.mou_status_id
                        inner join General.Office tb10 on
                        tb10.office_id = tb1.office_id
                        where tb1.is_deleted = 0 ";
                MOU mouBasicInfo = db.Database.SqlQuery<MOU>(sql_mouBasicInfo).First();
                //handlingMOUListData(mouList);
                return mouBasicInfo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void listAllExtraMOU()
        {
            handlingExMOUListData();
        }
        private void handlingExMOUListData()
        {
            return;
        }
        public void editMOUBasicInfo()
        {
            return;
        }
        public void addExtraMOU()
        {
            return;
        }
        public void editExtraMOU()
        {
            return;
        }
        public void deleteExtraMOU()
        {
            return;
        }
        public void getNewExtraMOUCode()
        {
            return;
        }
        public void getExMOUByCode()
        {
            return;
        }
        public void getMOUStatus()
        {
            return;
        }
        public void getExMOUInteUnit()
        {
            return;
        }
        public void getSpecialization()
        {
            return;
        }
        public void getCollabScope()
        {
            return;
        }
    }
}
