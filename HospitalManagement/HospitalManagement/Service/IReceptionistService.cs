using HospitalManagement.Models;
using HospitalManagement.ViewModels;

namespace HospitalManagement.Service
{
    public interface IReceptionistService
    {
        // get all patient
         public IEnumerable<Patient> GetAllPatients();

        // Insert new patient
        void InsertPatient(Patient patient);


        //search patient
        IEnumerable<Patient> SearchPatients(string searchValue);

        //get patient by id (load the patient for edit)
        Patient GetPatientById(int patientId);
        //update patient
        void UpdatePatient(Patient patient);

        /*
         * SERVICE LAYER METHOD
           * ---------------------
          * Receives booking data from Controller.
         * Passes it to Repository.
          */
        (bool success, int appointmentId, string message) BookAppointment(AppointmentBookingViewModel model);

        // get availble slot
        IEnumerable<SlotMaster> GetAvailableSlots(int doctorId, DateTime appointmentDate);

        //get all docotors
        IEnumerable<Doctor> GetAllDoctors();


        // ================= BILLING =================

        bool BillExists(int appointmentId);

        ConsultationBillViewModel GetConsultationBillDetails(int appointmentId);

        void InsertConsultationBill(int appointmentId, decimal totalAmount, int statusId);




    }
}
