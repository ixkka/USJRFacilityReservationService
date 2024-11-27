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

namespace ASI.Basecode.WebApp.Controllers
{
    public class BodyController : ControllerBase<BodyController>
    {

        private readonly IBookingService _bookingService;
        public BodyController(IHttpContextAccessor httpContextAccessor,
                              ILoggerFactory loggerFactory,
                              IConfiguration configuration,
                              IBookingService bookingService,
                              IMapper mapper = null) : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {
            this._bookingService = bookingService;
        }

        [HttpGet]
        public IActionResult Reservations()
        {
            try
            {
                ViewBag.CurrentView = "Reservations";
                var bookings = _bookingService.GetAllBookings();

                return PartialView("/Views/Body/_Reservations.cshtml", bookings);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Failed to load reservations");
            }
        }

        [HttpGet]
        public IActionResult BookReservation()
        {
            return PartialView("/Views/Body/_CreateSingleBooking.cshtml");
        }

        [HttpGet]
        public IActionResult PendingReservations()
        {
            return PartialView("/Views/Body/_PendingAndRequests.cshtml");
        }

        [HttpGet]
        public IActionResult Settings()
        {
            return PartialView("/Views/Body/_Settings.cshtml");
        }
        public IActionResult CancelBooking()
        {
            // Logic to handle cancellation
            return RedirectToAction("/Views/Body/_PendingAndRequests.cshtml"); // Or any other action
        }

        [HttpPost]
        public IActionResult CreateBooking(BookingViewModel booking)
        {
            try
            {
                _bookingService.AddBooking(booking);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = Resources.Messages.Errors.ServerError;
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
