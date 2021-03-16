using ENTITIES;
using ENTITIES.CustomModels.ScienceManagement.Invention;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ScienceManagement.Invention
{
    public class InventionRepo
    {
        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        public DetailInvention getDetail(string id)
        {
            DetailInvention item = new DetailInvention();
            string sql = @"select i.*, it.name as 'type_name', ri.reward_type, ri.total_reward
                            from [SM_ScientificProduct].Invention i join [SM_ScientificProduct].InventionType it on i.type_id = it.invention_type_id
	                            join [SM_ScientificProduct].RequestInvention ri on i.invention_id = ri.invention_id
                            where i.invention_id = @id";
            return item;
        }
    }
}
