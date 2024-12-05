using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.Interfaces
{
    public interface IBookingService
    {
        IEnumerable<BookingViewModel> GetAllBookings();
        void AddBooking(BookingViewModel model);
        IEnumerable<BookingViewModel> GetBookingById(int userId);

        IEnumerable<BookingViewModel> GetPendingBookings();

        IEnumerable<BookingViewModel> GetPendingBookingsById(int userId);
    }
}
