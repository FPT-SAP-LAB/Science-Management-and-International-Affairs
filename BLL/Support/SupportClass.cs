using System.Linq;

namespace BLL.Support
{
    public class SupportClass
    {
        public static void TrimProperties(object obj)
        {
            var stringProperties = obj.GetType().GetProperties()
                          .Where(p => p.PropertyType == typeof(string));

            foreach (var stringProperty in stringProperties)
            {
                string currentValue = (string)stringProperty.GetValue(obj, null);
                string newValue = currentValue?.Trim();
                newValue = newValue == "" ? null : newValue;
                stringProperty.SetValue(obj, newValue, null);
            }
        }
    }
}
