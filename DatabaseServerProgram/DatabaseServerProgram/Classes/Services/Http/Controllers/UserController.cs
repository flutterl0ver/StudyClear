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
        short accountType = short.Parse(request.RequestBody["accountType"]);

        User? createdUser = null;
        string error = string.Empty;

        if (UsersProvider.UserExists(username)) {
            error = "UserExists";
        } else {
            createdUser = new User(username, password, name, surname, accountType);
            UsersProvider.CreateUser(createdUser);
        }

        return new { error, user = createdUser };
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

        return new { error, user = foundUser };
    }

    public object SetParent(HttpRequest request) {
        string username = request.UrlVariables["user"];
        string parentUsername = request.RequestBody["parentUsername"];

        User? user = UsersProvider.GetUserByUsername(username);
        if (user is null) return new { error = "UserNotFound" };
        if (user.AccountType != 0) return new { error = "WrongUserAccountType" };

        User? parentUser = UsersProvider.GetUserByUsername(parentUsername);
        if (parentUser is null) return new { error = "ParentUserNotFound" };
        if (parentUser.AccountType != 1) return new { error = "WrongParentAccountType" };

        UsersProvider.SetParent(user, parentUser);
        return new { error = string.Empty, parentUser };
    }
    
    public object SetTeacher(HttpRequest request) {
        string username = request.UrlVariables["user"];
        string teacherUsername = request.RequestBody["teacherUsername"];

        User? user = UsersProvider.GetUserByUsername(username);
        if (user is null) return new { error = "UserNotFound" };
        if (user.AccountType != 0) return new { error = "WrongUserAccountType" };

        User? teacherUser = UsersProvider.GetUserByUsername(teacherUsername);
        if (teacherUser is null) return new { error = "TeacherUserNotFound" };
        if (teacherUser.AccountType != 2) return new { error = "WrongTeacherAccountType" };

        UsersProvider.SetTeacher(user, teacherUser);
        return new { error = string.Empty, teacherUser };
    }

    public object GetChildren(HttpRequest request) {
        string username = request.UrlVariables["user"];

        User? foundUser = UsersProvider.GetUserByUsername(username);
        if (foundUser == null) return new { error = "UserNotFound" };

        List<User> children = UsersProvider.GetChildren(foundUser);
        
        return new {
            error = string.Empty,
            users = children
        };
    }
}