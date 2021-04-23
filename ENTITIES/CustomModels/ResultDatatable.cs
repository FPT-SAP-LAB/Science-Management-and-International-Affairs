using System.Collections.Generic;
using System.Web;

namespace ENTITIES.CustomModels
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
    public class ResultDatatable<T>
    {
        public List<T> data { get; set; }

        public string draw { get; set; }

        public int recordsTotal { get; set; }

        public int recordsFiltered { get; set; }

        public ResultDatatable()
        {
        }

        public ResultDatatable(BaseServerSideData<T> output, HttpRequestBase Request)
        {
            data = output.Data;
            draw = Request["draw"];
            recordsTotal = output.RecordsTotal;
            recordsFiltered = recordsTotal;
        }
    }
}
