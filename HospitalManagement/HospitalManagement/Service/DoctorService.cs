using HospitalManagement.Models;
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
        public int AddConsultationService(ConsultationVM model)
        {
            return _repo.AddConsultation(model);
        }
        public void AddPrescriptionService(DoctorPrescription model)
        {
            _repo.AddPrescription(model);
        }
        public void AddMedicine(AddMedicineVM model)
        {
            int consultationId = _repo.GetConsultationIdByAppointment(model.AppointmentId);

            DoctorPrescription p = new DoctorPrescription()
            {
                ConsultationId = consultationId,
                MedicineId = model.MedicineId,
                Frequency = model.Frequency,
                DurationDays = model.DurationDays
            };

            _repo.AddPrescription(p);
        }
        public List<Medicine> GetAllMedicines()
        {
            return _repo.GetAllMedicines();
        }
        public List<LabTest> GetAllLabTests()
        {
            return _repo.GetAllLabTests();
        }
        public void AddLabTest(int appointmentId, int labTestId)
        {
            int consultationId = _repo.GetConsultationIdByAppointment(appointmentId);

            _repo.AddLabTest(consultationId, labTestId);
        }
    }
}
