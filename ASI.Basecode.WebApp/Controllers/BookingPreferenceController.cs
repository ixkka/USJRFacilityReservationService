﻿using ASI.Basecode.Data.Models;
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

        [HttpGet]
        public IActionResult Save()
        {
            var model = new BookingPreferenceViewModel();
            return PartialView("~/Views/Body/_Settings.cshtml", model);  // Ensure correct path
        }

        [HttpPost]
        public IActionResult Save(BookingPreferenceServiceModel model)
        {
            if (ModelState.IsValid)
            {
              
                var serviceModel = new BookingPreferenceServiceModel    
                {
                    UserId = model.UserId,
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

            return View("~/Views/Body/_Settings.cshtml", model);
        }
    }
}
