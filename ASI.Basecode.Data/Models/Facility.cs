using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Models
{
    public partial class Facility
    {
        public Facility()
        {
            Bookings = new HashSet<Booking>();
            ImageGalleries = new HashSet<ImageGallery>();
        }

        public int FacilityId { get; set; }
        public string FacilityName { get; set; }
        public string Thumbnail { get; set; }
        public int? Capacity { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDt { get; set; }
        public string Amenity { get; set; }
        public string BookingDays { get; set; }
        public string BookingHoursStart { get; set; }
        public string BookingHoursEnd { get; set; }
        public string BookingDuration { get; set; }
        public int BookingPrice { get; set; }
        public bool Confirmation { get; set; } 

        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<ImageGallery> ImageGalleries { get; set; }
    }
}
