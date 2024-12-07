using ASI.Basecode.Data.Interfaces;
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
using System.Collections.Generic;
using System.Linq;
using X.PagedList.Extensions;

namespace ASI.Basecode.WebApp.Controllers
{
    public class BodyController : ControllerBase<BodyController>
    {

        private readonly IBookingService _bookingService;
        private readonly IFacilityService _facilityService;
        private readonly IFacilityRepository _facilityRepository;

        public BodyController(IHttpContextAccessor httpContextAccessor,
                              ILoggerFactory loggerFactory,
                              IConfiguration configuration,
                              IBookingService bookingService,
                              IFacilityService facilityService,
                              IFacilityRepository facilityRepository,
                              IMapper mapper = null) : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {
            this._bookingService = bookingService;
            this._facilityService = facilityService;
            this._facilityRepository = facilityRepository;
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
                    if (userRole == 3 || userRole == 1) {
                        var bookings = _bookingService.GetBookingByUserId(userId.Value);

                        //var bookings = _bookingService.GetAllBookings();
                        ViewBag.adminCount = _bookingService.GetPendingBookings().Count();

                        int pageSize = 6;
                        var pagedBookings = bookings.ToPagedList(page, pageSize);

                        return PartialView("/Views/Body/_Reservations.cshtml", pagedBookings); // Explicitly return _Reservations
                    }
                    else
                    {
                        var bookings = _bookingService.GetBookingByUserId(userId.Value);
                        ViewBag.count = _bookingService.GetPendingBookingsById(userId.Value).Count();

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
        public IActionResult Settings()
        {
            return PartialView("/Views/Body/_Settings.cshtml");
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

        [HttpPost]
        public IActionResult CreateBooking(BookingViewModel booking)
        {
            try
            {
                // Convert BookingPrice to integer
                booking.BookingPrice = Convert.ToInt32(booking.BookingPrice);
                _bookingService.AddBooking(booking);

                return RedirectToAction("Index", "Home");
            }
            catch (ArgumentException ex)
            {
                // Pass the specific error message to the view
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index", "Home");
            }

        }

        [HttpPost]
        public IActionResult RejectBooking(int id)
        {
            try
            {
                _bookingService.RejectBooking(id);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = Resources.Messages.Errors.ServerError;
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [HttpGet]
        public IActionResult AcceptBooking(int id)
        {
            try
            {
                _bookingService.AcceptBooking(id);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = Resources.Messages.Errors.ServerError;
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult DeleteBooking(int id)
        {
            try
            {
                _bookingService.DeleteBooking(id);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = Resources.Messages.Errors.ServerError;
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult ViewBooking(int bookingId)
        {
            var newId = bookingId;
            var booking = _bookingService.GetSpecificBooking(newId);

            if (booking == null)
            {
                return NotFound("Booking Not Found");
            }

            return PartialView("/Views/Body/_ViewBooking.cshtml", booking);
            //return View();
        }
        public IActionResult UserAdminViewBooking(int bookingId)
        {
            var newId = bookingId;
            var booking = _bookingService.GetSpecificBooking(newId);

            ViewBag.id = booking.BookingId;
            if (booking == null)
            {
                return NotFound("Booking Not Found");
            }

            return PartialView("/Views/Body/_UserAdminViewBooking.cshtml", booking);
            //return View();
        }

        public IActionResult EditBooking(int bookingId)
        {
            var newId = bookingId;
            var booking = _bookingService.GetSpecificBooking(newId);

            var facilityList = _facilityService.GetFacilities();
            ViewBag.facilities = facilityList.ToList();

            if (booking == null)
            {
                return NotFound("Booking Not Found");
            }

            if(booking.BookingType == "Single")
            {
                return PartialView("/Views/Body/_EditSingleBooking.cshtml", booking);
            }
            else
            {
                return PartialView("/Views/Body/_EditRecurringBooking.cshtml", booking);
            }
        }

        [HttpPost]
        public IActionResult UpdateBooking(BookingViewModel booking)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _bookingService.UpdateBooking(booking);

                    // Redirect to the index or listing page after update
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    // Log the exception (not shown here)
                    ModelState.AddModelError("", "An error occurred while updating the booking.");
                }
            }

            // If we got this far, something failed, redisplay form
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
