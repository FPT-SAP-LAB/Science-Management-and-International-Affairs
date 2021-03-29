using ENTITIES;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;

namespace BLL.InternationalCollaboration.AcademicActivity
{
    public class AcademicActivityPhaseRepo
    {
        ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        public List<infoPhase> getPhase(int language_id, int activity_id)
        {
            try
            {
                string sql = @"SELECT p.phase_id, al.phase_name, cast(FORMAT(p.phase_start , 'dd/MM/yyyy') as nvarchar) as 'from',
                                cast(FORMAT(p.phase_end , 'dd/MM/yyyy') as nvarchar) as 'to', a.full_name
                                FROM SMIA_AcademicActivity.AcademicActivityPhase p INNER JOIN
                                SMIA_AcademicActivity.AcademicActivityPhaseLanguage al ON
                                p.phase_id = al.phase_id inner join General.Account a
                                on p.created_by = a.account_id
                                WHERE al.language_id = @language_id AND p.activity_id = @activity_id";
                List<infoPhase> data = db.Database.SqlQuery<infoPhase>(sql,
                    new SqlParameter("language_id", language_id),
                    new SqlParameter("activity_id", activity_id)).ToList();
                return data;
            }
            catch (Exception e)
            {
                return new List<infoPhase>();
            }
        }
        public basePhase getDetailPhase(int language_id, int phase_id)
        {
            try
            {
                string sql = @"SELECT al.phase_name, cast(FORMAT(p.phase_start , 'dd/MM/yyyy') as nvarchar) as 'from',
                            cast(FORMAT(p.phase_end , 'dd/MM/yyyy') as nvarchar) as 'to'
                            FROM SMIA_AcademicActivity.AcademicActivityPhase p INNER JOIN
                            SMIA_AcademicActivity.AcademicActivityPhaseLanguage al ON
                            p.phase_id = al.phase_id
                            WHERE al.language_id = @language_id AND p.phase_id = @phase_id";
                basePhase data = db.Database.SqlQuery<basePhase>(sql, new SqlParameter("language_id", language_id),
                                new SqlParameter("phase_id", phase_id)).FirstOrDefault();
                return data;
            }
            catch (Exception e)
            {
                return new basePhase();
            }
        }
        public List<baseParticipantRole> getParticipantRoleByPhase(int phase_id)
        {
            try
            {
                string sql = @"SELECT pr.participant_role_id,pr.participant_role_name, pr.price
                                FROM SMIA_AcademicActivity.ParticipantRole pr
                                where pr.phase_id = @phase_id";
                List<baseParticipantRole> data = db.Database.SqlQuery<baseParticipantRole>(sql, new SqlParameter("phase_id", phase_id)).ToList();
                return data;
            }
            catch (Exception e)
            {
                return new List<baseParticipantRole>();
            }
        }
        public bool addPhase(int language_id, int activity_id, int account_id, basePhase data)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    AcademicActivityPhase phase = db.AcademicActivityPhases.Add(new AcademicActivityPhase
                    {
                        phase_start = DateTime.ParseExact(data.from, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        phase_end = DateTime.ParseExact(data.to, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        activity_id = activity_id,
                        created_by = account_id
                    });
                    db.SaveChanges();
                    db.AcademicActivityPhaseLanguages.Add(new AcademicActivityPhaseLanguage
                    {
                        phase_id = phase.phase_id,
                        language_id = language_id,
                        phase_name = data.phase_name
                    });
                    if (language_id == 1)
                    {
                        AcademicActivityPhaseLanguage aapl = db.AcademicActivityPhaseLanguages.Where(x => x.phase_id == phase.phase_id && x.language_id == 2).FirstOrDefault();
                        if (aapl == null)
                        {
                            db.AcademicActivityPhaseLanguages.Add(new AcademicActivityPhaseLanguage
                            {
                                phase_id = phase.phase_id,
                                language_id = 2,
                                phase_name = String.Empty
                            });
                        }
                    }
                    else
                    {
                        AcademicActivityPhaseLanguage aapl = db.AcademicActivityPhaseLanguages.Where(x => x.phase_id == phase.phase_id && x.language_id == 1).FirstOrDefault();
                        if (aapl == null)
                        {
                            db.AcademicActivityPhaseLanguages.Add(new AcademicActivityPhaseLanguage
                            {
                                phase_id = phase.phase_id,
                                language_id = 1,
                                phase_name = String.Empty
                            });
                        }
                    }
                    db.SaveChanges();
                    transaction.Commit();
                    return true;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }
        public bool deletePhase(int phase_id)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    AcademicActivityPhase aap = db.AcademicActivityPhases.Find(phase_id);
                    db.AcademicActivityPhases.Remove(aap);
                    db.SaveChanges();
                    transaction.Commit();
                    return true;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }
        public bool editPhase(int language_id, infoPhase data)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    AcademicActivityPhase aap = db.AcademicActivityPhases.Find(data.phase_id);
                    aap.phase_start = DateTime.ParseExact(data.from, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    aap.phase_end = DateTime.ParseExact(data.to, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    db.Entry(aap).State = EntityState.Modified;
                    AcademicActivityPhaseLanguage aapl = db.AcademicActivityPhaseLanguages.Where(x => x.language_id == language_id && x.phase_id == data.phase_id).FirstOrDefault();
                    aapl.phase_name = data.phase_name;
                    db.Entry(aapl).State = EntityState.Modified;
                    db.SaveChanges();
                    transaction.Commit();
                    return true;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }
        public List<PlanParticipant> getParticipantPlanByRole(int participant_role_id)
        {
            try
            {
                string sql = @"select pp.participant_role_id,pp.quantity,pp.office_id from SMIA_AcademicActivity.PlanParticipant pp
                                inner join SMIA_AcademicActivity.ParticipantRole pr on pr.participant_role_id = pp.participant_role_id
                                where pr.participant_role_id = @participant_role_id";
                List<PlanParticipant> data = db.Database.SqlQuery<PlanParticipant>(sql, new SqlParameter("participant_role_id", participant_role_id)).ToList();
                return data;
            }
            catch (Exception e)
            {
                return new List<PlanParticipant>();
            }
        }
        public List<baseOffice> getOffices()
        {
            try
            {
                string sql = @"select o.office_id,o.office_name from General.Office o ";
                List<baseOffice> data = db.Database.SqlQuery<baseOffice>(sql).ToList();
                return data;
            }
            catch (Exception e)
            {
                return new List<baseOffice>();
            }
        }
        public class basePhase
        {
            public string phase_name { get; set; }
            public string from { get; set; }
            public string to { get; set; }
        }
        public class infoPhase : basePhase
        {
            public int phase_id { get; set; }
            public string full_name { get; set; }
        }
        public class baseParticipantRole
        {
            public int participant_role_id { get; set; }
            public string participant_role_name { get; set; }
            public string price { get; set; }
        }
        public class baseOffice
        {
            public int office_id { get; set; }
            public string office_name { get; set; }
        }
        public class basePlanParticipant
        {
            public int office_id { get; set; }
            public string quantity { get; set; }
        }
    }
}