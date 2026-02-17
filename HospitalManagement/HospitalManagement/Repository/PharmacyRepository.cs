using ClassLibraryADOConnectiion;
using HospitalManagement.Models;
using HospitalManagement.ViewModel;
using Microsoft.Data.SqlClient;
using System.Data;

namespace HospitalManagement.Repository
{
    public class PharmacyRepository : IPharmacyRepository
    {
        private readonly string connectionString;

        public PharmacyRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("ConnStringMVC");
        }
        public IEnumerable<PharmacyPendingVM> GetPendingMedicines()
        {
            List<PharmacyPendingVM> list = new List<PharmacyPendingVM>();

            using (SqlConnection connection = ConnectionManager.OpenConnection(connectionString))
            {
                if (connection != null)
                {
                    SqlCommand cmd = new SqlCommand("sp_Pharmacy_GetPendingMedicines", connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PharmacyPendingVM vm = new PharmacyPendingVM();

                            vm.PatientMedicineId = Convert.ToInt32(reader["patient_medicine_id"]);
                            vm.ConsultationId = Convert.ToInt32(reader["consultation_id"]);
                            vm.PatientId = Convert.ToInt32(reader["patient_id"]);
                            vm.PatientName = reader["patient_name"].ToString();
                            vm.MedicineName = reader["medicine_name"].ToString();
                            vm.MedicineType = reader["medicine_type"].ToString();
                            vm.StatusId = Convert.ToInt32(reader["status_id"]);

                            vm.Frequency = Convert.ToInt32(reader["frequency"]);   // ✅ NEW
                            vm.DurationDays = Convert.ToInt32(reader["duration_days"]);
                            vm.Stock = Convert.ToInt32(reader["stock_quantity"]);

                            list.Add(vm);
                        }
                    }

                    connection.Close();
                }
            }

            return list;
        }
        public bool HasPendingMedicines(int consultationId)
        {
            using SqlConnection con = ConnectionManager.OpenConnection(connectionString);

            SqlCommand cmd = new SqlCommand("sp_CheckPendingMedicines", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@consultation_id", consultationId);

            return (int)cmd.ExecuteScalar() > 0;
        }
        public void AddMedicine(Medicine m)
        {
            using SqlConnection con = ConnectionManager.OpenConnection(connectionString);

            SqlCommand cmd = new("sp_AddMedicine", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@name", m.MedicineName);
            cmd.Parameters.AddWithValue("@type", m.MedicineType);
            cmd.Parameters.AddWithValue("@desc", m.Description);
            cmd.Parameters.AddWithValue("@stock", m.StockQuantity);
            cmd.Parameters.AddWithValue("@price", m.Price);

            cmd.ExecuteNonQuery();
        }
        public void UpdateStock(int id, int stock)
        {
            using SqlConnection con = ConnectionManager.OpenConnection(connectionString);

            SqlCommand cmd = new("sp_UpdateMedicineStock", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@medicine_id", id);
            cmd.Parameters.AddWithValue("@stock", stock);

            cmd.ExecuteNonQuery();
        }
        public List<Medicine> GetMedicines()
        {
            List<Medicine> list = new();

            using SqlConnection con = ConnectionManager.OpenConnection(connectionString);

            SqlCommand cmd = new("sp_GetMedicines", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new Medicine
                {
                    MedicineId = Convert.ToInt32(reader["medicine_id"]),
                    MedicineName = reader["medicine_name"].ToString(),
                    MedicineType = reader["medicine_type"].ToString(),
                    Description = reader["description"].ToString(),
                    StockQuantity = Convert.ToInt32(reader["stock_quantity"]),
                    Price = Convert.ToDecimal(reader["price"])
                });
            }

            return list;
        }
        public string GenerateMedicineBill(int consultationId)
        {
            try
            {
                using SqlConnection con = ConnectionManager.OpenConnection(connectionString);

                SqlCommand cmd = new SqlCommand("sp_GenerateMedicineBill", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@consultation_id", consultationId);

                cmd.ExecuteNonQuery();

                return "SUCCESS";
            }
            catch (SqlException ex)
            {
                return ex.Message; // ← return error text instead
            }
        }
        public List<BillList> GetAllBills()
        {
            List<BillList> list = new();

            using SqlConnection con = ConnectionManager.OpenConnection(connectionString);

            SqlCommand cmd = new("sp_GetAllMedicineBills", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new BillList
                {
                    BillId = Convert.ToInt32(reader["bill_id"]),
                    ConsultationId = Convert.ToInt32(reader["consultation_id"]),
                    PatientName = reader["patient_name"].ToString(),
                    TotalAmount = Convert.ToDecimal(reader["total_amount"]),
                    Status = reader["status_name"].ToString(),
                    CreatedAt = Convert.ToDateTime(reader["created_at"])
                });
            }

            return list;
        }
        public decimal GetBillAmount(int consultationId)
        {
            using SqlConnection con = ConnectionManager.OpenConnection(connectionString);

            SqlCommand cmd = new SqlCommand(
                "SELECT total_amount FROM BillMed WHERE consultation_id=@id", con);

            cmd.Parameters.AddWithValue("@id", consultationId);

            var result = cmd.ExecuteScalar();

            return result == null ? 0 : Convert.ToDecimal(result);
        }
        public bool BillExists(int consultationId)
        {
            using SqlConnection con = ConnectionManager.OpenConnection(connectionString);

            SqlCommand cmd = new SqlCommand(
                "SELECT COUNT(*) FROM BillMed WHERE consultation_id=@id", con);

            cmd.Parameters.AddWithValue("@id", consultationId);

            return (int)cmd.ExecuteScalar() > 0;
        }
        public string GetPatientName(int consultationId)
        {
            using SqlConnection con = ConnectionManager.OpenConnection(connectionString);

            SqlCommand cmd = new SqlCommand(@"
        SELECT p.patient_name
        FROM Consultation c
        JOIN Appointment a ON c.appointment_id = a.appointment_id
        JOIN Patient p ON a.patient_id = p.patient_id
        WHERE c.consultation_id = @id", con);

            cmd.Parameters.AddWithValue("@id", consultationId);

            object result = cmd.ExecuteScalar();

            return result == null ? "Patient" : result.ToString();
        }
        public void IssueMedicine(int patientMedicineId, int pharmacyUserId)
        {
            using (SqlConnection connection = ConnectionManager.OpenConnection(connectionString))
            {
                if (connection != null)
                {
                    SqlCommand cmd = new SqlCommand("IssueMedicine", connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@patient_medicine_id", patientMedicineId);
                    cmd.Parameters.AddWithValue("@pharmacy_user_id", pharmacyUserId);

                    cmd.ExecuteNonQuery();

                    connection.Close();
                }
            }
        }
    }
}
