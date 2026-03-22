namespace HospitalManagement.Models
{
    public class SlotMaster
    {
        public int SlotId { get; set; }

        // Token number (1–30)
        public int TokenNo { get; set; }

        // Time of slot (09:00, 09:15 etc.)
        public TimeSpan SlotTime { get; set; }
    }
}
