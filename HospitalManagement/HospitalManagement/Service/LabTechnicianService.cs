using HospitalManagement.Models;
using HospitalManagement.Repository;
using HospitalManagement.ViewModels;

namespace HospitalManagement.Service
{
    public class LabTechnicianService : ILabTechnicianService
    {
        private readonly ILabTechnicianRepository _repo;

        public LabTechnicianService(ILabTechnicianRepository repo)
        {
            _repo = repo;
        }
        public void GenerateLabBillService(int patientLabTestId)
        {
            int consultationId = _repo.GetConsultationIdFromLabTest(patientLabTestId);

            _repo.GenerateLabBill(consultationId);
        }

        public List<LabPendingVM> GetPendingTests()
            =>_repo.GetPendingTests();

        public void UpdateResult(int id, string result, int userId)
            => _repo.UpdateResult(id, result, userId);
        public List<LabBill> GetBills()
    => _repo.GetBills();
    }
}
