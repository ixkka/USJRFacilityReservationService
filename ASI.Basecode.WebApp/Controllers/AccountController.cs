using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.Manager;
using ASI.Basecode.Services.ServiceModels;
/*using ASI.Basecode.Services.ServiceModels.ASI.Basecode.Services.ServiceModels;*/
using ASI.Basecode.Services.Services;
using ASI.Basecode.WebApp.Authentication;
using ASI.Basecode.WebApp.Models;
using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

using System.Text;
using System.Threading.Tasks;
using static ASI.Basecode.Resources.Constants.Enums;
using X.PagedList;
using X.PagedList.Extensions;
using Microsoft.AspNetCore.Hosting;


namespace ASI.Basecode.WebApp.Controllers
{
    public class AccountController : ControllerBase<AccountController>
    {
        private readonly SessionManager _sessionManager;
        private readonly SignInManager _signInManager;
        private readonly TokenValidationParametersFactory _tokenValidationParametersFactory;
        private readonly TokenProviderOptionsFactory _tokenProviderOptionsFactory;
        private readonly IConfiguration _appConfiguration;
        private readonly IUserService _userService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFacilityService _facilityService;





        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="signInManager">The sign in manager.</param>
        /// <param name="localizer">The localizer.</param>
        /// <param name="userService">The user service.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="tokenValidationParametersFactory">The token validation parameters factory.</param>
        /// <param name="tokenProviderOptionsFactory">The token provider options factory.</param>
        public AccountController(
                            SignInManager signInManager,
                            IWebHostEnvironment webHostEnvironment,
                            IHttpContextAccessor httpContextAccessor,
                            ILoggerFactory loggerFactory,
                            IConfiguration configuration,
                            IMapper mapper,
                            IUserService userService,
                            IFacilityService facilityService,
                            TokenValidationParametersFactory tokenValidationParametersFactory,
                            TokenProviderOptionsFactory tokenProviderOptionsFactory) : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {
            this._sessionManager = new SessionManager(this._session);
            this._signInManager = signInManager;
            this._tokenProviderOptionsFactory = tokenProviderOptionsFactory;
            this._tokenValidationParametersFactory = tokenValidationParametersFactory;
            this._appConfiguration = configuration;
            this._userService = userService;
            this._facilityService = facilityService;
            _webHostEnvironment = webHostEnvironment ?? throw new ArgumentNullException(nameof(webHostEnvironment));
        }



        /// <summary>
        /// Login Method
        /// </summary>
        /// <returns>Created response view</returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            TempData["returnUrl"] = System.Net.WebUtility.UrlDecode(HttpContext.Request.Query["ReturnUrl"]);
            this._sessionManager.Clear();
            this._session.SetString("SessionId", System.Guid.NewGuid().ToString());
            return this.View();
        }

        /// <summary>
        /// Authenticate user and signs the user in when successful.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns> Created response view </returns>
        ///


        [HttpPost]
        [AllowAnonymous]

        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            this._session.SetString("HasSession", "Exist");

            User user = null;

