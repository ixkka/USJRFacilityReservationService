using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.Services
{
    public interface IBookingPreferenceService
    {
        void AddPreference(BookingPreferenceServiceModel serviceModel);
        void UpdatePreference(BookingPreferenceServiceModel serviceModel);
    }
}