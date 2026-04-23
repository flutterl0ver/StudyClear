using DatabaseServerProgram.Database;
using DatabaseServerProgram.Database.Entities;

namespace DatabaseServerProgram.Classes.Providers;

public static class UsersProvider {
    public static bool UserExists(string username) {
        using (ApplicationDatabaseContext context = new()) {
            return context.Users.Any(u => u.Username == username);
        }
    }

    public static void CreateUser(User user) {
        using (ApplicationDatabaseContext context = new()) {
            context.Users.Add(user);
            context.SaveChanges();
        }
    }
}