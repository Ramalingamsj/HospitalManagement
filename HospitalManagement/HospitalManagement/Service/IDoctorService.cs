using HospitalManagement.Models;
using HospitalManagement.ViewModels;

namespace HospitalManagement.Service
{
    public interface IDoctorService
    {
        public int DoctorIdByUserId(int userId);
        List<DoctorAppointmentVM> GetTodayAppointments(int doctorId);
        public int AddConsultationService(ConsultationVM model);
        public void AddPrescriptionService(DoctorPrescription model);
        public void AddMedicine(AddMedicineVM model);
        public List<Medicine> GetAllMedicines();
        public void AddLabTest(int appointmentId, int labTestId);
        List<LabTest> GetAllLabTests();
    }
}
