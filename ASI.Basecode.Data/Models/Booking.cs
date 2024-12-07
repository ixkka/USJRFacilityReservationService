using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Models
{
    public partial class Booking
    {
        public Booking()
        {
            Recurrences = new HashSet<Recurrence>();
        }

        public int BookingId { get; set; }
        public int? UserId { get; set; }
        public string Thumbnail { get; set; }
        public int? FacilityId { get; set; }
        public string BookingStatus { get; set; }
        public string FacilityName { get; set; }
        public string Notes { get; set; }
        public string BookingType { get; set; }
        public DateTime BookingDate { get; set; }
        public string FullDayDuration { get; set; }
        public string BookingDays { get; set; }
        public int BookingPrice { get; set; }
        public TimeSpan? BookingTimeStart { get; set; }
        public TimeSpan? BookingTimeEnd { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDt { get; set; }

        public virtual Facility Facility { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Recurrence> Recurrences { get; set; }
    }
}
