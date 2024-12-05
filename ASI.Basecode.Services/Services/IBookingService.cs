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
using Microsoft.AspNetCore.Http;

namespace ASI.Basecode.Services.Services
{
    public class BookingService : IBookingService
    {
        private readonly IMapper _mapper;
        private readonly IBookingRepository _bookingRepository;
        private readonly IFacilityRepository _facilityRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BookingService(IMapper mapper, IBookingRepository bookingRepository, IFacilityRepository facilityRepository, IHttpContextAccessor httpContextAccessor)
        {
            _bookingRepository = bookingRepository;
            _facilityRepository = facilityRepository;
            _httpContextAccessor = httpContextAccessor;
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
                FullDayDuration = b.FullDayDuration,
                BookingPrice = b.BookingPrice,
                BookingDays = b.BookingDays,
                BookingType = b.BookingType,
            }).ToList();
        }

        public void AddBooking(BookingViewModel model)
        {
            // Get the user role from session
            var userRole = _httpContextAccessor.HttpContext.Session.GetInt32("Role");

            //check if the model facility id has value for retrieving a specific facility
            if (!model.FacilityId.HasValue)
{
                throw new ArgumentNullException("FacilityId cannot be null.");
            }
            var facility = _facilityRepository.GetFacilityById(model.FacilityId.Value);

           
            if(userRole == 3 ||  userRole == 1)  //check if admin or super admin
            {
                model.BookingStatus = "Booked";
            } else
            {
                if (facility.Confirmation)
                {
                    model.BookingStatus = "Pending";
                }
                else
                {
                    model.BookingStatus = "Booked";
                }
            }
            var booking = new Booking();
            _mapper.Map(model, booking);
            booking.FacilityName = model.FacilityName;
            booking.BookingId = model.BookingId;
            booking.BookingDate = model.BookingDate;
            booking.BookingTimeStart = model.BookingTimeStart;
            booking.BookingTimeEnd = model.BookingTimeEnd;
            booking.BookingStatus = model.BookingStatus;
            booking.Notes = model.Notes;
            booking.CreatedBy = Environment.UserName;
            booking.UpdatedBy = Environment.UserName;
            booking.BookingPrice = model.BookingPrice;
            booking.FullDayDuration = model.FullDayDuration;
            booking.BookingType = model.BookingType;
            booking.BookingDays = model.BookingDays;

            _bookingRepository.AddBooking(booking);
        }

        public IEnumerable<BookingViewModel> GetBookingById(int userId)
        {

            var newUserId = userId;
            var data = _bookingRepository.GetBookingById(userId)
                .Select(b => new BookingViewModel
                {
                    BookingId = b.BookingId,
                    BookingDate = b.BookingDate,
                    Notes = b.Notes,
                    BookingStatus = b.BookingStatus,
                    BookingTimeStart = b.BookingTimeStart,
                    BookingTimeEnd = b.BookingTimeEnd,
                    FacilityName = b.FacilityName,
                    FullDayDuration = b.FullDayDuration,
                    BookingPrice = b.BookingPrice,
                    BookingDays = b.BookingDays,
                    BookingType = b.BookingType,
                }).ToList(); // Assuming you want a single result
            return data;
            
            /*var data = _bookingRepository.GetAllBookings()
                .Where(x => x.UserId == newUserId)
                .Select(b => new BookingViewModel
                {
                    BookingId = b.BookingId,
                    BookingDate = b.BookingDate,
                    Notes = b.Notes,
                    BookingStatus = b.BookingStatus,
                    BookingTimeStart = b.BookingTimeStart,
                    BookingTimeEnd = b.BookingTimeEnd,
                    FacilityName = b.FacilityName,
                    FullDayDuration = b.FullDayDuration,
                    BookingPrice = b.BookingPrice,
                    BookingDays = b.BookingDays,
                    BookingType = b.BookingType,
                }).ToList(); // Assuming you want a single result
            return data;*/
        }

        public IEnumerable<BookingViewModel> GetPendingBookings()
        {
            //var booking = _bookingRepository.GetAllBookings().Where(b => b.BookingStatus == "Pending").ToList();
            var booking = _bookingRepository.GetPendingBookings();
            return booking.Select(b => new BookingViewModel
            {
                BookingId = b.BookingId,
                BookingDate = b.BookingDate,
                Notes = b.Notes,
                BookingStatus = b.BookingStatus,
                BookingTimeStart = b.BookingTimeStart,
                BookingTimeEnd = b.BookingTimeEnd,
                FacilityName = b.FacilityName,
                FullDayDuration = b.FullDayDuration,
                BookingPrice = b.BookingPrice,
                BookingDays = b.BookingDays,
                BookingType = b.BookingType,
            }).ToList();
        }

        public IEnumerable<BookingViewModel> GetPendingBookingsById(int userId)
        {
            //var booking = _bookingRepository.GetAllBookings().Where(b => b.BookingStatus == "Pending" && b.UserId == userId).ToList();
            var booking = _bookingRepository.GetPendingBookingsById(userId);
            return booking.Select(b => new BookingViewModel
            {
                BookingId = b.BookingId,
                BookingDate = b.BookingDate,
                Notes = b.Notes,
                BookingStatus = b.BookingStatus,
                BookingTimeStart = b.BookingTimeStart,
                BookingTimeEnd = b.BookingTimeEnd,
                FacilityName = b.FacilityName,
                FullDayDuration = b.FullDayDuration,
                BookingPrice = b.BookingPrice,
                BookingDays = b.BookingDays,
                BookingType = b.BookingType,
            }).ToList();
        }
    }
}
