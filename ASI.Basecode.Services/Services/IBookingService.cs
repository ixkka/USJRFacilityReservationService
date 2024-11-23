using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Data.Repositories;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.Manager;
using ASI.Basecode.Services.ServiceModels;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.Services
{
    public class BookingService : IBookingService
    {
        private readonly IMapper _mapper;
        private readonly IBookingRepository _bookingRepository;

        public BookingService(IMapper mapper, IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
            _mapper = mapper;
        }


        public IEnumerable<BookingViewModel> GetAllBookings()
        {
            var booking = _bookingRepository.GetAllBookings().ToList();
            return booking.Select(b=> new BookingViewModel
            {
                BookingId = b.BookingId,
                BookingDate = b.BookingDate,
                Notes = b.Notes,
                BookingStatus = b.BookingStatus,
                BookingTimeStart = b.BookingTimeStart,
                BookingTimeEnd = b.BookingTimeEnd,
                FacilityName = b.FacilityName,
            }).ToList();
        }

        public void AddBooking(BookingViewModel model)
        {
            var booking = new Booking();
            _mapper.Map(model, booking);
            booking.FacilityName = model.FacilityName;
            booking.BookingId = model.BookingId;
            booking.BookingDate = model.BookingDate;
            booking.BookingDate = model.BookingDate;
            booking.BookingTimeStart = model.BookingTimeStart;
            booking.BookingTimeEnd = model.BookingTimeEnd;
            booking.BookingStatus = "booked";
            booking.Notes = model.Notes;
            booking.CreatedBy = Environment.UserName;
            booking.UpdatedBy = Environment.UserName;

            _bookingRepository.AddBooking(booking);
        }
    }
}
