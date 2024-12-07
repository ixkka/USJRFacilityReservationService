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
using System.Globalization;

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
                Thumbnail = b.Thumbnail,
            }).ToList();
        }

        public IEnumerable<BookingViewModel> GetBookingByUserId(int userId)
        {
            var newUserId = userId;
            var data = _bookingRepository.GetBookingByUserId(userId)
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
                     Thumbnail = b.Thumbnail,
                 }).ToList();

            return data;
        }


        public IEnumerable<BookingViewModel> GetPendingBookings()
        {
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
                Thumbnail = b.Thumbnail,
            }).ToList();
        }

        public IEnumerable<BookingViewModel> GetPendingBookingsById(int userId)
        {
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
                Thumbnail= b.Thumbnail,
            }).ToList();
        }

        public void RejectBooking(int id)
        {
            _bookingRepository.RejectBooking(id);
        }

        public void AcceptBooking(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ArgumentException("Invalid booking ID");
                }

                _bookingRepository.AcceptBooking(id);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void DeleteBooking(int id)
        {
            _bookingRepository.DeleteBooking(id);
        }

        public BookingViewModel GetSpecificBooking(int bookingId)
        {

            var newbookingId = bookingId;
            var data = _bookingRepository.GetAllBookings()
                .Where(x => x.BookingId == newbookingId)
                .Select(s => new BookingViewModel
                {
                    FacilityName = s.FacilityName,
                    BookingPrice = s.BookingPrice,
                    BookingDays = s.BookingDays,
                    FullDayDuration= s.FullDayDuration,
                    BookingDate = s.BookingDate,
                    BookingTimeEnd = s.BookingTimeEnd,
                    BookingTimeStart = s.BookingTimeStart,
                    BookingType = s.BookingType,
                    BookingStatus = s.BookingStatus,
                    Notes = s.Notes,
                    BookingId = s.BookingId,
                    Thumbnail = s.Thumbnail,
                }).FirstOrDefault(); 
            return data;
        }



        public void UpdateBooking(BookingViewModel model)
        {
            var existingData = _bookingRepository.GetAllBookings()
                .FirstOrDefault(s => s.BookingId == model.BookingId);

            if (existingData != null)
            {
                _mapper.Map(model, existingData);

                existingData.UpdatedDt = DateTime.Now;
                existingData.UpdatedBy = System.Environment.UserName;
                existingData.BookingStatus = model.BookingStatus;
                existingData.BookingType = model.BookingType;
                existingData.Notes = model.Notes;
                existingData.BookingDate = model.BookingDate;
                existingData.BookingPrice = model.BookingPrice;
                existingData.FullDayDuration = model.FullDayDuration;
                existingData.FacilityName = model.FacilityName;
                existingData.FacilityId = model.FacilityId;
                existingData.BookingDays = model.BookingDays;
                existingData.BookingTimeEnd = model.BookingTimeEnd;
                existingData.BookingTimeStart = model.BookingTimeStart;
                existingData.Thumbnail = model.Thumbnail;

                _bookingRepository.UpdateBooking(existingData);
            }
            else
            {
                throw new Exception("Booking not found.");
            }
        }
     
        public void AddBooking(BookingViewModel model)
        {
            // Get the user role from session
            var userRole = _httpContextAccessor.HttpContext.Session.GetInt32("Role");

            // Get Facility Associated with the Model
            var facility = _facilityRepository.GetFacilityById(model.FacilityId.Value);

            if (userRole == 3 || userRole == 1)
            {
                model.BookingStatus = "Booked";
            }
            else
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
            booking.Thumbnail = model.Thumbnail;

            _bookingRepository.AddBooking(booking);
        }

    }
}
