using HospitalManagement.Models;
using HospitalManagement.Repository;
using HospitalManagement.ViewModel;

namespace HospitalManagement.Service
{
    public class PharmacyService : IPharmacyService
    {
        private readonly IPharmacyRepository _pharmacyRepository;
        public PharmacyService(IPharmacyRepository pharmacyRepository)
        {
            _pharmacyRepository = pharmacyRepository;
        }
        public IEnumerable<PharmacyPendingVM> GetPendingMedicinesService()
        {
            return _pharmacyRepository.GetPendingMedicines();
        }
        public void IssueMedicineService(int patientMedicineId, int pharmacyUserId)
        {
            _pharmacyRepository.IssueMedicine(patientMedicineId, pharmacyUserId);
        }
        public bool HasPendingMedicinesSercice(int consultationId)
        {
            return _pharmacyRepository.HasPendingMedicines(consultationId);
        }
        public void UpdateStockService(int id, int stock)
        {
            _pharmacyRepository.UpdateStock(id, stock);
        }
        public void AddMedicineService(HospitalManagement.Models.Medicine m)
        {
            _pharmacyRepository.AddMedicine(m);
        }
        public List<Medicine> GetMedicinesService()
        {
            return _pharmacyRepository.GetMedicines();
        }
        public string GenerateMedicineBillService(int consultationId)
        {
            return _pharmacyRepository.GenerateMedicineBill(consultationId);
        }
        public bool BillExistsService(int id)
        {
            return _pharmacyRepository.BillExists(id);
        }
        public string GetPatientNameService(int consultationId)
        {
            return _pharmacyRepository.GetPatientName(consultationId);
        }
        public decimal GetBillAmountService(int id)
        {
            return _pharmacyRepository.GetBillAmount(id);
        }
        public List<BillList> GetBillsService()
        {
            return _pharmacyRepository.GetAllBills();
        }
    }
}
