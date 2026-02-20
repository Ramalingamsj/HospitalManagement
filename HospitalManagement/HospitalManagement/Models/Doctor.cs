namespace HospitalManagement.Models
{
    public class Doctor
    {
        public int DoctorId { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string SpecializationName { get; set; }
    }
}
