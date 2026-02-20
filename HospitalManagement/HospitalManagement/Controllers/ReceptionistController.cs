using HospitalManagement.Models;
using HospitalManagement.Service;
using HospitalManagement.ViewModels;
using Humanizer;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Controllers
{
    public class ReceptionistController : Controller

    {

        // field
        private readonly IReceptionistService _receptionistService;

        // DI
        public ReceptionistController(IReceptionistService receptionistService)
        {
            _receptionistService = receptionistService;
        }

        #region get all paients
        // get all paients
        [HttpGet]

        public IActionResult Index()
        {
            // Get all patients from service
            List<Patient> patients = _receptionistService.GetAllPatients().ToList();
            
            // Send patient list to dropdown (ViewBag)
            ViewBag.Patients = patients;

            // Send doctor list to dropdown
            ViewBag.Doctors = _receptionistService.GetAllDoctors();





            return View(patients);
        }
        #endregion

        #region  create patient     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult InsertPatient(PatientCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false });
            }

            Patient patient = new Patient
            {
                PatientName = model.PatientName,
                Gender = model.Gender,
                Dob = model.Dob,
                Contact = model.Contact,
                Email = model.Email,
                Address = model.Address
            };

            _receptionistService.InsertPatient(patient);

            return Json(new { success = true });
        }

        #endregion

        #region Search Patients

        [HttpGet]
        public JsonResult SearchPatients(string searchValue)
        {
            var patients = _receptionistService.SearchPatients(searchValue).ToList();
            return Json(patients);
        }

        #endregion

        #region Get Patient By Id (for edit) 
        
        [HttpGet]
       
        public JsonResult Edit(int id)
        {
            var patient = _receptionistService.GetPatientById(id);
            return Json(patient);
        }
        #endregion

        #region Update Patient
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Edit(Patient patient)
        {
            if (ModelState.IsValid)
            {
                _receptionistService.UpdatePatient(patient);
                return Json(new { success = true });
            }

            return Json(new { success = false, message = "Invalid data" });
        }
        #endregion


        #region Book Appointment

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult BookAppointment(AppointmentBookingViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = _receptionistService.BookAppointment(model);

                return Json(new
                {
                    success = result.success,
                    message = result.message,
                    appointmentId = result.appointmentId
                });
            }

            return Json(new
            {
                success = false,
                message = "Invalid booking data",
                appointmentId = 0
            });
        }


        #endregion

        #region Get Available Slots

        [HttpGet]
        public JsonResult GetAvailableSlots(int doctorId, DateTime? appointmentDate)
        {
            if (!appointmentDate.HasValue)
            {
                return Json(new { success = false, message = "Please select appointment date" });
            }

            var slots = _receptionistService.GetAvailableSlots(doctorId, appointmentDate.Value);

            return Json(slots);
        }


        #endregion



        #region bill 

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GenerateConsultationBill(int appointmentId)
        {
            bool exists = _receptionistService.BillExists(appointmentId);

            ConsultationBillViewModel billDetails;

            if (!exists)
            {
                billDetails = _receptionistService.GetConsultationBillDetails(appointmentId);

                _receptionistService.InsertConsultationBill(
                    appointmentId,
                    billDetails.ConsultationFee,
                    5 // Paid
                );
            }

            billDetails = _receptionistService.GetConsultationBillDetails(appointmentId);

            return Json(new
            {
                success = true,
                exists = exists,
                bill = billDetails
            });
        }

        #endregion







    }
}
