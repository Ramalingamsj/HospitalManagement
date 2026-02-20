using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.ViewModels
{
    public class AppointmentListViewModel
    {
        /*
     * VIEW MODEL
     * ----------
     * Used for displaying appointment list.
     * Combines multiple tables into one display model.
     */
        
            public int AppointmentId { get; set; }

            public string PatientName { get; set; }

            public string DoctorName { get; set; }

            public string SlotTime { get; set; }

            public DateTime AppointmentDate { get; set; }

            public string StatusName { get; set; }
    }
}



