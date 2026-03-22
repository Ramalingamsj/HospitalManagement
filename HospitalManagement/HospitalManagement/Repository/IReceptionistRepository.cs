using HospitalManagement.Models;
using HospitalManagement.ViewModels;
using System.Numerics;

namespace HospitalManagement.Repository
{
    public interface IReceptionistRepository
    {
        // Get all patients (for grid)

        public IEnumerable<Patient> GetAllPatients();

        // Insert new patient
        void InsertPatient(Patient patient);

        // search paient
        IEnumerable<Patient> SearchPatients( string searchValue);


        //getpaientid(load the paient for edit)Patient GetPatientById(int patientId);

        Patient GetPatientById(int patientId);
        // update paient
        void UpdatePatient(Patient patient);


        /*
   * BOOK APPOINTMENT
     * ----------------
     * This method will call stored procedure to book appointment.
     * It returns:
     *   success = true/false
    *   message = result message from database
      */
        (bool success, int appointmentId,string message) BookAppointment(AppointmentBookingViewModel model);

        // get avalible slots
        IEnumerable<SlotMaster> GetAvailableSlots(int doctorId, DateTime appointmentDate);

        //get all docotrs for the drop in book appointment
        IEnumerable<Doctor> GetAllDoctors();


        bool BillExists(int appointmentId);

        ConsultationBillViewModel GetBillDetails(int appointmentId);

        void InsertBill(int appointmentId, decimal totalAmount, int statusId);



    }
}
