using HospitalManagement.Models;
using HospitalManagement.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HospitalManagement.Controllers
{

    public class LoginsController : Controller
    {
        
        private readonly IUserService _userService;
        //DI
        public LoginsController(IUserService userService)
        {
            _userService = userService;
        }
        public IActionResult Login()
        {
            ViewData["Title"] = "Login";
            return View(new LoginModel());
        }
        /// <summary>
        /// Post
        /// </summary>
        /// <returns></returns>
        //POST https://localhots:5580/Logins/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginModel loginVModel)
        {
            if (ModelState.IsValid)
            {
                //get Authenticated User
                var availableUser = _userService.LoginService(loginVModel.Username, loginVModel.Password);
                if (availableUser != null)
                {
                    //Stores in cookies
                    HttpContext.Session.SetString("UserId", availableUser.UserId.ToString());
                    HttpContext.Session.SetString("UserName", availableUser.Username);
                    HttpContext.Session.SetString("RoleId", availableUser.RoleId.ToString());

                    //Show Message on landing Page
                    TempData["SuccessMessage"] = $"Welcome,{availableUser.Username}!";

                    //Custom redirect
                    return RedirectToRoleBasedDashBoard(availableUser.RoleId);
                }
                TempData["ErrorMessage"] = "Invalid username or password.";
            }
            return View(new LoginModel());
        }


        private IActionResult RedirectToRoleBasedDashBoard(int roleId)
        {
            switch (roleId)
            {
                case 1: return RedirectToAction("TodayAppointments", "Doctor");
                case 2: return RedirectToAction("Index", "Receptionist");
                case 4: return RedirectToAction("PendingList", "Pharmacy");
                case 3: return RedirectToAction("Pending", "LabTechnician");

                default: return RedirectToAction("Login", "Logins");

            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
