using ClassLibraryADOConnectiion;
using HospitalManagement.Models;
using HospitalManagement.ViewModels;
using Microsoft.Data.SqlClient;
using System.Data;

namespace HospitalManagement.Repository
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly string connectionString;

        public DoctorRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("ConnStringMVC");
        }

        public int GetDoctorIdByUserId(int userId)
        {
            using SqlConnection con = ConnectionManager.OpenConnection(connectionString);
            using SqlCommand cmd = new SqlCommand("sp_GetDoctorIdByUserId", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;

            object result = cmd.ExecuteScalar();

            return result != null ? Convert.ToInt32(result) : 0;
        }
        
        public List<DoctorAppointmentVM> GetTodayAppointments(int doctorId)
        {
            List<DoctorAppointmentVM> list = new List<DoctorAppointmentVM>();

            using (SqlConnection con = ConnectionManager.OpenConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_GetTodayAppointmentsByDoctor", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DoctorId", doctorId);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new DoctorAppointmentVM
                    {
                        AppointmentId = Convert.ToInt32(reader["appointment_id"]),
                        PatientName = reader["patient_name"].ToString(),
                        Gender = reader["gender"].ToString(),
                        Contact = reader["contact"].ToString(),
                        SlotTime = (TimeSpan)reader["slot_time"],
                        TokenNo = Convert.ToInt32(reader["token_no"]),
                        AppointmentDate = Convert.ToDateTime(reader["appointment_date"]),
                        StatusName = reader["status_name"]?.ToString()
                    });
                }
            }

            return list;
        }
        public void AddPrescription(DoctorPrescription model)
        {
            using SqlConnection con = ConnectionManager.OpenConnection(connectionString);

            using SqlCommand cmd = new SqlCommand("sp_DoctorAddMedicine", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@ConsultationId", SqlDbType.Int).Value = model.ConsultationId;
            cmd.Parameters.Add("@MedicineId", SqlDbType.Int).Value = model.MedicineId;
            cmd.Parameters.Add("@Frequency", SqlDbType.Int).Value = model.Frequency;
            cmd.Parameters.Add("@DurationDays", SqlDbType.Int).Value = model.DurationDays;

            cmd.ExecuteNonQuery();
        }
        public int GetConsultationIdByAppointment(int appointmentId)
        {
            using SqlConnection con = ConnectionManager.OpenConnection(connectionString);

            SqlCommand cmd = new SqlCommand(
                "SELECT consultation_id FROM Consultation WHERE appointment_id=@id", con);

            cmd.Parameters.AddWithValue("@id", appointmentId);

            return Convert.ToInt32(cmd.ExecuteScalar());
        }
        public List<LabTest> GetAllLabTests()
        {
            List<LabTest> list = new List<LabTest>();

            using SqlConnection con = ConnectionManager.OpenConnection(connectionString);

            SqlCommand cmd = new SqlCommand("sp_GetAllLabTests", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new LabTest
                {
                    LabTestId = Convert.ToInt32(reader["labtest_id"]),
                    TestName = reader["test_name"].ToString()
                });
            }

            return list;
        }
        public void AddLabTest(int consultationId, int labTestId)
        {
            using SqlConnection con = ConnectionManager.OpenConnection(connectionString);

            using SqlCommand cmd = new SqlCommand("sp_DoctorAddLabTest", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@ConsultationId", SqlDbType.Int).Value = consultationId;
            cmd.Parameters.Add("@LabTestId", SqlDbType.Int).Value = labTestId;

            cmd.ExecuteNonQuery();
        }
        public List<Medicine> GetAllMedicines()
        {
            List<Medicine> list = new List<Medicine>();

            using SqlConnection con = ConnectionManager.OpenConnection(connectionString);

            SqlCommand cmd = new SqlCommand("SELECT medicine_id, medicine_name FROM Medicine", con);

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new Medicine
                {
                    MedicineId = Convert.ToInt32(reader["medicine_id"]),
                    MedicineName = reader["medicine_name"].ToString()
                });
            }

            return list;
        }
        public int AddConsultation(ConsultationVM model)
        {
            using SqlConnection con = ConnectionManager.OpenConnection(connectionString);

            using SqlCommand cmd = new SqlCommand("sp_AddConsultation", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@AppointmentId", SqlDbType.Int).Value = model.AppointmentId;
            cmd.Parameters.Add("@Symptoms", SqlDbType.VarChar).Value = model.Symptoms;
            cmd.Parameters.Add("@Diagnosis", SqlDbType.VarChar).Value = model.Diagnosis;
            cmd.Parameters.Add("@DoctorNotes", SqlDbType.VarChar).Value = model.DoctorNotes;

            return Convert.ToInt32(cmd.ExecuteScalar());
        }
    }
}
