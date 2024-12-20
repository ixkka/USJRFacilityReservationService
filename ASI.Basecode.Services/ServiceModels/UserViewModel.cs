﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.ServiceModels
{
    public class UserViewModel
    {
        [Required(ErrorMessage = "Username is required.")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirmation Password is required.")]
        [Compare("Password", ErrorMessage = "Password and Confirmation Password must match.")]


        public string ConfirmPassword { get; set; }


        public string CreatedBy { get; set; }
        public DateTime CreatedTime { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedTime { get; set; }
        public int UserTypeId { get; set; }
        public string Department { get; set; }
        public int Id { get; set; }

        // New property for profile picture upload
        [Display(Name = "Profile Picture")]
        public IFormFile? ProfilePicture { get; set; } // For file upload
        public string? ProfilePictureUrl { get; set; } // To store file path in the database
    }
}
