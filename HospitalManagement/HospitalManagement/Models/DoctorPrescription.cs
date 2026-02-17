namespace HospitalManagement.Models
{
    public class DoctorPrescription
    {
        public int ConsultationId { get; set; }
        public int MedicineId { get; set; }
        public int Frequency { get; set; }
        public int DurationDays { get; set; }
    }
}
