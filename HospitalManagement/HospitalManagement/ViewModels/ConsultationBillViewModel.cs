namespace HospitalManagement.ViewModels
{
    public class ConsultationBillViewModel
    {
        public int AppointmentId { get; set; }
        public DateTime AppointmentDate { get; set; }

        public string PatientName { get; set; }
        public string Contact { get; set; }

        public string DoctorName { get; set; }
        public string Specialization { get; set; }

        public decimal ConsultationFee { get; set; }

    }
}
