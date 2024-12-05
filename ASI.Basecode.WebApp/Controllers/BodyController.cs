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
            var userId = HttpContext.Session.GetInt32("UserId");
            var userRole = HttpContext.Session.GetInt32("Role");
            try
            {
                //ViewBag.CurrentView = "Reservations";
                if(userId.HasValue)
                {
                    if (userRole == 3) {
                        //var bookings = _bookingService.GetBookingById(userId.Value);
                        var bookings = _bookingService.GetAllBookings();
                        ViewBag.adminCount = _bookingService.GetPendingBookings().Count();

                        int pageSize = 6;
                        var pagedBookings = bookings.ToPagedList(page, pageSize);

                        return PartialView("/Views/Body/_Reservations.cshtml", pagedBookings); // Explicitly return _Reservations
                    }
                    else
                    {
                        var bookings = _bookingService.GetBookingById(userId.Value);
                        ViewBag.count = bookings.Count();

                        int pageSize = 6;
                        var pagedBookings = bookings.ToPagedList(page, pageSize);

                        return PartialView("/Views/Body/_UserReservations.cshtml", pagedBookings);
                    }
                }
                else
                {
                    // Return an empty list if the userId is null
                    var emptyBookings = Enumerable.Empty<Booking>().ToList();
                    return PartialView("/Views/Body/_Reservations.cshtml", emptyBookings);
                }
               
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
            ViewBag.BookingType = "Single";

            return PartialView("/Views/Body/_CreateSingleBooking.cshtml");
        }

        [HttpGet]
        public IActionResult RecurringBookingReservation()
        {
            var facilityList = _facilityService.GetFacilities();
            ViewBag.facilities = facilityList.ToList();
            ViewBag.Type = "Recurring";

            return PartialView("/Views/Body/_CreateRecurringBooking.cshtml");
        }

        [HttpGet]
        public IActionResult SingleBookingReservationFromSpecFacility(int facilityId)
        {
            var facilityList = _facilityService.GetFacilities();
            ViewBag.facilities = facilityList.ToList();
            ViewBag.selectedFacilityId = facilityId;

            return PartialView("/Views/Body/_CreateSingleBooking.cshtml");
        }

        [HttpGet]
        public IActionResult ReservationRequests(int page = 1)
        {
            var bookings = _bookingService.GetPendingBookings();
            ViewBag.adminCount = bookings.Count();

            int pageSize = 6;
            var pagedBookings = bookings.ToPagedList(page, pageSize);

            return PartialView("/Views/Body/_ReservationRequests.cshtml", pagedBookings);
            

        }
        [HttpGet]
        public IActionResult PendingReservations(int page = 1)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId.HasValue) {
                var bookings = _bookingService.GetPendingBookingsById(userId.Value);
                ViewBag.count = bookings.Count();

                int pageSize = 6;
                var pagedBookings = bookings.ToPagedList(page, pageSize);

                return PartialView("/Views/Body/_PendingRequests.cshtml", pagedBookings);
            } else
            {
                // Return an empty list if the userId is null
                var emptyBookings = Enumerable.Empty<Booking>().ToList();
                return PartialView("/Views/Body/_PendingRequests.cshtml", emptyBookings);
            }
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
                // Or use Convert.ToInt32()
                booking.BookingPrice = Convert.ToInt32(booking.BookingPrice);
                _bookingService.AddBooking(booking);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = Resources.Messages.Errors.ServerError;
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult CreateRecurringBooking(BookingViewModel booking)
        {
            try
            {
                // Or use Convert.ToInt32()
                booking.BookingPrice = Convert.ToInt32(booking.BookingPrice);
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
