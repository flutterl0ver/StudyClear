using DatabaseServerProgram.Database;
using DatabaseServerProgram.Database.Entities;

namespace DatabaseServerProgram.Classes.Providers;

public static class UsersProvider {
    public static bool UserExists(string username) {
        return GetUserByUsername(username) is not null;
    }

    public static User? GetUserByUsername(string username) {
        using (ApplicationDatabaseContext context = new()) {
            return context.Users.FirstOrDefault(u => u.Username == username);
        }
    }
    
    public static void CreateUser(User user) {
        using (ApplicationDatabaseContext context = new()) {
            context.Users.Add(user);
            context.SaveChanges();
        }
    }
}