using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ASI.Basecode.Data.Models;

namespace ASI.Basecode.Data
{
    public partial class AsiBasecodeDBContext : DbContext
    {
        public AsiBasecodeDBContext()
        {
        }

        public AsiBasecodeDBContext(DbContextOptions<AsiBasecodeDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<User> Users {get; set;}
        public virtual DbSet<UserType> UserType { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           // Set default values for Department and UserTypeId
            modelBuilder.Entity<User>()
                .Property(u => u.Department)
                .HasDefaultValue("USJRSCS");

            modelBuilder.Entity<User>()
                .Property(u => u.UserTypeId)
                .HasDefaultValue(2); // Ensure that UserTypeId defaults to a valid existing UserType

            // Define the relationship between User and UserType
            modelBuilder.Entity<User>()
                .HasOne(u => u.UserType)
                .WithMany(ut => ut.Users)
                .HasForeignKey(u => u.UserTypeId);


            ModelBuilderExtensions.Seed(modelBuilder);
            base.OnModelCreating(modelBuilder);

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
