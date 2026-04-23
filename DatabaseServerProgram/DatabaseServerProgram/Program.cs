using System.Net;
using System.Text;
using System.Text.Json;
using DatabaseServerProgram.Classes.Providers;
using DatabaseServerProgram.Database.Entities;

namespace DatabaseServerProgram;

class Program {
    static void Main(string[] args) {
        HttpListener listener = new();
        listener.Prefixes.Add("http://*:8080/");
        listener.Prefixes.Add("http://+:8080/");

        listener.Start();
        Console.WriteLine("Сервер запущен.\n");

        while (true) {
            try {
                HttpListenerContext context = listener.GetContext();
                Console.WriteLine($"Запрос на {context.Request.Url}");

                if (context.Request.Url.AbsolutePath.StartsWith("/register")) {
                    Dictionary<string, JsonElement> body = ParseBody(context.Request.InputStream);
                    string error = string.Empty;
                    User? createdUser = null;

                    string username = body["username"].GetString();
                    string password = body["password"].GetString();
                    string name = body["name"].GetString();
                    string surname = body["surname"].GetString();

                    if (UsersProvider.UserExists(username)) {
                        error = "UserExists";
                    } else {
                        createdUser = new User(username, password, name, surname);
                        UsersProvider.CreateUser(createdUser);
                    }

                    string response = JsonSerializer.Serialize(new { error = error, user = createdUser });

                    byte[] buffer = Encoding.UTF8.GetBytes(response);
                    using (Stream output = context.Response.OutputStream) {
                        output.Write(buffer, 0, buffer.Length);
                    }
                }
                
                if (context.Request.Url.AbsolutePath.StartsWith("/login")) {
                    Dictionary<string, JsonElement> body = ParseBody(context.Request.InputStream);
                    string error = string.Empty;
                    User? foundUser = null;

                    string username = body["username"].GetString();
                    string password = body["password"].GetString();

                    if (UsersProvider.UserExists(username)) {
                        foundUser = UsersProvider.GetUserByUsername(username);
                        if (!BCrypt.Net.BCrypt.Verify(password, foundUser.Password)) {
                            foundUser = null;
                            error = "LoginFailed";
                        }
                    } else {
                        error = "LoginFailed";
                    }

                    string response = JsonSerializer.Serialize(new { error = error, user = foundUser });

                    byte[] buffer = Encoding.UTF8.GetBytes(response);
                    using (Stream output = context.Response.OutputStream) {
                        output.Write(buffer, 0, buffer.Length);
                    }
                }
            } catch (Exception e) {
                Console.Error.WriteLine(e);
            }
        }
    }

    static Dictionary<string, JsonElement> ParseBody(Stream stream) {
        using (StreamReader reader = new(stream)) {
            string postData = reader.ReadToEnd();
            return JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(postData);
        }
    }
}