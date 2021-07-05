using HeroesApi.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HeroesApi.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public virtual DbSet<Hero> Heroes { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Hero>().Property(h => h.CurrentPower).HasColumnType("DECIMAL(10,2)");
            modelBuilder.Entity<Hero>().Property(h => h.StartingPower).HasColumnType("DECIMAL(10,2)");
            modelBuilder.Entity<Hero>().Ignore(h => h.NumTrainingToday);
        }
    }
}
