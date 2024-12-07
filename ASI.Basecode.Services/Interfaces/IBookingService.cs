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
        IEnumerable<BookingViewModel> GetBookingByUserId(int userId);

        IEnumerable<BookingViewModel> GetPendingBookings();
        //void DeleteBooking(BookingViewModel booking);
        void DeleteBooking(int id);
        void UpdateBooking(BookingViewModel model);
        void RejectBooking(int id);
        void AcceptBooking(int id);

        BookingViewModel GetSpecificBooking(int bookingId);
        IEnumerable<BookingViewModel> GetPendingBookingsById(int userId);
    }
}
