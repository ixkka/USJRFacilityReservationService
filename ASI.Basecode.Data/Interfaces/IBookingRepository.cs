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
        IQueryable<Booking> GetBookingByUserId(int userId);

        IQueryable<Booking> GetPendingBookings();
        void UpdateBooking(Booking booking);

        IQueryable<Booking> GetPendingBookingsById(int userId);

        void RejectBooking(int bookingId);

        Booking GetSpecificBooking(int bookingId);

        void AcceptBooking(int bookingId);
        void DeleteBooking(int bookingId);
        //void DeleteBooking(Booking booking);
    }
}