using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Models
{
    public class Patient
    {

        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public string Gender { get; set; }
        public DateTime? Dob { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        
        public DateTime? CreatedAt { get; set; }
        
    }
}
