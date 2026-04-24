using System.Net;

namespace DatabaseServerProgram.DTO;

public class HttpRequest {
    public HttpListenerRequest ListenerRequest { get; set; }
    public Dictionary<string, string> RequestBody { get; set; } = new();
    public Dictionary<string, string> UrlVariables { get; set; } = new();
    public Dictionary<string, string> QueryString { get; set; } = new();

    public HttpRequest(HttpListenerRequest listenerRequest) {
        ListenerRequest = listenerRequest;
        RequestBody = ParseRequestBody(listenerRequest);
        QueryString = ParseQueryString(listenerRequest);
    }

    private Dictionary<string, string> ParseRequestBody(HttpListenerRequest listenerRequest) {
        if (!listenerRequest.HasEntityBody) return [];

        string body;
        using (StreamReader reader = new(listenerRequest.InputStream)) {
            body = reader.ReadToEnd();
        }

        return ParseNameValues(body);
    }

    private Dictionary<string, string> ParseQueryString(HttpListenerRequest listenerRequest) {
        if (listenerRequest.Url is null) return [];
        if (!listenerRequest.Url.ToString().Contains('?')) return [];

        return ParseNameValues(listenerRequest.Url.ToString().Split('?')[1]);
    }

    private Dictionary<string, string> ParseNameValues(string query) {
        Dictionary<string, string> dict = new();

        foreach (string pair in query.Split('&')) {
            string[] split = pair.Split('=');
            dict[split[0]] = split[1];
        }

        return dict;
    }
}