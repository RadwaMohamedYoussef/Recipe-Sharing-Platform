using Microsoft.EntityFrameworkCore;
using Recipe_Sharing_Platform.Entities;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Recipe_Sharing_Platform.Data
{
        public class ApplicationDbContext : DbContext
        {
            public ApplicationDbContext(DbContextOptions options) : base(options) { }

            public DbSet<User> Users { get; set; }
            public DbSet<Recipe> Recipes { get; set; }
            public DbSet<Entities.Label> Labels { get; set; }
            public DbSet<RecipeLabel> RecipeLabels { get; set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<RecipeLabel>()
                    .HasKey(rl => new { rl.RecipeId, rl.LabelId });
            }
        }

    }
