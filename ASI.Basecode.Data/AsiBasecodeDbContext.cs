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
        public virtual DbSet<Facility> Facility { get; set; }
        public DbSet<BookingPreference> BookingPreferences { get; set; }
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

            modelBuilder.Entity<DayOfTheWeek>(entity =>
            {
                entity.HasKey(e => e.DayOfWeekId)
                    .HasName("PK__DayOfThe__01AA8DDF270B0DA0");

                entity.ToTable("DayOfTheWeek");

                entity.Property(e => e.DayOfWeekId).HasColumnName("DayOfWeekID");

                entity.Property(e => e.DayName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ImageGallery>(entity =>
            {
                entity.HasKey(e => e.ImageId)
                    .HasName("PK__ImageGal__94940404");

                entity.ToTable("ImageGallery");

                entity.Property(e => e.ImageId).HasColumnName("ImageID");

                entity.Property(e => e.ImageName).IsUnicode(false);

                entity.Property(e => e.Path).IsUnicode(false);

                entity.Property(e => e.FacilityId).HasColumnName("FacilityID");

                entity.HasOne(d => d.Facility)
                    .WithMany(p => p.ImageGalleries)
                    .HasForeignKey(d => d.FacilityId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__ImageGall__FacilityI__4CA06362");
            });

            modelBuilder.Entity<Facility>(entity =>
            {
                entity.ToTable("Facility");

                entity.HasIndex(e => e.FacilityName, "FAC_00_000000")
                    .IsUnique();

                entity.Property(e => e.FacilityId).HasColumnName("FacilityID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDt)
                    .HasColumnType("datetime")
                    .HasColumnName("CreatedDT");

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.Amenity).IsUnicode(false);

                entity.Property(e => e.Location)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FacilityName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Thumbnail)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedDt)
                    .HasColumnType("datetime")
                    .HasColumnName("UpdatedDT");

                entity.Property(e => e.Confirmation).IsRequired().HasDefaultValue(1);
            });

            modelBuilder.Entity<BookingPreference>(entity =>
            {
                entity.ToTable("BookingPreference");

                entity.HasKey(e => e.BookingPreferenceId);

                entity.Property(e => e.BookingPreferenceId)
                    .HasColumnName("BookingPreferenceID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.UserId)
                    .IsRequired(false); 

                entity.Property(e => e.SingleBookingStartTime)
                    .HasColumnType("time")
                    .IsRequired(false);

                entity.Property(e => e.SingleBookingEndTime)
                    .HasColumnType("time")
                    .IsRequired(false);

                entity.Property(e => e.SingleBookingNotes)
                    .HasMaxLength(500)
                    .IsUnicode(false); 

                entity.Property(e => e.RecurrentBookingStartTime)
                    .HasColumnType("time")
                    .IsRequired(false);

                entity.Property(e => e.RecurrentBookingEndTime)
                    .HasColumnType("time")
                    .IsRequired(false);

                entity.Property(e => e.RecurrentBookingDays)
                    .HasMaxLength(50)
                    .IsUnicode(false); 

                entity.Property(e => e.RecurrentBookingNotes)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                // Define the foreign key relationship with the User entity
                //entity.HasOne(d => d.User)
                //    .WithMany(p => p.BookingPreferences) // One User can have many BookingPreferences
                //    .HasForeignKey(d => d.UserId) // Foreign key for UserId
                //    .OnDelete(DeleteBehavior.Cascade) // If User is deleted, delete associated BookingPreferences
                //    .HasConstraintName("FK_BookingPreference_User"); // Specify constraint name
            });


            ModelBuilderExtensions.Seed(modelBuilder);
            base.OnModelCreating(modelBuilder);

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
