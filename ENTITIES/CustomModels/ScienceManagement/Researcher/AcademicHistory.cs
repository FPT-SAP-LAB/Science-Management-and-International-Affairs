namespace ENTITIES.CustomModels.ScienceManagement.Researcher
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
    public class AcadBiography
    {
        public int people_id { get; set; }
        public int acad_id { get; set; }
        public int rownum { get; set; }
        public string degree { get; set; }
        public string time { get; set; }
        public string place { get; set; }
    }
}
