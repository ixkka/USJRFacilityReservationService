using ASI.Basecode.Data.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.ServiceModels
{
    public class FacilityViewModel
    {
        [Display(Name = "Facility ID")]
        public int FacilityId { get; set; }

        [Display(Name = "Facility Name")]
        public string FacilityName { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Display(Name = "Facility Location")]
        public string Location { get; set; }
        [Display(Name = "Capacity")]
        public int Capacity { get; set; }
        [Display(Name = "Amenities")]
        public string Amenity { get; set; }
        public IFormFileCollection FacilityGalleryImg { get; set; }

        [Display(Name = "Thumbnail Photo")]
        public IFormFile FacilityThumbnailImg { get; set; }
        public string Thumbnail { get; set; }
        public List<RoomGalleryViewModel> _RoomGallery { get; set; }
        public IEnumerable<FacilityViewModel> facilityList { get; set; }
        public BookingViewModel BookingViewModel { get; set; }
        public int BookingDuration { get; set; }
    }
}
