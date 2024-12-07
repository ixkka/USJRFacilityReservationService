using ASI.Basecode.Data.Models;
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


namespace ASI.Basecode.WebApp.Controllers
{
    public class BookingPreferenceController : Controller
    {
        private readonly IBookingPreferenceService _bookingPreferenceService;

        public BookingPreferenceController(IBookingPreferenceService bookingPreferenceService)
        {
            _bookingPreferenceService = bookingPreferenceService ?? throw new ArgumentNullException(nameof(bookingPreferenceService));
        }

        ///Returns a partial view with an empty booking preference form for users to input their preferences.
        [HttpGet]
        public IActionResult Save()
        {
            var model = new BookingPreferenceViewModel();
            return PartialView("~/Views/Body/_Settings.cshtml", model);  // Ensure correct path
        }

        ///Processes and saves a user's booking preferences including single and recurrent booking details to the database, associated with their user ID.
        [HttpPost]
        public IActionResult Save(BookingPreferenceServiceModel model)
        {
            if (ModelState.IsValid)
            {

                var userId = HttpContext.Session.GetInt32("UserId");

                if (userId.HasValue){
                    var serviceModel = new BookingPreferenceServiceModel
                    {
                        UserId = userId,
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
                    return RedirectToAction("Index", "Home");
                }
                
            }

            return View("~/Views/Body/_Settings.cshtml", model);
        }
    }


}
