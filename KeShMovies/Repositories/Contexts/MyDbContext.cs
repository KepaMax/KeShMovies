using KeShMovies.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeShMovies.Repositories.Contexts;

public class MyDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var configurationBuilder = new ConfigurationBuilder();
        var path = Path.Combine(Directory.GetCurrentDirectory(), "appSettings.json");
        configurationBuilder.AddJsonFile(path, false);
        var root = configurationBuilder.Build();
        var sqlConnectionString = root.GetConnectionString("UsersDBConnectionStr");

        optionsBuilder.UseSqlServer(sqlConnectionString);

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<List<string>>().HasNoKey();
    }

    public DbSet<User>? Users { get; set; }
}
