using System.Collections.Generic;

namespace ENTITIES.CustomModels
{
    public class Select2Ajax<T>
    {
        public List<T> Results { get; set; }
        public int CountFiltered { get; set; }
        public Select2Ajax()
        {
        }
        public Select2Ajax(List<T> results, int countFiltered)
        {
            Results = results;
            CountFiltered = countFiltered;
        }
    }
}
