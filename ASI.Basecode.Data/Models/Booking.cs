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
        public int? RoomId { get; set; }
        public string BookingStatus { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public TimeSpan? TimeFrom { get; set; }
        public TimeSpan? TimeTo { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDt { get; set; }

        public virtual Room Room { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Recurrence> Recurrences { get; set; }
    }
}
