using DatabaseServerProgram.Database;
using DatabaseServerProgram.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatabaseServerProgram.Classes.Providers;

public static class UsersProvider {
    public static bool UserExists(string username) {
        return GetUserByUsername(username) is not null;
    }

    public static User? GetUserByUsername(string username) {
        using ApplicationDatabaseContext context = new();
        
        return context.Users
            .Include(u => u.Parent)
            .Include(u => u.Teacher)
            .FirstOrDefault(u => u.Username == username);
    }
    
    public static void CreateUser(User user) {
        using ApplicationDatabaseContext context = new();
        
        context.Users.Add(user);
        context.SaveChanges();
    }

    public static void SetParent(User user, User parent) {
        using ApplicationDatabaseContext context = new();
        
        context.Users.Attach(user)
            .Property("ParentId").CurrentValue = parent.Id;
        context.SaveChanges();
    }
    
    public static void SetTeacher(User user, User parent) {
        using ApplicationDatabaseContext context = new();
        
        context.Users.Attach(user)
            .Property("TeacherId").CurrentValue = parent.Id;
        context.SaveChanges();
    }

    public static List<User> GetChildren(User user) {
        using ApplicationDatabaseContext context = new();
        
        return context.Users.Where(u => u.Parent == user || u.Teacher == user).ToList();
    }
}