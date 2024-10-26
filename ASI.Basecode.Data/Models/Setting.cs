using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Models
{
    public partial class Setting
    {
        public int SettingId { get; set; }
        public int? UserId { get; set; }
        public int? BookingSuccess { get; set; }
        public int? BookingStatusChange { get; set; }
        public int? BookingReminder { get; set; }
        public int? BookingDuration { get; set; }

        public virtual User User { get; set; }
    }
}
