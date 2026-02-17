namespace HospitalManagement.ViewModels
{
    public class AddMedicineVM
    {
        public int AppointmentId { get; set; }
        public int MedicineId { get; set; }
        public int Frequency { get; set; }
        public int DurationDays { get; set; }

        // display only
        public string MedicineName { get; set; }
    }
}
