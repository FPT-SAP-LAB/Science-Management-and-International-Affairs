namespace ENTITIES.CustomModels
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
    public class AlertModal<T>
    {
        public bool success { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public T obj { get; set; }

        public AlertModal(T obj, bool success, string title, string content)
        {
            this.obj = obj;
            this.success = success;
            this.title = title;
            this.content = content;
        }

        public AlertModal(T obj, bool success, string content)
        {
            this.obj = obj;
            this.success = success;
            title = success ? "Thành công" : "Lỗi";
            this.content = content;
        }

        public AlertModal(T obj, bool success)
        {
            this.obj = obj;
            this.success = success;
            title = success ? "Thành công" : "Lỗi";
        }

        public AlertModal(bool success, string content)
        {
            this.success = success;
            title = success ? "Thành công" : "Lỗi";
            this.content = content;
        }

        public AlertModal(bool success)
        {
            this.success = success;
            title = success ? "Thành công" : "Lỗi";
            if (!success) content = "Có lỗi xảy ra";
        }
    }
}