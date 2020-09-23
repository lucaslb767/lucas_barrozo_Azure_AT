using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Mapping.Context
{
    class ProjectContext : DbContext
    {
        public DbSet<Friend> Friends { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Brother> Brothers { get; set; }

        public static readonly ILoggerFactory loggerFactory = LoggerFactory.Create(a => { a.AddConsole(); });

        public ProjectContext(DbContextOptions<ProjectContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(loggerFactory);

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new FriendMap());
            modelBuilder.ApplyConfiguration(new CountryMap());
            modelBuilder.ApplyConfiguration(new StateMap());
            modelBuilder.ApplyConfiguration(new BrotherMap());

            base.OnModelCreating(modelBuilder);

        }
    }
}
