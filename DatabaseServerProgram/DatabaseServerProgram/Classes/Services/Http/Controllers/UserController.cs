using System.Text;
using System.Text.Json;
using DatabaseServerProgram.Classes.Providers;
using DatabaseServerProgram.Database.Entities;
using DatabaseServerProgram.DTO;
using DatabaseServerProgram.Interfaces;

namespace DatabaseServerProgram.Classes.Services.Http.Controllers;

public class UserController : IHttpController {
    public object HandleRequest(HttpRequest request) {
        throw new NotImplementedException();
    }

    public object Register(HttpRequest request) {
        string username = request.RequestBody["username"];
        string password = request.RequestBody["password"];
        string name = request.RequestBody["name"];
        string surname = request.RequestBody["surname"];
        
        User? createdUser = null;
        string error = string.Empty;

        if (UsersProvider.UserExists(username)) {
            error = "UserExists";
        } else {
            createdUser = new User(username, password, name, surname);
            UsersProvider.CreateUser(createdUser);
        }

        return new {
            error,
            user = createdUser
        };
    }

    public object Login(HttpRequest request) {
        string username = request.RequestBody["username"];
        string password = request.RequestBody["password"];

        User? foundUser = UsersProvider.GetUserByUsername(username);
        string error = string.Empty;
        
        if (foundUser == null || !EncryptionService.Validate(password, foundUser.Password)) {
            foundUser = null;
            error = "LoginFailed";
        }

        return new {
            error,
            user = foundUser
        };
    }
}