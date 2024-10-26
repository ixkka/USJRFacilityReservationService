using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Models
{
    public partial class Recurrence
    {
        public int RecurrenceId { get; set; }
        public int? DayOfWeekId { get; set; }
        public int? BookingId { get; set; }

        public virtual Booking Booking { get; set; }
        public virtual DayOfTheWeek DayOfWeek { get; set; }
    }
}
