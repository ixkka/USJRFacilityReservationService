using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Models
{
    public partial class DayOfTheWeek
    {
        public DayOfTheWeek()
        {
            Recurrences = new HashSet<Recurrence>();
        }

        public int DayOfWeekId { get; set; }
        public string DayName { get; set; }

        public virtual ICollection<Recurrence> Recurrences { get; set; }
    }
}
