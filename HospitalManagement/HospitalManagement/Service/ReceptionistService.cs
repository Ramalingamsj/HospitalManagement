using HospitalManagement.Models;
using HospitalManagement.Repository;
using HospitalManagement.ViewModels;

namespace HospitalManagement.Service
{
    public class ReceptionistService : IReceptionistService

    {
        //field
        private readonly IReceptionistRepository _receptionistRepository;

        // DI
        public ReceptionistService(IReceptionistRepository receptionistRepository)
        {
            _receptionistRepository = receptionistRepository;
        }
        //get all patients

        public IEnumerable<Patient> GetAllPatients()
        {
            return _receptionistRepository.GetAllPatients();
        }


        // addpaient
        public void InsertPatient(Patient patient)
        {
            _receptionistRepository.InsertPatient(patient);
        }
        //search patient by name or contact
        public IEnumerable<Patient> SearchPatients(string searchValue)
        {
            return _receptionistRepository.SearchPatients(searchValue);
        }
        //get patient by id (load the patient for edit)
        public Patient GetPatientById(int patientId)
        {
            return _receptionistRepository.GetPatientById(patientId);
        }
        //update patient
        public void UpdatePatient(Patient patient)
        {
            _receptionistRepository.UpdatePatient(patient);
        }
        // book appointment
        public (bool success, int appointmentId, string message) BookAppointment(AppointmentBookingViewModel model)
        {
            return _receptionistRepository.BookAppointment(model);
        }




        public IEnumerable<SlotMaster> GetAvailableSlots(int doctorId, DateTime appointmentDate)
        {
            return _receptionistRepository.GetAvailableSlots(doctorId, appointmentDate);
        }
        // get all dr for drop down for appointment booking
        public IEnumerable<Doctor> GetAllDoctors()
        {
            return _receptionistRepository.GetAllDoctors();

        }

        public bool BillExists(int appointmentId)
        {
            return _receptionistRepository.BillExists(appointmentId);
        }

        public ConsultationBillViewModel GetConsultationBillDetails(int appointmentId)
        {
            return _receptionistRepository.GetBillDetails(appointmentId);
        }

        public void InsertConsultationBill(int appointmentId, decimal totalAmount, int statusId)
        {
            _receptionistRepository.InsertBill(
        appointmentId,
        totalAmount,
        statusId
    );
        }

        
    }
}
