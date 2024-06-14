using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskManagerAngular.Api.Identity;
using TaskManagerAngular.Api.Models;

namespace TaskManagerAngular.Api;

public sealed class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{

    public ApplicationDbContext(
        DbContextOptions options)
        :base(options)
    {
    }
    public DbSet<Project> Projects { get; set; }
    public DbSet<ApplicationRole> ApplicationRoles { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DependencyInjection).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}