            // Authenticate the user
            var loginResult = _userService.AuthenticateUser(model.UserId, model.Password, ref user);
            if (loginResult == LoginResult.Success)
            {
                // Successful authentication
                await this._signInManager.SignInAsync(user);
                this._session.SetString("UserName", user.Name); // Store user's name in session
                this._session.SetString("Department", user.Department);
                this._session.SetInt32("Role", user.UserTypeId);
                this._session.SetString("ProfilePictureUrl", user.ProfilePictureUrl);

                // Pass the role to the view
                ViewData["UserRole"] = user.UserTypeId;

                return RedirectToAction("Index", "Home"); // Redirect to home
            }
            else
            {
                // Authentication failed
                TempData["ErrorMessage"] = "Incorrect UserId or Password";
                return View();
            }
        }




        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserViewModel model)
        {
            // Check if the model is null
            if (model == null)
            {
                TempData["ErrorMessage"] = "Model is null.";
                return View();
            }

            // Handle file upload if a profile picture is provided
            if (model.ProfilePicture != null && model.ProfilePicture.Length > 0)
            {
                try
                {
                    // Ensure _webHostEnvironment is not null
                    if (_webHostEnvironment == null)
                    {
                        TempData["ErrorMessage"] = "Web Host Environment is not available.";
                        return View();
                    }

                    // Generate a unique file name
                    var fileName = Path.GetFileNameWithoutExtension(model.ProfilePicture.FileName);
                    var extension = Path.GetExtension(model.ProfilePicture.FileName);
                    fileName = $"{fileName}_{Guid.NewGuid()}{extension}"; // Append GUID to avoid name conflicts

                    // Define the path where the file will be saved
                    var directoryPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");

                    // Ensure the directory exists
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath); // Create folder if not exists
                    }

                    var filePath = Path.Combine(directoryPath, fileName);

                    // Save the file
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ProfilePicture.CopyToAsync(stream);
                    }

                    // Save the relative path to the database
                    model.ProfilePictureUrl = "/uploads/" + fileName;
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Error while uploading file: {ex.Message}";
                    return View();
                }
            }
            else
            {
                TempData["ErrorMessage"] = "No profile picture uploaded.";
                return View();
            }

            try
            {
                // Now pass the model to the service layer
                _userService.AddUser(model);  // This should work if model is correctly populated
                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }






        [AllowAnonymous]
        public async Task<IActionResult> SignOutUser()
        {
            await this._signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        //public IActionResult LoadUsers()
        //{
        //    var users = _userService.GetAllUsers();
        //    ViewBag.UserRole = HttpContext.Session.GetInt32("Role") ?? 0;
        //    ViewBag.UserCount = users.Count();
        //    return PartialView("/Views/Body/_Users.cshtml", users);



        public IActionResult LoadUsers(int page = 1)
        {
            // Fetch all users
            var users = _userService.GetAllUsers();

            // Convert to IPagedList for pagination
            int pageSize = 10;  // Number of users per page
            var pagedUsers = users.ToPagedList(page, pageSize);  // Ensure this is IPagedList

            // Add necessary data to ViewBag
            ViewBag.UserRole = HttpContext.Session.GetInt32("Role") ?? 0;
            ViewBag.UserCount = users.Count();

            // Return the paginated users to the partial view
            return PartialView("/Views/Body/_Users.cshtml", users.ToPagedList(page, 10));
        }

        [HttpGet]
        public IActionResult LoadFacilities(int page =1)
        {
            /*ViewBag.CurrentView = "Facilities"; // Set the current view flag
            return PartialView("/Views/Body/_Facilities.cshtml");*/

            try
            {
                ViewBag.CurrentView = "Facilities";
                var facilities = _facilityService.GetFacilities();

                int pageSize = 8; 
                var pagedUsers = facilities.ToPagedList(page, pageSize);

                return PartialView("/Views/Body/_Facilities.cshtml", facilities.ToPagedList(page, 8));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in LoadFacilities: {ex.Message}");
                return StatusCode(500, "Failed to load facilities");
            }
        }

        public IActionResult ViewFacility()
        {
            /*var facility = _facilityService.GetFacilityByIdService(id);

            if (facility == null)
            {
                return NotFound("Facility Not Found");
            }
*/
            //return PartialView("/Views/Body/_SpecificFacility.cshtml", facility);
            return PartialView("/Views/Body/_SpecificFacility.cshtml");
        }


        [HttpGet]
        public IActionResult LoadAddFacilities(bool isEdit = false)
        {
            try
            {
                // Set ViewBag based on whether it's an Add or Edit operation
                ViewBag.CurrentView = isEdit ? "EditFacility" : "AddFacility";
                ViewBag.ShowAddFacilities = true;

                // If editing, you can retrieve facility data from the database
                // Example: if (isEdit) { var facility = GetFacility(id); return PartialView("_AddFacilities", facility); }

                return PartialView("/Views/Body/_AddFacilities.cshtml");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in LoadAddFacilities: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = _userService.GetAllUsers(); // Fetch the list of users
            /*ViewBag.Users = users; */// Store it in ViewBag to make it accessible in the view/layout
            return View(users);
        }




        //[HttpGet]
        //public IActionResult Index()
        //{
        //    var users = _userService.GetAllUsers().ToList(); // Fetch users from the database
        //    var userCount = users.Count; // Get the count of users
        //    ViewBag.UserCount = userCount; // Pass the count to the view
        //    return View(users); // Return the view with users
        //}


        [HttpPost]
        [Route("Account/DeleteUser")]
        public IActionResult DeleteUser(int id)
        {
            bool result = _userService.DeleteUserById(id);
            if (result)
            {
                TempData["SuccessMessage"] = "User deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete user. User might not exist or there was an error.";
            }
            return Redirect(Request.Headers["Referer"].ToString());
        }














        [HttpPost]
        [Route("Account/UpdateUser")]
        public async Task<IActionResult> UpdateUser(string userId, string userName, string department, int userTypeId, string? password, IFormFile profilePicture)
        {
            var user = _userService.GetUserById(userId);
            if (user == null) return Json(new { success = false, message = "User not found." });

            // Update name if provided
            if (!string.IsNullOrEmpty(userName))
                user.Name = userName;

            // Update department if provided
            if (!string.IsNullOrEmpty(department))
                user.Department = department;

            // Update UserTypeId only if it's a valid role
            if (userTypeId == 1 || userTypeId == 2 || userTypeId == 3)
                user.UserTypeId = userTypeId;
            else
                return Redirect(Request.Headers["Referer"].ToString());

            // Update password if provided
            if (!string.IsNullOrEmpty(password))
                user.Password = PasswordManager.EncryptPassword(password);

            // Handle profile picture upload
            if (profilePicture != null && profilePicture.Length > 0)
            {
                try
                {
                    var fileName = Path.GetFileNameWithoutExtension(profilePicture.FileName);
                    var extension = Path.GetExtension(profilePicture.FileName);
                    fileName = $"{fileName}_{Guid.NewGuid()}{extension}";

                    var directoryPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");

                    if (!Directory.Exists(directoryPath))
                        Directory.CreateDirectory(directoryPath);

                    var filePath = Path.Combine(directoryPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await profilePicture.CopyToAsync(stream);
                    }

                    user.ProfilePictureUrl = "/uploads/" + fileName; // Save relative path
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Error while uploading profile picture: " + ex.Message });
                }
            }

            // Update the user in the database
            _userService.UpdateUser(user);

            return Redirect(Request.Headers["Referer"].ToString());
        }











        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateUser(UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Validation error occurred.";
                return Redirect(Request.Headers["Referer"].ToString());
            }

            // Handle file upload (same as your existing implementation)
            if (model.ProfilePicture != null && model.ProfilePicture.Length > 0)
            {
                try
                {
                    if (_webHostEnvironment == null)
                    {
                        TempData["ErrorMessage"] = "Server error: Unable to upload profile picture.";
                        return Redirect(Request.Headers["Referer"].ToString());
                    }

                    var fileName = Path.GetFileNameWithoutExtension(model.ProfilePicture.FileName);
                    var extension = Path.GetExtension(model.ProfilePicture.FileName);
                    fileName = $"{fileName}_{Guid.NewGuid()}{extension}";

                    var uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }
                    var filePath = Path.Combine(uploadPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ProfilePicture.CopyToAsync(stream);
                    }

                    model.ProfilePictureUrl = $"/uploads/{fileName}";
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Error during file upload: {ex.Message}";
                    return Redirect(Request.Headers["Referer"].ToString());
                }
            }

            try
            {
                _userService.AddUser(model);
                TempData["SuccessMessage"] = "User created successfully!";

                // Redirect to the previous page (or a specific page if "Referer" is unavailable)
                return Redirect(Request.Headers["Referer"].ToString() ?? Url.Action("Users", "Account"));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return Redirect(Request.Headers["Referer"].ToString());
            }
        }





        public IActionResult Return()
        {
            return PartialView("/Views/Body/Users.cshtml");
        }

    }



}
