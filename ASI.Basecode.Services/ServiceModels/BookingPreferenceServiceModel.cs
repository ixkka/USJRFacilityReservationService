using ASI.Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.ServiceModels
{
    public class BookingPreferenceServiceModel
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

