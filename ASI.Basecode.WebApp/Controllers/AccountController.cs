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
                            IHttpContextAccessor httpContextAccessor,
                            ILoggerFactory loggerFactory,
                            IConfiguration configuration,
                            IMapper mapper,
                            IUserService userService,
                            TokenValidationParametersFactory tokenValidationParametersFactory,
                            TokenProviderOptionsFactory tokenProviderOptionsFactory) : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {
            this._sessionManager = new SessionManager(this._session);
            this._signInManager = signInManager;
            this._tokenProviderOptionsFactory = tokenProviderOptionsFactory;
            this._tokenValidationParametersFactory = tokenValidationParametersFactory;
            this._appConfiguration = configuration;
            this._userService = userService;
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





        /*
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

                        return RedirectToAction("Index", "Home"); // Redirect to home
                    }
                    else
                    {
                        // Authentication failed
                        TempData["ErrorMessage"] = "Incorrect UserId or Password";
                        return View();
                    }
                }*/


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
        public IActionResult Register(UserViewModel model)
        {
            try
            {
                _userService.AddUser(model);
                return RedirectToAction("Login", "Account");
            }
            catch (InvalidDataException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = Resources.Messages.Errors.ServerError;
            }
            return View();
        }


        [AllowAnonymous]
        public async Task<IActionResult> SignOutUser()
        {
            await this._signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        public IActionResult LoadUsers()
        {
            var users = _userService.GetAllUsers();
            ViewBag.UserCount = users.Count();
            return PartialView("/Views/Body/_Users.cshtml", users);
        }

        public IActionResult LoadFacilities()
        {
            ViewBag.CurrentView = "Facilities"; // Set the current view flag
            return PartialView("/Views/Body/_Facilities.cshtml");
        }

        public IActionResult ViewFacility()
        {
            return PartialView("/Views/Body/_SpecificFacility.cshtml");
        }
        /* public IActionResult LoadAddFacilities()
         {
             try
             {
                 ViewBag.CurrentView = "Facilities";
                 ViewBag.ShowAddFacilities = true;
                 return PartialView("/Views/Body/_AddFacilities.cshtml");
             }
             catch (Exception ex)
             {
                 // Log the exception (you can use a logging framework like Serilog or NLog)
                 Console.WriteLine($"Error in LoadAddFacilities: {ex.Message}");
                 return StatusCode(500, "Internal server error"); // You can customize the error response
             }
         }*/

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



        ///////////////////////////////////////////////////// TRIAL
        //public IActionResult GetUsers()
        //{
        //    var users = _userService.GetAllUsers();
        //    return View(users);
        //}
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
        public IActionResult DeleteUser(int id)
        {
            // Call the service to delete the user
            bool result = _userService.DeleteUserById(id);

            // Only return success if the deletion was actually successful
            if (result)
            {
                return Json(new { success = true, message = "User deleted successfully!" });
            }
            else
            {
                return Json(new { success = false, message = "Failed to delete user. User might not exist or there was an error." });
            }
        }
















        [HttpPost]
        [Route("Account/UpdateUser")]
        public IActionResult UpdateUser(string userId, string userName, string department, int userTypeId, string? password)
        {
            // Fetch user by userId as a string
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
            {
                user.UserTypeId = userTypeId;
            }
            else
            {
                return Json(new { success = false, message = "Invalid role ID." });
            }

            // Update password if provided
            if (!string.IsNullOrEmpty(password))
                user.Password = PasswordManager.EncryptPassword(password); // Use PasswordManager for encryption

            // Update the user in the database
            _userService.UpdateUser(user);

            return Json(new { success = true, message = "User updated successfully." });
        }





        [HttpPost]
        [AllowAnonymous]
        public IActionResult CreateUser([FromBody] UserViewModel model)
        {
            Console.WriteLine("Received data:");
            Console.WriteLine("UserId: " + model.UserId);
            Console.WriteLine("Name: " + model.Name);
            Console.WriteLine("Password: " + model.Password);
            Console.WriteLine("ConfirmPassword: " + model.ConfirmPassword); // Log ConfirmPassword
            Console.WriteLine("Department: " + model.Department);
            Console.WriteLine("Role: " + model.UserTypeId);

            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine("Validation error: " + error.ErrorMessage);
                }

                return Json(new { success = false, message = "Invalid form data." });
            }

            try
            {
                _userService.AddUserAdmin(model);
                return Json(new { success = true, message = "User added successfully!" });
            }
            catch (InvalidDataException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return Json(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return Json(new { success = false, message = "An error occurred while adding the user." });
            }
        }






    }



}
