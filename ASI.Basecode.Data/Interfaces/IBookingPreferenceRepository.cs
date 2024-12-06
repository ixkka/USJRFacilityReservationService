using ASI.Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Interfaces
{
    public interface IBookingPreferenceRepository
    {
        void AddPreference(BookingPreference bookingPreference);
        void UpdatePreference(BookingPreference bookingPreference);
    }
}
