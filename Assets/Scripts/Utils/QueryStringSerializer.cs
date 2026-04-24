using System.Reflection;
using System.Text;

public static class QueryStringSerializer {
    public static string Serialize(object obj) {
        if(obj == null) return string.Empty;
        
        StringBuilder result = new();

        PropertyInfo[] properties = obj.GetType().GetProperties();
        
        bool isFirst = true;
        foreach (PropertyInfo property in properties) {
            if(!isFirst) result.Append("&");
            else isFirst = false;
            
            result.Append($"{property.Name}={property.GetValue(obj)}");
        }

        return result.ToString();
    }
}