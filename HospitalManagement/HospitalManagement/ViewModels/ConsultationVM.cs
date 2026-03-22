using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.ViewModels
{
    public class ConsultationVM
    {
        public int AppointmentId { get; set; }
        [Required]
        public string Symptoms { get; set; }
        [Required]
        public string Diagnosis { get; set; }
        [Required]
        public string DoctorNotes { get; set; }
    }
}
