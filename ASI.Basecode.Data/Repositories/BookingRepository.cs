using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using Basecode.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Repositories
{
    public class BookingRepository : BaseRepository, IBookingRepository
    {
        public BookingRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public IQueryable<Booking> GetAllBookings()
        {
            return this.GetDbSet<Booking>();
        }

        public void AddBooking(Booking booking)
        {
            this.GetDbSet<Booking>().Add(booking);
            UnitOfWork.SaveChanges();
        }

        public IQueryable<Booking> GetBookingByUserId(int userId)
        {
            return this.GetDbSet<Booking>().Where(u => u.UserId == userId);
        }

        public IQueryable<Booking> GetPendingBookings()
        {
            //return this.GetDbSet<Booking>().Where(u => u.UserId == userId);
            return this.GetDbSet<Booking>().Where(u => u.BookingStatus == "Pending");
        }
        public IQueryable<Booking> GetPendingBookingsById(int userId)
        {
            return this.GetDbSet<Booking>().Where(u => u.BookingStatus == "Pending" && u.UserId == userId);
        }

        public void UpdateBooking(Booking booking)
        {
            this.GetDbSet<Booking>().Update(booking);
            UnitOfWork.SaveChanges();
        }
        public void RejectBooking(int bookingId)
        {
            var booking = this.GetDbSet<Booking>()
                .Where(b => b.BookingId == bookingId)
                .SingleOrDefault();

            if (booking != null)
            {
                booking.BookingStatus = "Rejected";
                this.GetDbSet<Booking>().Update(booking);
                UnitOfWork.SaveChanges();
            }
        }
        public Booking GetSpecificBooking(int bookingId)
        {
            return this.GetDbSet<Booking>().FirstOrDefault(u => u.BookingId == bookingId);
        }
        public void DeleteBooking(int bookingId)
        {
            var booking = this.GetDbSet<Booking>()
                .Where(b => b.BookingId == bookingId)
                .SingleOrDefault();

            if (booking != null)
            {
                booking.BookingStatus = "Booked";
                this.GetDbSet<Booking>().Remove(booking);
                UnitOfWork.SaveChanges();
            }
        }

        public void AcceptBooking(int bookingId)
        {
            var booking = this.GetDbSet<Booking>()
                .Where(b => b.BookingId == bookingId)
                .SingleOrDefault();

            if (booking != null)
            {
                booking.BookingStatus = "Booked";
                this.GetDbSet<Booking>().Update(booking);
                UnitOfWork.SaveChanges();
            }
        }



    }
}
