using HospitalManagement.ViewModels;

namespace HospitalManagement.Service
{
    public interface IDoctorService
    {
        public int DoctorIdByUserId(int userId);
        List<DoctorAppointmentVM> GetTodayAppointments(int doctorId);
        public void AddConsultation(ConsultationVM model);
    }
}
