namespace HospitalManagement.ViewModels
{
    public class DoctorAppointmentVM
    {
        public int AppointmentId { get; set; }
        public string PatientName { get; set; }
        public string Gender { get; set; }
        public string Contact { get; set; }
        public TimeSpan SlotTime { get; set; }
        public int TokenNo { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string StatusName { get; set; }
    }
}
