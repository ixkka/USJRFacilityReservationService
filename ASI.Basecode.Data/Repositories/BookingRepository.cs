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

        public IQueryable<Booking> GetBookingById(int userId)
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
    }
}
