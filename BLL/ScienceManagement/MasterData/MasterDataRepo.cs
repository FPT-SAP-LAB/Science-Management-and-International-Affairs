using ENTITIES;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ScienceManagement.MasterData
{
    public class MasterDataRepo
    {
        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        public List<SpecializationLanguage> getSpec(string language)
        {
            List<SpecializationLanguage> list = new List<SpecializationLanguage>();
            string sql = @"select sl.name, sl.specialization_id, sl.language_id
                            from [General].Specialization s join [Localization].SpecializationLanguage sl on s.specialization_id = sl.specialization_id
	                            join [Localization].Language l on sl.language_id = l.language_id
                            where l.language_name = @lang";
            list = db.Database.SqlQuery<SpecializationLanguage>(sql, new SqlParameter("lang", language)).ToList();
            return list;
        }

        public List<PaperCriteria> getPaperCriteria()
        {
            List<PaperCriteria> list = new List<PaperCriteria>();
            string sql = @"select c.*
                            from [SM_ScientificProduct].PaperCriteria c
                            where c.status = 'active'";
            list = db.Database.SqlQuery<PaperCriteria>(sql).ToList();
            return list;
        }
    }
}
