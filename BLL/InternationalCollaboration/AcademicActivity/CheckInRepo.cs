using ENTITIES;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;

namespace BLL.InternationalCollaboration.AcademicActivity
{
    public class CheckInRepo
    {
        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        public List<dataParticipant> getParticipantByPhase(int phase_id)
        {
            try
            {
                string sql = @"SELECT ROW_NUMBER() OVER (ORDER BY p.participant_id) as 'stt', CONCAT(p.participant_id,'$',p.is_checked) as 'participant_id', pr.participant_role_name,p.participant_name, p.email, u.unit_name, a.[name], p.participant_number, p.is_checked
                                FROM SMIA_AcademicActivity.ParticipantRole pr inner join
                                SMIA_AcademicActivity.Participant p on pr.participant_role_id = p.participant_role_id left join General.Office o 
                                on o.office_id = p.office_id left join General.InternalUnit u on u.unit_id = o.unit_id
                                left join General.Area a on o.area_id = a.area_id where pr.phase_id = @phase_id";
                List<dataParticipant> data = db.Database.SqlQuery<dataParticipant>(sql, new SqlParameter("phase_id", phase_id)).ToList();
                return data;
            }
            catch (Exception e)
            {
                return new List<dataParticipant>();
            }
        }
        public List<Phase> getPhase(int activity_id)
        {
            try
            {
                string sql = @"SELECT al.phase_id,al.phase_name 
                                FROM SMIA_AcademicActivity.AcademicActivityPhase p 
                                INNER JOIN SMIA_AcademicActivity.AcademicActivityPhaseLanguage al
                                ON p.phase_id = al.phase_id
                                where p.activity_id = @activity_id AND  al.language_id = 1";
                List<Phase> data = db.Database.SqlQuery<Phase>(sql, new SqlParameter("activity_id", activity_id)).ToList();
                return data;
            }
            catch (Exception e)
            {
                return new List<Phase>();
            }
        }
        public bool Checkin(int participant_id)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    Participant p = db.Participants.Where(x => x.participant_id == participant_id).FirstOrDefault();
                    p.is_checked = true;
                    db.Entry(p).State = EntityState.Modified;
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
        public bool Checkout(int participant_id)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    Participant p = db.Participants.Where(x => x.participant_id == participant_id).FirstOrDefault();
                    p.is_checked = false;
                    db.Entry(p).State = EntityState.Modified;
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
        public List<PartiRole> GetParticipantRolesByPhase(int phase_id)
        {
            try
            {
                string sql = @"select pr.participant_role_id,pr.participant_role_name from SMIA_AcademicActivity.ParticipantRole pr where pr.phase_id = @phase_id";
                List<PartiRole> data = db.Database.SqlQuery<PartiRole>(sql, new SqlParameter("phase_id", phase_id)).ToList();
                return data;
            }
            catch (Exception e)
            {
                return new List<PartiRole>();
            }
        }
        public List<Unit> GetInternalUnit()
        {
            try
            {
                string sql = @"select iu.unit_id,iu.unit_name from General.InternalUnit iu";
                List<Unit> data = db.Database.SqlQuery<Unit>(sql).ToList();
                return data;
            }
            catch (Exception e)
            {
                return new List<Unit>();
            }
        }
        public List<Area> getAreaByUnit(int unit_id)
        {
            try
            {
                string sql = @"select o.office_id,o.office_name from General.Office o, General.Area a where a.area_id = o.area_id and o.unit_id = @unit_id";
                List<Area> data = db.Database.SqlQuery<Area>(sql, new SqlParameter("unit_id", unit_id)).ToList();
                return data;
            }
            catch (Exception e)
            {
                return new List<Area>();
            }
        }
        public bool addParticipant(infoParticipant obj)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    db.Participants.Add(new Participant
                    {
                        participant_role_id = obj.participant_role_id,
                        participant_name = obj.name,
                        email = obj.email,
                        participant_number = obj.participant_number,
                        office_id = obj.office_id,
                        is_checked = false
                    });
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
        public class dataParticipant
        {
            public long stt { get; set; }
            public string participant_id { get; set; }
            public string participant_role_name { get; set; }
            public string participant_name { get; set; }
            public string email { get; set; }
            public string unit_name { get; set; }
            public string name { get; set; }
            public string participant_number { get; set; }
            public bool is_checked { get; set; }
        }
        public class Phase
        {
            public int phase_id { get; set; }
            public string phase_name { get; set; }
        }
        public class PartiRole
        {
            public int participant_role_id { get; set; }
            public string participant_role_name { get; set; }
        }
        public class Unit
        {
            public int unit_id { get; set; }
            public string unit_name { get; set; }
        }
        public class Area
        {
            public int office_id { get; set; }
            public string office_name { get; set; }
        }
        public class infoParticipant
        {
            public string name { get; set; }
            public int participant_role_id { get; set; }
            public string email { get; set; }
            public string participant_number { get; set; }
            public int office_id { get; set; }
        }
    }
}