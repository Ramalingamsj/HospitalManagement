using HospitalManagement.Models;
using HospitalManagement.ViewModel;

namespace HospitalManagement.Service
{
    public interface IPharmacyService
    {
        public IEnumerable<PharmacyPendingVM> GetPendingMedicinesService();
        public void IssueMedicineService(int patientMedicineId, int pharmacyUserId);
        public bool HasPendingMedicinesSercice(int consultationId);
        public void UpdateStockService(int id, int stock);
        public void AddMedicineService(Medicine m);
        public List<Medicine> GetMedicinesService();
        public string GenerateMedicineBillService(int consultationId);
        public bool BillExistsService(int id);
        public List<BillList> GetBillsService();
        public string GetPatientNameService(int consultationId);
        public decimal GetBillAmountService(int id);
    }
}
