using HospitalManagement.Models;
using HospitalManagement.ViewModels;

namespace HospitalManagement.Service
{
    public interface ILabTechnicianService
    {
        List<LabPendingVM> GetPendingTests();
        void UpdateResult(int id, string result, int userId);
        public void GenerateLabBillService(int patientLabTestId);
        public List<LabBill> GetBills();

    }
}
