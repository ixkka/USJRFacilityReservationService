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

namespace ASI.Basecode.WebApp.Controllers
{
    public class BodyController : ControllerBase<BodyController>
    {

        private readonly IBookingService _bookingService;
        private readonly IFacilityService _facilityService;
        public BodyController(IHttpContextAccessor httpContextAccessor,
                              ILoggerFactory loggerFactory,
                              IConfiguration configuration,
                              IBookingService bookingService,
                              IFacilityService facilityService,
                              IMapper mapper = null) : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {
            this._bookingService = bookingService;
            this._facilityService = facilityService;
        }

        [HttpGet]
        public IActionResult Reservations(int page = 1)
        {
            try
            {
                //ViewBag.CurrentView = "Reservations";
                var bookings = _bookingService.GetAllBookings();
   
                int pageSize = 6;
                var pagedBookings = bookings.ToPagedList(page, pageSize);

                return PartialView("/Views/Body/_Reservations.cshtml", pagedBookings);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Failed to load reservations");
            }
        }

        [HttpGet]
        public IActionResult SingleBookingReservation()
        {
            var facilityList = _facilityService.GetFacilities();
            ViewBag.facilities = facilityList.ToList();

            return PartialView("/Views/Body/_CreateSingleBooking.cshtml");
        }

        [HttpGet]
        public IActionResult PendingReservations()
        {
            return PartialView("/Views/Body/_PendingAndRequests.cshtml");
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
