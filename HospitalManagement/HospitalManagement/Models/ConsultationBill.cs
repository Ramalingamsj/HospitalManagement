namespace HospitalManagement.Models
{
    public class ConsultationBill
    {
        public int BillId { get; set; }
        public int AppointmentId { get; set; }
        public decimal TotalAmount { get; set; }
        public int StatusId { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
