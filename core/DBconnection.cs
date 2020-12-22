using System;
using System.Data;
using System.Data.SqlClient;
using pulse_2._0.collection;

namespace pulse_2._0.core
{
    class DBconnection
    {

        /*  Variable defenition */
        public const string defaultConnection = @"Data Source=(LocalDB)\MSSQLLocalDB;" + 
            @"AttachDbFilename='|DataDirectory|\Database1.mdf';Integrated Security=True";

        public SqlConnection sqlConnection = null;

        public static string RECORD_ADDTITION = "INSERT INTO [DATA] (Id, Время, Длительность, Пациент) VALUES(@Id, @Время, @Длительность, @Пациент)";
        public static string RECORD_UPDATE = "UPDATE [DATA] SET [Примечание] = @Примечание, [Время] = @Время, [Длительность] = @Длительность, [Пациент] = @Пациент WHERE Id = @Id";
        public static string PATIENTS_GET = "SELECT *, 'Choose' AS [Выбрать] FROM [Table]";

        public static string PATIENT_GET = "SELECT * FROM [Table] WHERE Id = @Id";
        public static string PATIENT_INSERT = "INSERT INTO [TABLE] (Фамилия, Имя, Отчество, Дата_рождения, Рост, Вес, Пол) VALUES(@Фамилия, @Имя, @Отчество, @Дата_рождения, @Рост, @Вес, @Пол)";
        public static string PATIENT_UPDATE = "UPDATE [TABLE] SET [Фамилия] = @Фамилия, [Имя] = @Имя, [Отчество] = @Отчество, [Дата_рождения] = @Дата_рождения, [Рост] = @Рост, [Вес] = @Вес, [Пол] = @Пол WHERE Id = @r";

        /*  Main function   */
        public DBconnection(string connectionString = defaultConnection) {
            try { sqlConnection = new SqlConnection(connectionString); }
            catch(Exception e) { throw e; }
        }

        /* Public classes*/
        public DataSet get_patients() {
            try
            {
                sqlConnection.Open();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(PATIENTS_GET, sqlConnection);
                SqlCommandBuilder sqlBuilder = new SqlCommandBuilder(sqlDataAdapter);
                DataSet dataSet = new DataSet();
                sqlDataAdapter.Fill(dataSet, "Table");
                return dataSet;
            }
            catch (Exception e) { throw e; }
            finally { sqlConnection.Close(); }
        }

        /*  Record block    */
        public void insert_record(Record record)
        {

            try
            {
                sqlConnection.Open();
                SqlCommand command = new SqlCommand(RECORD_ADDTITION, sqlConnection);
                command.Parameters.AddWithValue("Id", record.id);
                command.Parameters.AddWithValue("Время", record.time);
                command.Parameters.AddWithValue("Длительность", record.duration);
                command.Parameters.AddWithValue("Пациент", record.patient.id);
                command.ExecuteNonQuery();
            }
            catch (Exception e) { throw e; }
            finally { sqlConnection.Close(); }
        }
        public void update_record(Record record)
        {
            try
            {
                sqlConnection.Open();
                SqlCommand command = new SqlCommand(RECORD_UPDATE, sqlConnection);
                command.Parameters.AddWithValue("Id", record.id);
                command.Parameters.AddWithValue("Время", record.time);
                command.Parameters.AddWithValue("Длительность", record.duration);
                command.Parameters.AddWithValue("Пациент", record.patient.id);
                command.Parameters.AddWithValue("Примечание", record.comments);
                command.ExecuteNonQuery();
            }
            catch (Exception e) { throw e; }
            finally { sqlConnection.Close(); }
        }

        /*  Patient Block    */
        public void fill_patient(Patient patient) {
            try
            {
                sqlConnection.Open();
                SqlCommand command = new SqlCommand(PATIENT_GET, sqlConnection);
                command.Parameters.AddWithValue("Id", patient.id);

                var reader = command.ExecuteReader();
                if (reader.HasRows) {
                    while (reader.Read()) {
                        patient.surname = reader.GetString(1);
                        patient.name = reader.GetString(2);
                        patient.middleName = reader.GetString(3);
                        patient.gender = reader.GetBoolean(4);
                        patient.birthdayDate = reader.GetDateTime(5);
                        patient.height = reader.GetInt32(6);
                        patient.weight = reader.GetInt32(7);
                    }
                }                 
            }
            catch (Exception e) { throw e; }
            finally { sqlConnection.Close(); }
        }
        public void insert_patient(Patient patient) { 
            try
            {
                sqlConnection.Open();
                SqlCommand command = new SqlCommand(PATIENT_INSERT, sqlConnection);
                command.Parameters.AddWithValue("Фамилия", patient.surname);
                command.Parameters.AddWithValue("Имя", patient.name);
                command.Parameters.AddWithValue("Отчество", patient.middleName);
                command.Parameters.AddWithValue("Дата_рождения", patient.birthdayDate);
                command.Parameters.AddWithValue("Рост", patient.height);
                command.Parameters.AddWithValue("Вес", patient.weight);
                command.Parameters.AddWithValue("Пол", patient.gender);
                command.ExecuteNonQuery();
            }
            catch (Exception e) { throw e; }
            finally { sqlConnection.Close(); }
        }
        public void update_patient(Patient patient) { 
            try
            {
                sqlConnection.Open();
                SqlCommand command = new SqlCommand(PATIENT_UPDATE, sqlConnection);
                command.Parameters.AddWithValue("r", patient.id);
                command.Parameters.AddWithValue("Фамилия", patient.surname);
                command.Parameters.AddWithValue("Имя", patient.name);
                command.Parameters.AddWithValue("Отчество", patient.middleName);
                command.Parameters.AddWithValue("Дата_рождения", patient.birthdayDate);
                command.Parameters.AddWithValue("Рост", patient.height);
                command.Parameters.AddWithValue("Вес", patient.weight);
                command.Parameters.AddWithValue("Пол", patient.gender);
                command.ExecuteNonQuery();
            }
            catch (Exception e) { throw e; }
            finally { sqlConnection.Close(); }
        }
        
    }
}
