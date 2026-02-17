namespace HospitalManagement.Models
{
    public class BillList
    {
        public int BillId { get; set; }
        public int ConsultationId { get; set; }
        public string PatientName { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
