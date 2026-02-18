namespace HospitalManagement.ViewModel
{
    public class PharmacyPendingVM
    {
        public int PatientMedicineId { get; set; }
        public int ConsultationId { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public string MedicineName { get; set; }
        public string MedicineType { get; set; }
        public int StatusId { get; set; }
        public int Frequency { get; set; }
        public int DurationDays { get; set; }
        public int Stock { get; set; }
    }
}
