using ClassLibraryDataBaseConnection;
using HospitalManagement.Models;
using HospitalManagement.ViewModels;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.Data.SqlClient;
using System.Buffers;
using System.Data;

namespace HospitalManagement.Repository
{
    public class ReceptionistRepository : IReceptionistRepository
    {

        //field 
        private readonly string connectionString;

        //DI
        public ReceptionistRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("ConnStringMVC");
        }


        #region list all paitent (get all paients)
        public IEnumerable<Patient> GetAllPatients()
        {
            List<Patient> patients = new List<Patient>();
            using (SqlConnection connection = ConnectionManager.OpenConnection(connectionString))
            {

                if (connection == null)
                    return patients;

                using (SqlCommand cmd = new SqlCommand("sp_GetAllPatients", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Patient patient = new Patient();
                            patient.PatientId = Convert.ToInt32(reader["patient_id"]);
                            patient.PatientName = Convert.ToString(reader["patient_name"]);
                            patient.Dob = reader["dob"] == DBNull.Value ? null : Convert.ToDateTime(reader["dob"]);
                            patient.Gender = Convert.ToString(reader["gender"]);
                            patient.Contact = Convert.ToString(reader["contact"]);
                            patient.Email = Convert.ToString(reader["email"]);
                            patient.Address = Convert.ToString(reader["address"]);
                            patient.CreatedAt = reader["created_at"] == DBNull.Value ? null : Convert.ToDateTime(reader["created_at"]);

                            patients.Add(patient);

                        }

                    }
                }
                return patients;

            }
        }

       
        #endregion

        #region adding paient
        public void InsertPatient(Patient patient)
        {
            using (SqlConnection connection = ConnectionManager.OpenConnection(connectionString))
            {
                if (connection != null)
                {
                    // Create command and pass stored procedure name
                    SqlCommand cmd = new SqlCommand("sp_InsertPatient", connection);

                    // Specify that we are calling a Stored Procedure
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Pass parameters to stored procedure
                    cmd.Parameters.AddWithValue("@patient_name", patient.PatientName);
                    cmd.Parameters.AddWithValue("@dob", patient.Dob);
                    cmd.Parameters.AddWithValue("@gender", patient.Gender);
                    cmd.Parameters.AddWithValue("@contact", patient.Contact);
                    cmd.Parameters.AddWithValue("@email", patient.Email);
                    cmd.Parameters.AddWithValue("@address", patient.Address);

                    // Execute insert command
                    cmd.ExecuteNonQuery();
                }

                // Close the connection
                connection.Close();
            }
        }



        #endregion
        
        #region search patient
        public IEnumerable<Patient> SearchPatients(string searchValue)
        {
            List<Patient> patients = new List<Patient>();

            using (SqlConnection connection = ConnectionManager.OpenConnection(connectionString))
            {
                if (connection != null)
                {
                    SqlCommand cmd = new SqlCommand("sp_SearchPatients", connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@searchValue",
     string.IsNullOrWhiteSpace(searchValue) ? (object)DBNull.Value : searchValue);


                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Patient patient = new Patient();

                        patient.PatientId = Convert.ToInt32(reader["patient_id"]);
                        patient.PatientName = Convert.ToString(reader["patient_name"]);
                        patient.Dob = reader["dob"] == DBNull.Value ? null : Convert.ToDateTime(reader["dob"]);
                        patient.Gender = Convert.ToString(reader["gender"]);
                        patient.Contact = Convert.ToString(reader["contact"]);
                        patient.Email = Convert.ToString(reader["email"]);
                        patient.Address = Convert.ToString(reader["address"]);
                        patient.CreatedAt = reader["created_at"] == DBNull.Value ? null : Convert.ToDateTime(reader["created_at"]);

                        patients.Add(patient);
                    }

                    reader.Close();
                }

                connection.Close();
            }

            return patients;
        }



        #endregion


        #region get paient by id(load data for edit)
        public Patient GetPatientById(int patientId)

             
        {
            Patient patient = null;
            using (SqlConnection connection = ConnectionManager.OpenConnection(connectionString))
            {
                if (connection != null)
                {
                    SqlCommand cmd = new SqlCommand("sp_GetPatientById", connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@patient_id", patientId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        patient = new Patient
                        {
                            PatientId = Convert.ToInt32(reader["patient_id"]),
                            PatientName = Convert.ToString(reader["patient_name"]),
                            Dob = reader["dob"] == DBNull.Value ? null : Convert.ToDateTime(reader["dob"]),
                            Gender = Convert.ToString(reader["gender"]),
                            Contact = Convert.ToString(reader["contact"]),
                            Email = Convert.ToString(reader["email"]),
                            Address = Convert.ToString(reader["address"]),
                            CreatedAt = reader["created_at"] == DBNull.Value ? null : Convert.ToDateTime(reader["created_at"])
                        };
                    }

                    reader.Close();
                }

                connection.Close();
            }

            return patient;
        }

