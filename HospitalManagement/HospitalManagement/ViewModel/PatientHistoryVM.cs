namespace HospitalManagement.ViewModel
{
    public class PatientHistoryVM
    {
        public int ConsultationId { get; set; }
        public DateTime ConsultationDate { get; set; }

        public string Symptoms { get; set; }
        public string Diagnosis { get; set; }
        public string DoctorNotes { get; set; }

        public string MedicineName { get; set; }
        public int Frequency { get; set; }
        public int DurationDays { get; set; }
        public int Quantity { get; set; }

        public string LabTestName { get; set; }
        public string LabResult { get; set; }
        public DateTime? LabTestDate { get; set; }
    }
}
