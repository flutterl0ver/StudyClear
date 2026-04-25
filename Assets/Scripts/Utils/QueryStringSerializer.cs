using System.Reflection;
using System.Text;
using System.Text.Json;
using UnityEngine;

public static class QueryStringSerializer {
    public static string Serialize(object obj) {
        if(obj == null) return string.Empty;
        
        StringBuilder result = new();

        PropertyInfo[] properties = obj.GetType().GetProperties();
        
        bool isFirst = true;
        foreach (PropertyInfo property in properties) {
            if(!isFirst) result.Append("&");
            else isFirst = false;

            string value;
            if (property.PropertyType.IsClass && property.PropertyType != typeof(string)) {
                value = JsonSerializer.Serialize(property.GetValue(obj), property.PropertyType);
            }
            else value = property.GetValue(obj)?.ToString();
            
            result.Append($"{property.Name}={value}");
        }

        return result.ToString();
    }
}