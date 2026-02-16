
using HospitalManagement.Service;
using HospitalManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Controllers
{
    public class DoctorController : Controller
    {
        private readonly IDoctorService _service;

        public DoctorController(IDoctorService service)
        {
            _service = service;
        }

        public IActionResult TodayAppointments()
        {
            int userId = Convert.ToInt32(Request.Cookies["UserId"]);
            int doctorId = _service.DoctorIdByUserId(userId);


            var list = _service.GetTodayAppointments(doctorId);

            return View(list);
        }
        [HttpPost]
        public IActionResult AddConsultation(ConsultationVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _service.AddConsultation(model);

            TempData["Success"] = "Consultation added successfully.";

            return RedirectToAction("TodayAppointments");
        }
    }
}
