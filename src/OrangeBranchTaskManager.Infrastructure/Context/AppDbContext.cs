/*
    Create and run migration 
 
    dotnet ef migrations add AddInitialMigration --startup-project .\OrangeBranchTaskManager.Api\ --project .\OrangeBranchTaskManager.Infrastructure\
    dotnet ef database update --startup-project .\OrangeBranchTaskManager.Api\ --project .\OrangeBranchTaskManager.Infrastructure\
 */

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OrangeBranchTaskManager.Domain.Entities;

namespace OrangeBranchTaskManager.Infrastructure.Context;

public class AppDbContext : IdentityDbContext<IdentityUser>
{
    // Passa options para o construtor de DbContext
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    { }

    // Define a tabela no banco de dados
    public virtual DbSet<TaskModel> Tasks { get; set; }
    public virtual DbSet<UserModel> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configura Id como primary key de Tasks
        modelBuilder.Entity<TaskModel>(entity =>
        {
            entity.HasKey(k => k.Id);
        });

        modelBuilder.Entity<TaskModel>()
            .Property(x => x.Title)
            .HasMaxLength(100)
            .IsRequired();

        modelBuilder.Entity<TaskModel>()
            .Property(x => x.Description)
            .HasMaxLength(400)
            .IsRequired();

        modelBuilder.Entity<TaskModel>()
            .Property(x => x.DueDate)
            .IsRequired();
    }
}

