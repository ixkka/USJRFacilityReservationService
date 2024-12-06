using System;

namespace ASI.Basecode.WebApp.Models
{
    public class BookingPreferenceViewModel
    {
        public TimeSpan? SingleBookingStartTime { get; set; }
        public TimeSpan? SingleBookingEndTime { get; set; }
        public string SingleBookingNotes { get; set; }
        public TimeSpan? RecurrentBookingStartTime { get; set; }
        public TimeSpan? RecurrentBookingEndTime { get; set; }
        public string RecurrentBookingDays { get; set; }
        public string RecurrentBookingNotes { get; set; }
    }
}