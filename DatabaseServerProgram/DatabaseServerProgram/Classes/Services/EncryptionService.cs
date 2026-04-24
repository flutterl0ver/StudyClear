namespace DatabaseServerProgram.Classes.Services;

public static class EncryptionService {
    public static string Encrypt(string passsword) {
        return BCrypt.Net.BCrypt.HashPassword(passsword);
    }

    public static bool Validate(string password, string hash) {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}