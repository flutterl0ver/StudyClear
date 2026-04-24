using System.Text.Json;
using DatabaseServerProgram.DTO;
using DatabaseServerProgram.Interfaces;

namespace DatabaseServerProgram.Classes.Services.Http.Controllers;

public class AiController : IHttpController {
    private const string MODEL = "gemma3:4b";
    
    public object HandleRequest(HttpRequest request) {
        throw new NotImplementedException();
    }
    
    public object SendMessage(HttpRequest request) {
        string prompt = request.RequestBody["prompt"];
        string systemPrompt = request.RequestBody.GetValueOrDefault("systemPrompt", string.Empty);
        
        HttpClient client = new();
        HttpResponseMessage response = client.PostAsync("http://localhost:11434/api/generate", 
            new StringContent(JsonSerializer.Serialize(new {
                model = MODEL,
                prompt = systemPrompt + '\n' + prompt,
                stream = false
            }))).Result;
        
        return response.Content.ReadAsStringAsync().Result;
    }
}