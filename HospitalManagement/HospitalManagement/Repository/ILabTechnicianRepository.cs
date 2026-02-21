using HospitalManagement.Models;
using HospitalManagement.ViewModels;

namespace HospitalManagement.Repository
{
    public interface ILabTechnicianRepository
    {
        List<LabPendingVM> GetPendingTests();
        void UpdateResult(int id, string result, int userId);
        void GenerateLabBill(int consultationId);
        public int GetConsultationIdFromLabTest(int id);
        public List<LabBill> GetBills();
    }
}
