using DatabaseServerProgram.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatabaseServerProgram.Database;

public class ApplicationDatabaseContext : DbContext {
    public DbSet<User> Users { get; set; }
    public DbSet<Entities.Task> Tasks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        optionsBuilder.UseSqlite("Data Source=../../../Database/database.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<User>()
            .HasIndex(b => b.Username)
            .IsUnique();

        modelBuilder.Entity<User>().HasOne(u => u.Teacher).WithMany();
    }
}