using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.ServiceModels
{
    public class ImageViewModel
    {
        [Required(ErrorMessage = "Please select an image.")]
        [Display(Name = "Image")]
        public IFormFile ImageUpload { get; set; }
    }
}