using ASI.Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace ASI.Basecode.Data.Interfaces
{
    public interface IBookingRepository
    {
        IQueryable<Booking> GetAllBookings();
        void AddBooking(Booking booking);
        IQueryable<Booking> GetBookingById(int userId);

        IQueryable<Booking> GetPendingBookings();

        IQueryable<Booking> GetPendingBookingsById(int userId);
    }
}