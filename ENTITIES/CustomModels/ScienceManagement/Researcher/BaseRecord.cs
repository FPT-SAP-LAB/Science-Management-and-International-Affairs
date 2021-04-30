namespace ENTITIES.CustomModels.ScienceManagement.Researcher
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
    public class BaseRecord<T>
    {
        public int index { get; set; }
        public T records { get; set; }
    }
}
