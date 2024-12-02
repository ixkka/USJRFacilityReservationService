using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using ASI.Basecode.Services.Services;
using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ASI.Basecode.WebApp.Controllers
{
    public class FacilityController : ControllerBase<FacilityController>
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFacilityService _facilityService;
        public FacilityController(IHttpContextAccessor httpContextAccessor,
                              ILoggerFactory loggerFactory,
                              IWebHostEnvironment webHostEnvironment,
                              IConfiguration configuration,
                              IFacilityService facilityService,
                              IMapper mapper = null) : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {
            _webHostEnvironment = webHostEnvironment;
            this._facilityService = facilityService;
        }

        [HttpPost]
        public async Task<IActionResult> AddFacility(FacilityViewModel facility)
        {
            if (ModelState.IsValid)
            {
                if (facility.FacilityThumbnailImg != null)
                {
                    // Create uploads directory
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");

                    // Ensure the uploads directory exists and create one if it does not
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string fileName = Guid.NewGuid().ToString() + "_" + facility.FacilityThumbnailImg.FileName;

                    facility.Thumbnail = fileName;

                    string serverFolder = Path.Combine(uploadsFolder, fileName);

                    try
                    {
                        await facility.FacilityThumbnailImg.CopyToAsync(new FileStream(serverFolder, FileMode.Create));
                        //return Json(new { success = true, message = "Image uploaded successfully" });
                        _facilityService.AddFacility2(facility);
                        return RedirectToAction("Index", "Home");
                        //return Ok("Success");
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("ImageUpload", "Image upload failed: " + ex.Message);
                        //return Json(new { success = false, message = "Image upload failed: " + ex.Message });
                        TempData["ErrorMessage"] = Resources.Messages.Errors.ServerError;
                    }
                }
            }
            return RedirectToAction("Index", "Home");
        }

     //[HttpGet]
        public IActionResult GetspecFacility(int facilityId)
        {
            var newId = facilityId;
            var facility = _facilityService.GetFacilityByIdService(newId);

            if (facility == null)
            {
                return NotFound("Facility Not Found this is GetspecFacility");
            }

            return PartialView("/Views/Body/_SpecificFacility.cshtml", facility);
            //return View();
        }

        [HttpPost]
        public IActionResult DeleteFacility(FacilityViewModel room)
        {
            if (room == null || room.FacilityId <= 0)
            {
                return BadRequest("Invalid facility data.");
            }

            try
            {
                _facilityService.DeleteFacility(room);
                //return Ok("Facility deleted successfully.");
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                // Log the exception (not shown here)
                return StatusCode(500, "Internal server error");
            }
        }

        // Update Facility GET action to load the update form
        [HttpGet]
        public IActionResult Edit(int facilityId)
        {
            var facility = _facilityService.GetFacilityByIdService(facilityId);

            if (facility == null)
            {
                return NotFound("Facility not found.");
            }

            // Return the view with the facility details
            return View(facility);
        }

        // Update Facility POST action to handle form submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(FacilityViewModel facility)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _facilityService.UpdateFacility(facility);

                    // Redirect to the index or listing page after update
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    // Log the exception (not shown here)
                    ModelState.AddModelError("", "An error occurred while updating the facility.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(facility);
        }

        // Other controller actions


        public IActionResult Index()
        {
            return View();
        }

    }
}