#endregion
        

        #region update patient
        public void UpdatePatient(Patient patient)
        {
            using (SqlConnection connection = ConnectionManager.OpenConnection(connectionString))
            {
                if (connection != null)
                {
                    SqlCommand cmd = new SqlCommand("sp_UpdatePatient", connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@patient_id", patient.PatientId);
                    cmd.Parameters.AddWithValue("@patient_name", patient.PatientName);
                    cmd.Parameters.AddWithValue("@dob", patient.Dob ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@gender", patient.Gender);
                    cmd.Parameters.AddWithValue("@contact", patient.Contact);
                    cmd.Parameters.AddWithValue("@email", patient.Email ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@address", patient.Address ?? (object)DBNull.Value);

                    cmd.ExecuteNonQuery();
                }

                connection.Close();
            }
        }


        #endregion



        #region Book Appointment

        /*
         * PURPOSE:
         * --------
         * This method calls the stored procedure: sp_BookAppointment
         * 
         * It sends booking details to database.
         * The SP will:
         *   - Validate inputs
         *   - Prevent past date
         *   - Prevent past time
         *   - Prevent double booking
         *   - Insert appointment safely
         *   - Return success or failure
         * 
         * RETURN TYPE:
         * ------------
         * (bool success, string message)
         * 
         * success = true  -> Booking successful
         * success = false -> Booking failed
         */

        public (bool success, int appointmentId, string message) BookAppointment(AppointmentBookingViewModel model)
        {
            using (SqlConnection connection = ConnectionManager.OpenConnection(connectionString))
            {
                if (connection != null)
                {
                    SqlCommand cmd = new SqlCommand("sp_BookAppointment", connection);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@patient_id", model.PatientId);
                    cmd.Parameters.AddWithValue("@doctor_id", model.DoctorId);
                    cmd.Parameters.AddWithValue("@slot_id", model.SlotId);
                    cmd.Parameters.AddWithValue("@appointment_date", model.AppointmentDate);
                    cmd.Parameters.AddWithValue("@created_by", model.CreatedBy);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            bool success = Convert.ToInt32(reader["success"]) == 1;

                            string message = Convert.ToString(reader["message"]);

                            int appointmentId = Convert.ToInt32(reader["appointment_id"]);

                            return (success, appointmentId, message);
                        }
                    }
                }
            }

            return (false, 0, "Unexpected error occurred");
        }

        #endregion


        #region Get Available Slots

        /*
         * PURPOSE:
         * --------
         * Calls sp_GetAvailableSlots
         * Returns only free slots for selected doctor and date.
         */

        public IEnumerable<SlotMaster> GetAvailableSlots(int doctorId, DateTime appointmentDate)
        {
            List<SlotMaster> slots = new List<SlotMaster>();

            using (SqlConnection connection = ConnectionManager.OpenConnection(connectionString))
            {
                if (connection != null)
                {
                    SqlCommand cmd = new SqlCommand("sp_GetAvailableSlots", connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@doctor_id", doctorId);
                    cmd.Parameters.AddWithValue("@appointment_date", appointmentDate);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SlotMaster slot = new SlotMaster();

                            slot.SlotId = Convert.ToInt32(reader["slot_id"]);
                            slot.TokenNo = Convert.ToInt32(reader["token_no"]);
                            slot.SlotTime = (TimeSpan)reader["slot_time"];

                            slots.Add(slot);
                        }
                    }
                }
            }

            return slots;
        }


        #endregion


        #region Get All Doctors

        public IEnumerable<Doctor> GetAllDoctors()
        {
            List<Doctor> doctors = new List<Doctor>();

            using (SqlConnection connection = ConnectionManager.OpenConnection(connectionString))
            {
                if (connection != null)
                {
                    SqlCommand cmd = new SqlCommand("sp_GetAllDoctors", connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Doctor doctor = new Doctor();

                            doctor.DoctorId = Convert.ToInt32(reader["doctor_id"]);
                            doctor.UserId = Convert.ToInt32(reader["user_id"]);
                            doctor.FullName = Convert.ToString(reader["full_name"]);
                            doctor.SpecializationName = Convert.ToString(reader["specialization_name"]);

                            doctors.Add(doctor);
                        }
                    }
                }
            }

            return doctors;
        }



        #endregion

        #region
        public bool BillExists(int appointmentId)
        {
           
        
            using SqlConnection connection = ConnectionManager.OpenConnection(connectionString);

            if (connection == null)
                return false;

            SqlCommand cmd = new SqlCommand("sp_CheckConsultationBill", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@appointment_id", appointmentId);

            SqlDataReader reader = cmd.ExecuteReader();

            bool exists = reader.HasRows;

            reader.Close();
            connection.Close();

            return exists;
        }

        
        #endregion
    #region
        public ConsultationBillViewModel GetBillDetails(int appointmentId)
        {
            ConsultationBillViewModel bill = new ConsultationBillViewModel();

            using SqlConnection connection = ConnectionManager.OpenConnection(connectionString);

            if (connection == null)
                return bill;

            SqlCommand cmd = new SqlCommand("sp_GetConsultationBillDetails", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@appointment_id", appointmentId);

            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                bill.AppointmentId = Convert.ToInt32(reader["appointment_id"]);
                bill.AppointmentDate = Convert.ToDateTime(reader["appointment_date"]);
                bill.PatientName = Convert.ToString(reader["patient_name"]);
                bill.Contact = Convert.ToString(reader["contact"]);
                bill.DoctorName = Convert.ToString(reader["doctor_name"]);
                bill.Specialization = Convert.ToString(reader["specialization_name"]);
                bill.ConsultationFee = Convert.ToDecimal(reader["consultation_fee"]);
            }

            reader.Close();
            connection.Close();

            return bill;
        }
        
        #endregion

        #region

        public void InsertBill(int appointmentId, decimal totalAmount, int statusId)
        {
            using SqlConnection connection = ConnectionManager.OpenConnection(connectionString);

            if (connection == null)
                return;

            SqlCommand cmd = new SqlCommand("sp_InsertConsultationBill", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@appointment_id", appointmentId);
            cmd.Parameters.AddWithValue("@total_amount", totalAmount);
            cmd.Parameters.AddWithValue("@status_id", statusId);

            cmd.ExecuteNonQuery();

            connection.Close();
        }
        #endregion




    }




}


