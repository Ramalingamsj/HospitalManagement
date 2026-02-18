using HospitalManagement.Models;
using HospitalManagement.ViewModel;

namespace HospitalManagement.Repository
{
    public interface IPharmacyRepository
    {
        public IEnumerable<PharmacyPendingVM> GetPendingMedicines();
        public void UpdateStock(int id, int stock);
        public void AddMedicine(Medicine m);
        public List<Medicine> GetMedicines();
        public List<BillList> GetAllBills();
        public void IssueMedicine(int patientMedicineId, int pharmacyUserId);
        public string GenerateMedicineBill(int consultationId);
        public bool BillExists(int consultationId);
        public string GetPatientName(int consultationId);
        public decimal GetBillAmount(int consultationId);
        public bool HasPendingMedicines(int consultationId);
    }
}
