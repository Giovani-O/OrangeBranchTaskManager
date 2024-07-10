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

internal class AppDbContext : IdentityDbContext<IdentityUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    { }
    
    public DbSet<TaskModel> Tasks { get; set; }
    public new DbSet<UserModel> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
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
            .HasMaxLength(300)
            .IsRequired();

        modelBuilder.Entity<TaskModel>()
            .Property(x => x.DueDate)
            .IsRequired();
    }
}

