using System;
using JetBrains.Annotations;

[Serializable]
public class UserDto {
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    [CanBeNull]
    public UserDto Parent { get; set; }
    [CanBeNull]
    public UserDto Teacher { get; set; }
    public short AccountType { get; set; }
}