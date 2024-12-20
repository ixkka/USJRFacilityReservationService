﻿using ASI.Basecode.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(ModelBuilder modelBuilder) {
            
            SeedUserType(modelBuilder);
            SeedUsers(modelBuilder);
            SeedFacility(modelBuilder);

        }
        public static void SeedUsers(ModelBuilder modelBuilder) {
            modelBuilder.Entity<User>().HasData(
                new User { 
                    Id = 1, 
                    UserId = "NewAdminSeed", 
                    Name = "NewAdmin", 
                    Password = "USJR456", 
                    CreatedBy = System.Environment.UserName, 
                    UpdatedBy = System.Environment.UserName, 
                    CreatedTime = DateTime.Now, 
                    UpdatedTime = DateTime.Now,
                    Department = "SCS",
                    UserTypeId = 2
                }
            );
        }

        public static void SeedUserType(ModelBuilder modelBuilder) {

            modelBuilder.Entity<UserType>().HasData(
                new UserType
                {
                    UserTypeId = 1,
                    UserTypeName = "Admin"
                }, new UserType
                {
                    UserTypeId = 2,
                    UserTypeName = "Student"
                }, new UserType
                {
                    UserTypeId = 3,
                    UserTypeName = "SuperAdmin"
                }

                );
        }

        public static void SeedFacility(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Facility>().HasData(
                new Facility
                {
                    FacilityId = 1,
                    FacilityName = "Main Conference Room",
                    Thumbnail = "main_conference_thumb.jpg",
                    Capacity = 100,
                    Description = "A large conference room equipped with modern facilities.",
                    Location = "Building A, 1st Floor",
                    CreatedBy = "AdminUser",
                    CreatedDt = DateTime.Now,
                    UpdatedBy = "AdminUser",
                    UpdatedDt = DateTime.Now,
                    Amenity = "Projector"
                }

                );
        }
    }
}
