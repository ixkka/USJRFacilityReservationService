using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Data.Repositories;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.Manager;
using ASI.Basecode.Services.ServiceModels;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ASI.Basecode.Services/BookingPreferenceService.cs
// ASI.Basecode.Services/BookingPreferenceService.cs
namespace ASI.Basecode.Services.Services
{
    public class BookingPreferenceService : IBookingPreferenceService
    {
        private readonly IBookingPreferenceRepository _bookingPreferenceRepository;

        public BookingPreferenceService(IBookingPreferenceRepository bookingPreferenceRepository)
        {
            _bookingPreferenceRepository = bookingPreferenceRepository;
        }

        // Implementing AddPreference method
        public void AddPreference(BookingPreferenceServiceModel serviceModel)
        {
            var bookingPreference = new BookingPreference
            {
                BookingPreferenceId = serviceModel.BookingPreferenceId,
                UserId = serviceModel.UserId,
                SingleBookingStartTime = serviceModel.SingleBookingStartTime,
                SingleBookingEndTime = serviceModel.SingleBookingEndTime,
                SingleBookingNotes = serviceModel.SingleBookingNotes,
                RecurrentBookingStartTime = serviceModel.RecurrentBookingStartTime,
                RecurrentBookingEndTime = serviceModel.RecurrentBookingEndTime,
                RecurrentBookingDays = serviceModel.RecurrentBookingDays,
                RecurrentBookingNotes = serviceModel.RecurrentBookingNotes
            };

            _bookingPreferenceRepository.AddPreference(bookingPreference);
        }

        // Implementing UpdatePreference method
        public void UpdatePreference(BookingPreferenceServiceModel serviceModel)
        {
            var bookingPreference = new BookingPreference
            {
                BookingPreferenceId = serviceModel.BookingPreferenceId,
                UserId = serviceModel.UserId,
                SingleBookingStartTime = serviceModel.SingleBookingStartTime,
                SingleBookingEndTime = serviceModel.SingleBookingEndTime,
                SingleBookingNotes = serviceModel.SingleBookingNotes,
                RecurrentBookingStartTime = serviceModel.RecurrentBookingStartTime,
                RecurrentBookingEndTime = serviceModel.RecurrentBookingEndTime,
                RecurrentBookingDays = serviceModel.RecurrentBookingDays,
                RecurrentBookingNotes = serviceModel.RecurrentBookingNotes
            };

            _bookingPreferenceRepository.UpdatePreference(bookingPreference);
        }
    }
}


