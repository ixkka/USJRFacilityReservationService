using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
                    // Create uploads directory if it doesn't exist
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
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
            //return View("_Facilities");
            //return Redirect(Url.Action("_Facilities", "Body"));
            return RedirectToAction("Index", "Home");
            //return RedirectToAction("Facilities", "Body");
            //return RedirectToRoute("/Views/Body/_Facilities.cshtml");
            //return View("~/Views/Body/_Facilities.cshtml");
            //return PartialView("~/Views/Body/_Facilities.cshtml");
        }


        public IActionResult Index()
        {
            return View();
        }

    }
}
