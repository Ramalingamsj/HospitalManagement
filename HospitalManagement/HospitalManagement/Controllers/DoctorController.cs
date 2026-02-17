
using HospitalManagement.Models;
using HospitalManagement.Service;
using HospitalManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

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

            ViewBag.Medicines = _service.GetAllMedicines();
            ViewBag.LabTests = _service.GetAllLabTests();   // ⭐ ADD THIS LINE

            return View(list);
        }
        [HttpPost]
        public IActionResult AddConsultation(ConsultationVM model)
        {
            try
            {
                int consultationId = _service.AddConsultationService(model);

                TempData["ConsultationAdded"] = true;
                TempData["ConsultationId"] = consultationId;

                return RedirectToAction("TodayAppointments");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("TodayAppointments");
            }
        }
        [HttpPost]

 
        public IActionResult AddMedicine(AddMedicineVM model)
        {
            try
            {
                _service.AddMedicine(model);
                TempData["Success"] = "Medicine added successfully";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("TodayAppointments");
        }
        [HttpPost]
        [HttpPost]
        public IActionResult AddLabTest(AddLabTest model)
        {
            try
            {
                _service.AddLabTest(model.AppointmentId, model.LabTestId);
                TempData["Success"] = "Lab test added successfully";
            }
            catch (SqlException ex) when (ex.Number == 50000)
            {
                TempData["Error"] = ex.Message;
            }
            catch
            {
                TempData["Error"] = "Something went wrong";
            }

            return RedirectToAction("TodayAppointments");
        }
    }
}
