using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels
{
    public class BaseServerSideData<T>
    {
        public List<T> Data { get; set; }
        public int RecordsTotal { get; set; }
        public BaseServerSideData(List<T> data, int recordsTotal)
        {
            Data = data;
            RecordsTotal = recordsTotal;
        }
    }
}
