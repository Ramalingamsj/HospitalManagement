using HospitalManagement.Models;
using HospitalManagement.ViewModels;

namespace HospitalManagement.Repository
{
    public interface IDoctorRepository
    {
        public int GetDoctorIdByUserId(int userId);
        List<DoctorAppointmentVM> GetTodayAppointments(int doctorId);
        public void AddPrescription(DoctorPrescription model);
        public List<LabTest> GetAllLabTests();
        public int AddConsultation(ConsultationVM model);
        public int GetConsultationIdByAppointment(int appointmentId);
        public List<Medicine> GetAllMedicines();
        public void AddLabTest(int consultationId, int labTestId);
    }
}
