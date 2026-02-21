using ClassLibraryADOConnectiion;
using HospitalManagement.Models;
using HospitalManagement.ViewModels;
using Microsoft.Data.SqlClient;
using System.Data;

namespace HospitalManagement.Repository
{
    public class LabTechnicianRepository : ILabTechnicianRepository
    {
        private readonly string connectionString;

        public LabTechnicianRepository(IConfiguration config)
        {
            connectionString = config.GetConnectionString("ConnStringMVC");
        }

        public List<LabPendingVM> GetPendingTests()
        {
            List<LabPendingVM> list = new();

            using SqlConnection con = new(connectionString);
            con.Open();

            SqlCommand cmd = new("sp_Lab_GetPendingTests", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new LabPendingVM
                {
                    PatientLabTestId = Convert.ToInt32(reader["patient_labtest_id"]),
                    ConsultationId = Convert.ToInt32(reader["consultation_id"]),
                    PatientName = reader["patient_name"].ToString(),
                    TestName = reader["test_name"].ToString(),
                    StatusId = Convert.ToInt32(reader["status_id"])
                });
            }

            return list;
        }
        public void GenerateLabBill(int consultationId)
        {
            using SqlConnection con = ConnectionManager.OpenConnection(connectionString);

            using SqlCommand cmd = new SqlCommand("sp_GenerateLabBill", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@ConsultationId", SqlDbType.Int).Value = consultationId;

            cmd.ExecuteNonQuery();
        }
        public int GetConsultationIdFromLabTest(int id)
        {
            using SqlConnection con = ConnectionManager.OpenConnection(connectionString);

            SqlCommand cmd = new SqlCommand(
                "SELECT consultation_id FROM PatientLabTest WHERE patient_labtest_id=@id",
                con);

            cmd.Parameters.AddWithValue("@id", id);

            return Convert.ToInt32(cmd.ExecuteScalar());
        }
        public List<LabBill> GetBills()
        {
            List<LabBill> list = new();

            using SqlConnection con = new(connectionString);
            con.Open();

            SqlCommand cmd = new("sp_GetAllLabBills", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new LabBill
                {
                    BillId = Convert.ToInt32(reader["bill_id"]),
                    ConsultationId = Convert.ToInt32(reader["consultation_id"]),
                    PatientName = reader["patient_name"].ToString(),
                    TotalAmount = Convert.ToDecimal(reader["total_amount"]),
                    CreatedAt = Convert.ToDateTime(reader["created_at"])
                });
            }

            return list;
        }
        public void UpdateResult(int id, string result, int userId)
        {
            using SqlConnection con = new(connectionString);
            con.Open();

            SqlCommand cmd = new("sp_Lab_UpdateResult", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@PatientLabTestId", id);
            cmd.Parameters.AddWithValue("@Result", result);
            cmd.Parameters.AddWithValue("@UserId", userId);

            cmd.ExecuteNonQuery();
        }
    }
}
