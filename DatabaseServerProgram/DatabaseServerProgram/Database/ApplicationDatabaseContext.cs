using DatabaseServerProgram.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatabaseServerProgram.Database;

public class ApplicationDatabaseContext : DbContext {
    public DbSet<User> Users { get; set; }
    public DbSet<Entities.Task> Tasks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        optionsBuilder.UseSqlite("Data Source=../../../Database/database.db");
    }
}