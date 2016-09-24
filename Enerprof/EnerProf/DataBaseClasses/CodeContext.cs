using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using EnerProf.Models;
namespace EnerProf.DataBaseClasses
{
    public class CodeContext : DbContext
    {
        public CodeContext():base("connectionString")
        {
                
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Industries> Industries { get; set; }
        public DbSet<Clients> Clients { get; set; }
        public DbSet<AttributeModel> Attributes { get; set; }
        public DbSet<AttributesValues> AttributesValues { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasMany(c => c.Attributes)
                .WithMany(s => s.Categories)
                .Map(t => t.MapLeftKey("Cat_ID")
                .MapRightKey("Attr_ID")
                .ToTable("CategoriesAttributes"));
        }
    }
}