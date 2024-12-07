using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ASI.Basecode.Data.Models/BookingPreference.cs
namespace ASI.Basecode.Data.Models
{
    public class BookingPreference
    {
        public int BookingPreferenceId { get; set; }
        public int? UserId { get; set; }
        public TimeSpan? SingleBookingStartTime { get; set; }
        public TimeSpan? SingleBookingEndTime { get; set; }
        public string SingleBookingNotes { get; set; }
        public TimeSpan? RecurrentBookingStartTime { get; set; }
        public TimeSpan? RecurrentBookingEndTime { get; set; }
        public string RecurrentBookingDays { get; set; }
        public string RecurrentBookingNotes { get; set; }

        public virtual User User { get; set; }
    }
}

