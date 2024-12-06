using ASI.Basecode.Data.Models;
using ASI.Basecode.Service.Interfaces;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using ASI.Basecode.Services.Services;
using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using X.PagedList.Extensions;
using ASI.Basecode.WebApp.Models;
/*using BookingPreferenceServiceModel = ASI.Basecode.Services.ServiceModels.BookingPreferenceServiceModel;*/
/*using BookingPreferenceViewModel = ASI.Basecode.WebApp.Models.BookingPreferenceViewModel;*/

// ASI.Basecode.WebApp.Controllers/BookingPreferenceController.cs
namespace ASI.Basecode.WebApp.Controllers
{
    public class BookingPreferenceController : Controller
    {
        private readonly IBookingPreferenceService _bookingPreferenceService;

        public BookingPreferenceController(IBookingPreferenceService bookingPreferenceService)
        {
            _bookingPreferenceService = bookingPreferenceService;
        }

        [HttpGet]
        public IActionResult Save()
        {
            var model = new BookingPreferenceViewModel();
            return View("~/Views/Body/_Settings.cshtml", model);  // Ensure correct path
        }

        [HttpPost]
        public IActionResult Save(BookingPreferenceViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Manually map the form data to the service model
                var serviceModel = new BookingPreferenceServiceModel
                {
                    SingleBookingStartTime = model.SingleBookingStartTime,
                    SingleBookingEndTime = model.SingleBookingEndTime,
                    SingleBookingNotes = model.SingleBookingNotes,
                    RecurrentBookingStartTime = model.RecurrentBookingStartTime,
                    RecurrentBookingEndTime = model.RecurrentBookingEndTime,
                    RecurrentBookingDays = model.RecurrentBookingDays,
                    RecurrentBookingNotes = model.RecurrentBookingNotes
                };

                // Save the preferences via the service
                _bookingPreferenceService.AddPreference(serviceModel);

                // Redirect to another view after saving
                return RedirectToAction("Index", "Facility");
            }

            return View("~/Views/Body/_Settings.cshtml", model);
        }
    }
}
