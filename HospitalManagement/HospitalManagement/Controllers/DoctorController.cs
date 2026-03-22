
using HospitalManagement.Models;
using HospitalManagement.Service;
using HospitalManagement.ViewModel;
using HospitalManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Data.SqlClient;
using System.Data;

namespace HospitalManagement.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class DoctorController : Controller
    {
        private readonly string _con;
        private readonly IDoctorService _service;

        public DoctorController(IConfiguration config, IDoctorService service)
        {
            _con = config.GetConnectionString("ConnStringMVC");
            _service = service;
        }


        public override void OnActionExecuting(ActionExecutingContext context)
        {
            context.HttpContext.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            context.HttpContext.Response.Headers["Pragma"] = "no-cache";
            context.HttpContext.Response.Headers["Expires"] = "0";

            var userId = HttpContext.Session.GetString("UserId");
            var roleId = HttpContext.Session.GetString("RoleId");

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(roleId))
            {
                context.Result = RedirectToAction("Login", "Logins");
                return;
            }

            base.OnActionExecuting(context);
        }

        public IActionResult TodayAppointments()
        {
            var userIdString = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userIdString))
                return RedirectToAction("Login", "Logins");

            int userId = Convert.ToInt32(userIdString);

            int doctorId = _service.DoctorIdByUserId(userId);

            var list = _service.GetTodayAppointments(doctorId);

            ViewBag.Medicines = _service.GetAllMedicines();
            ViewBag.LabTests = _service.GetAllLabTests();

            return View(list);
        }
        [HttpPost]
        public IActionResult AddConsultation(ConsultationVM model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _service.AddConsultationService(model);

                    TempData["Success"] = "Consultation added successfully!";
                    return RedirectToAction("TodayAppointments");
                }
                catch (Exception)
                {
                    TempData["Error"] = "Something went wrong while saving consultation.";
                    return RedirectToAction("TodayAppointments");
                }
            }

            TempData["Error"] = "Invalid input data.";
            return RedirectToAction("TodayAppointments");
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
        public JsonResult GetResults(int appointmentId)
        {
            List<object> list = new();

            using SqlConnection con = new(_con);
            con.Open();
            using SqlCommand cmd = new("sp_GetLabResultsByAppId", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@appointment_id", appointmentId);

            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new
                {
                    labtest_name = reader["test_name"].ToString(),
                    result = reader["result"].ToString(),
                    test_date = reader["test_date"]?.ToString(),
                });
            }
            con.Close();
            return Json(list);
        }
        public List<PatientHistoryVM> GetPatientFullHistory(int patientId)
        {
            List<PatientHistoryVM> list = new();

            using SqlConnection con = new(_con);
            con.Open();

            using SqlCommand cmd = new("sp_GetPatientFullHistory", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@patient_id", patientId);

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new PatientHistoryVM
                {
                    ConsultationId = Convert.ToInt32(reader["consultation_id"]),
                    ConsultationDate = Convert.ToDateTime(reader["consultation_date"]),
                    Symptoms = reader["symptoms"]?.ToString(),
                    Diagnosis = reader["diagnosis"]?.ToString(),
                    DoctorNotes = reader["doctor_notes"]?.ToString(),

                    MedicineName = reader["medicine_name"]?.ToString(),
                    Frequency = reader["frequency"] == DBNull.Value ? 0 : Convert.ToInt32(reader["frequency"]),
                    DurationDays = reader["duration_days"] == DBNull.Value ? 0 : Convert.ToInt32(reader["duration_days"]),
                    Quantity = reader["quantity"] == DBNull.Value ? 0 : Convert.ToInt32(reader["quantity"]),

                    LabTestName = reader["test_name"]?.ToString(),
                    LabResult = reader["result"]?.ToString(),
                    LabTestDate = reader["test_date"] == DBNull.Value ? null : Convert.ToDateTime(reader["test_date"])
                });
            }

            return list;
        }
        public JsonResult GetPatientHistory(int patientId)
        {
            var history = GetPatientFullHistory(patientId);
            return Json(history);
        }
    }
}
