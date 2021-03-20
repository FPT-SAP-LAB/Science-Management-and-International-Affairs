namespace ENTITIES.CustomModels.InternationalCollaboration.AcademicCollaborationEntities
{
    public class StatusType
    {
        public int status_type { get; set; }
        public string status_type_name { get; set; }

        public StatusType(int status_type, string status_type_name)
        {
            this.status_type = status_type;
            this.status_type_name = status_type_name;
        }
    }
}