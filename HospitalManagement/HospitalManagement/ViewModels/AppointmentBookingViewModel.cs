using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.ViewModels
{
  
        
          //VIEW MODEL
        
          //Used only for Booking Appointment form.
          //It contains only the fields required from UI.
          //It does NOT contain appointment_id, created_at, status_id.
         
        public class AppointmentBookingViewModel
        {
            [Required(ErrorMessage = "Patient is required")]
            [Display(Name = "Patient")]
            public int PatientId { get; set; }

            [Required(ErrorMessage = "Doctor is required")]
            [Display(Name = "Doctor")]
            public int DoctorId { get; set; }

            [Required(ErrorMessage = "Please select a slot")]
            [Display(Name = "Time Slot")]
            public int SlotId { get; set; }

            [Required(ErrorMessage = "Appointment date is required")]
            [DataType(DataType.Date)]
            [Display(Name = "Appointment Date")]
            public DateTime AppointmentDate { get; set; }

            /*
             * This is set internally from logged-in user.
             * Not entered by receptionist manually.
             */
            public int CreatedBy { get; set; }
        }
    
}

