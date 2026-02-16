using HospitalManagement.ViewModels;

namespace HospitalManagement.Repository
{
    public interface IDoctorRepository
    {
        public int GetDoctorIdByUserId(int userId);
        List<DoctorAppointmentVM> GetTodayAppointments(int doctorId);
        public void AddConsultation(ConsultationVM model);
    }
}
