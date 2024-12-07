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

            //var result = _facilityRepository.GetFacility().Where(f => f.FacilityId == data)
            // Assuming you want a single result
            return data;
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
                Thumbnail = b.Thumbnail,
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
                // Validate input
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
                }).FirstOrDefault(); // Assuming you want a single result
            return data;

            //return _facilityRepository.GetFacilityById(facilityId); 
        }



        public void UpdateBooking(BookingViewModel model)
        {
            var existingData = _bookingRepository.GetAllBookings()
                .FirstOrDefault(s => s.BookingId == model.BookingId);

            if (existingData != null)
            {
                // Map the updated model to the existing data
                _mapper.Map(model, existingData);


                // Update metadata
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

        public bool ValidateBookingDay(DateTime bookingDate, string allowedDays)
        {
            // Map day of week to abbreviated format used in the allowedDays string
            Dictionary<DayOfWeek, string> dayMapping = new Dictionary<DayOfWeek, string>
            {
                { DayOfWeek.Monday, "M" },
                { DayOfWeek.Tuesday, "T" },
                { DayOfWeek.Wednesday, "W" },
                { DayOfWeek.Thursday, "Th" },
                { DayOfWeek.Friday, "F" },
                { DayOfWeek.Saturday, "Sa" },
                { DayOfWeek.Sunday, "Su" }
            };

            // Get the day of week for the booking date
            string bookingDay = dayMapping[bookingDate.DayOfWeek];

            // Split the allowed days string and check if the booking day is in the list
            string[] allowedDaysArray = allowedDays.Split(',', StringSplitOptions.RemoveEmptyEntries);
            string[] finalDays = allowedDaysArray.Select
                (c => c.Trim()).ToArray();
            return finalDays.Contains(bookingDay.Trim());
        }

        public (bool isValid, string message) ValidateBookingTime(TimeSpan bookingTimeStart, TimeSpan bookingTimeEnd, string allowedTimeStart, string allowedTimeEnd)
        {
            // Normalize allowed times
            allowedTimeStart = NormalizeTimeInput(allowedTimeStart);
            allowedTimeEnd = NormalizeTimeInput(allowedTimeEnd);

            // Parse allowed times
            if (!TimeSpan.TryParse(allowedTimeStart, out TimeSpan allowedStart))
            {
                return (false, "Invalid allowed start time format");
            }

            if (!TimeSpan.TryParse(allowedTimeEnd, out TimeSpan allowedEnd))
            {
                return (false, "Invalid allowed end time format");
            }

            // Validate booking times
            if (bookingTimeStart >= bookingTimeEnd)
            {
                return (false, "Booking start time must be before end time");
            }

            if (bookingTimeStart < allowedStart)
            {
                return (false, $"Booking cannot start before {allowedStart}");
            }

            if (bookingTimeEnd > allowedEnd)
            {
                return (false, $"Booking cannot end after {allowedEnd}");
            }

            return (true, "Booking time is valid");
        }

        // Helper method to normalize input
        private string NormalizeTimeInput(string input)
        {
            // Remove extra spaces
            input = input.Trim();

            // Add ':' if missing (e.g., "1430" -> "14:30")
            if (input.Length == 4 && !input.Contains(":"))
            {
                input = input.Insert(2, ":");
            }

            // Trim seconds if present (e.g., "14:30:00" -> "14:30")
            if (input.Length > 5 && input.Contains(":"))
            {
                input = input.Substring(0, 5);
            }

            return input;
        }
     
        public void AddBooking(BookingViewModel model)
        {
            // Get the user role from session
            var userRole = _httpContextAccessor.HttpContext.Session.GetInt32("Role");

            // Get Facility Associated with the Model
            var facility = _facilityRepository.GetFacilityById(model.FacilityId.Value);

            if (userRole == 3 || userRole == 1)  //check if admin or super admin
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
