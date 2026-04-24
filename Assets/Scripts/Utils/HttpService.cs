using System.Net.Http;
using System.Threading.Tasks;
using JetBrains.Annotations;

public static class HttpService {
    private static string _urlBase = "http://localhost:13000";

    public static string SendPostRequest(string url, [CanBeNull] object body = null) {
        string bodyString = QueryStringSerializer.Serialize(body);

        if (!url.StartsWith('/')) url = '/' + url;
        
        HttpClient client = new();
        HttpResponseMessage response = client.PostAsync(_urlBase + url, new StringContent(bodyString)).Result;
        return response.Content.ReadAsStringAsync().Result;
    }

    public static async Task<string> SendPostRequestAsync(string url, [CanBeNull] object body = null) {
        return await Task.Run(() => SendPostRequest(url, body));
    }
    
    public static string SendGetRequest(string url, [CanBeNull] object query = null) {
        string queryString = QueryStringSerializer.Serialize(query);

        if (!url.StartsWith('/')) url = '/' + url;
        
        if(query != null) url += '?' + queryString;
        
        HttpClient client = new();
        HttpResponseMessage response = client.GetAsync(_urlBase + url).Result;
        return response.Content.ReadAsStringAsync().Result;
    }
}