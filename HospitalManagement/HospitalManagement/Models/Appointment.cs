namespace HospitalManagement.Models
{
    public class Appointment
    {
        /*
     * ENTITY MODEL
     * ------------
     * This class represents the Appointment table exactly.
     * It mirrors the database structure.
     * We use this when retrieving full appointment records.
     */

        // Primary Key
        public int AppointmentId { get; set; }

        // Foreign Keys
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public int SlotId { get; set; }

        // Appointment Date (date only)
        public DateTime AppointmentDate { get; set; }

        // Status (Scheduled, Completed, etc.)
        public int StatusId { get; set; }

        // User who created appointment (Receptionist)
        public int CreatedBy { get; set; }

        // Automatically inserted by database
        public DateTime CreatedAt { get; set; }
    }
}

