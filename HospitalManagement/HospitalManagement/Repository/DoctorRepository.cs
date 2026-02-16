using ClassLibraryADOConnectiion;
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
        public void AddConsultation(ConsultationVM model)
        {
            using SqlConnection con = ConnectionManager.OpenConnection(connectionString);
            {
                if (con != null)
                {
                    con.Open();
                    using SqlCommand cmd = new SqlCommand("sp_AddConsultation", con);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@AppointmentId", SqlDbType.Int).Value = model.AppointmentId;
                    cmd.Parameters.Add("@Symptoms", SqlDbType.VarChar).Value = model.Symptoms;
                    cmd.Parameters.Add("@Diagnosis", SqlDbType.VarChar).Value = model.Diagnosis;
                    cmd.Parameters.Add("@DoctorNotes", SqlDbType.VarChar).Value = model.DoctorNotes;
                    cmd.ExecuteNonQuery();
                }
            }
            con.Close();
        }
    }
}
