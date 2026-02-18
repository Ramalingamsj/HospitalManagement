namespace HospitalManagement.Models
{
    public class LabBill
    {
        public int BillId { get; set; }
        public int ConsultationId { get; set; }
        public string PatientName { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
