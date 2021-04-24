using System.Collections.Generic;

namespace ENTITIES.CustomModels.Datatable
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
