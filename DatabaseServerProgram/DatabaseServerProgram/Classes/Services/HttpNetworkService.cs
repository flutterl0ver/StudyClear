using DatabaseServerProgram.DTO;
using DatabaseServerProgram.Interfaces;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace DatabaseServerProgram.Classes.Services;

public class HttpNetworkService {
    public delegate object HttpRequestHandler(HttpRequest request);

    private HttpListener _httpListener;
    private bool _isListening;
    private List<HttpPath> _paths = new();
    private IHttpController? _notFoundController;

    public HttpNetworkService() {
        _httpListener = new HttpListener();

        _httpListener.Prefixes.Add("http://*:13000/");
        _httpListener.Prefixes.Add("http://+:13000/");
    }

    public void Post<T>(string url, string? controllerMethod = null) where T : IHttpController, new() {
        RegisterPath<T>(url.Trim('/'), "POST", controllerMethod);
    }

    public void Get<T>(string url, string? controllerMethod = null) where T : IHttpController, new() {
        RegisterPath<T>(url.Trim('/'), "GET", controllerMethod);
    }

    public void SetNotFoundHandler<T>() where T : IHttpController, new() {
        _notFoundController = new T();
    }

    private void RegisterPath<T>(string url, string method, string? controllerMethod) where T : IHttpController, new() {
        _paths.Add(new HttpPath(url, method, controllerMethod ?? "HandleRequest", new T()));
    }

    public async Task StartListeningAsync() {
        _httpListener.Start();
        _isListening = true;
        LoggingService.Log("Сервер запущен.\n", ConsoleColor.Green);

        while (_isListening) {
            try {
                HttpListenerContext context = await _httpListener.GetContextAsync();
                HandleRequest(context);
            } catch (Exception e) {
                LoggingService.Log(e.ToString(), ConsoleColor.Red);
            }
        }
    }

    public void StopListening() {
        _isListening = false;
        _httpListener.Stop();
    }

    private void HandleRequest(HttpListenerContext context) {
        string url = (context.Request.RawUrl ?? string.Empty).Trim('/').Split('?')[0];
        LoggingService.Log($"{context.Request.RemoteEndPoint}: {context.Request.HttpMethod} {context.Request.Url}");

        object? responseData = null;
        bool validUrl = false;
        HttpRequest httpRequest = new(context.Request);

        foreach (HttpPath path in _paths) {
            if (path.Method == context.Request.HttpMethod) {
                Dictionary<string, string>? urlVariables = ValidateUrl(url, path.Path);
                if (urlVariables is null) continue;

                httpRequest.UrlVariables = urlVariables;
                MethodInfo handleMethod = path.Controller.GetType().GetMethod(path.ControllerMethod)!;
                HttpRequestHandler handleDelegate = handleMethod.CreateDelegate<HttpRequestHandler>(path.Controller);
                responseData = handleDelegate.Invoke(httpRequest);

                validUrl = true;
                break;
            }
        }

        HttpListenerResponse response = context.Response;

        if (!validUrl) {
            response.StatusCode = (int)HttpStatusCode.NotFound;
            if (_notFoundController is not null) {
                responseData = _notFoundController.HandleRequest(httpRequest);
            }
        }

        string responseText;
        if (responseData is string) {
            response.ContentType = "text/html";
            responseText = (responseData as string)!;
        } else {
            response.ContentType = "application/json";
            responseText = JsonSerializer.Serialize(responseData);
        }

        response.ContentEncoding = Encoding.UTF8;
        Console.WriteLine(responseText);

        byte[] bytes = response.ContentEncoding.GetBytes(responseText);

        response.Close(bytes, true);
    }

    private Dictionary<string, string>? ValidateUrl(string url, string path) {
        string[] urlSplit = url.Split('/');
        string[] pathSplit = path.Split('/');

        if (urlSplit.Length != pathSplit.Length) return null;

        Dictionary<string, string> result = new();
        for (int i = 0; i < urlSplit.Length; i++) {
            if (pathSplit[i].StartsWith('{') && pathSplit[i].EndsWith('}')) {
                result[pathSplit[i].Substring(1, pathSplit[i].Length - 2)] = urlSplit[i];
            } else if (pathSplit[i] != urlSplit[i]) return null;
        }

        return result;
    }

    private class HttpPath {
        public string Path;
        public string Method;
        public string ControllerMethod;
        public IHttpController Controller;

        public HttpPath(string path, string method, string controllerMethod, IHttpController controller) {
            Path = path;
            Method = method;
            ControllerMethod = controllerMethod;
            Controller = controller;
        }
    }
}