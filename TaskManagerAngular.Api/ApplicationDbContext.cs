﻿using Microsoft.EntityFrameworkCore;

namespace TaskManagerAngular.Api;

public sealed class ApplicationDbContext : DbContext
{

    public ApplicationDbContext(
        DbContextOptions options)
        :base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DependencyInjection).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}