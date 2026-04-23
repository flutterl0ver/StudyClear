namespace DatabaseServerProgram.Database.Entities;

public class User {
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    
    public User() {}

    public User(string username, string password, string name, string surname) {
        Username = username;
        Password = password;
        Name = name;
        Surname = surname;
    }
}