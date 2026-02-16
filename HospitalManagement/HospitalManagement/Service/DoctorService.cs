using HospitalManagement.Repository;
using HospitalManagement.ViewModels;

namespace HospitalManagement.Service
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _repo;

        public DoctorService(IDoctorRepository repo)
        {
            _repo = repo;
        }

        public int DoctorIdByUserId(int userId)
        {
           return _repo.GetDoctorIdByUserId(userId);
        }

        public List<DoctorAppointmentVM> GetTodayAppointments(int doctorId)
        {
            return _repo.GetTodayAppointments(doctorId);
        }
        public void AddConsultation(ConsultationVM model)
        {
            _repo.AddConsultation(model);
        }

    }
}
