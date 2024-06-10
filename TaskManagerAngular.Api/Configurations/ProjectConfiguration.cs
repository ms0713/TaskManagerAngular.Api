using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Data;
using TaskManagerAngular.Api.Models;
namespace TaskManagerAngular.Api.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable("projects");

        builder.HasKey(project => project.Id);

        //builder.HasMany(role => role.Users)
        //    .WithMany(user => user.Roles);

        //builder.HasData(Role.Registered);
    }
}
