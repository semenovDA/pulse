using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;
using pulse.collection;

namespace pulse.core
{
    public class DBconnection
    {
        /*  Variable defenition */
        public const string connString = @"DataSource={0}";

#if DEBUG
        public string defaultDBPath = @"C:\Users\Admin\Desktop\Pulse_2.0\Исходники\pulse_2.0\main.db";
#else
        public string defaultDBPath = Properties.Settings.Default.DBPath;
#endif

        public SQLiteConnection sqlConnection = null;

        public static string RECORD_GET = "SELECT * FROM [Data] WHERE Id = @Id";
        public static string RECORD_GET_BY_PATIENT = "SELECT * FROM [Data] WHERE Пациент = @Id";
        public static string RECORD_ADDTITION = "INSERT INTO [DATA] (Id, Время, Длительность, Пациент, Примечание) VALUES(@Id, @Время, @Длительность, @Пациент, @Примечание)";
        public static string RECORD_UPDATE = "UPDATE [DATA] SET [Примечание] = @Примечание, [Время] = @Время, [Длительность] = @Длительность, [Пациент] = @Пациент WHERE Id = @Id";
        public static string RECORD_DELETE = "DELETE FROM [DATA] WHERE Id = @Id";

        public static string PATIENTS_GET = "SELECT *, 'Delete' AS [Удалить], 'Update' AS [Изменить], 'Data' AS [Данные] FROM [Table]";
        public static string PATIENTS_GET_CHOOSE = "SELECT *, 'Choose' AS [Выбрать] FROM [Table]";

        public static string PATIENT_GET = "SELECT * FROM [Table] WHERE Id = @Id";
        public static string PATIENT_INSERT = "INSERT INTO [TABLE] (Фамилия, Имя, Отчество, Дата_рождения, Рост, Вес, Пол) VALUES(@Фамилия, @Имя, @Отчество, @Дата_рождения, @Рост, @Вес, @Пол)";
        public static string PATIENT_UPDATE = "UPDATE [TABLE] SET [Фамилия] = @Фамилия, [Имя] = @Имя, [Отчество] = @Отчество, [Дата_рождения] = @Дата_рождения, [Рост] = @Рост, [Вес] = @Вес, [Пол] = @Пол WHERE Id = @r";
        public static string PATIENT_DELETE = "DELETE FROM [TABLE] WHERE Id = @Id";

        /*  Main function   */
        public DBconnection(string connectionString = connString) {
            if(connectionString == connString) {
                connectionString = String.Format(connectionString, defaultDBPath);
            }
            try {
                sqlConnection = new SQLiteConnection(connectionString);
                sqlConnection.Open();
            }
            catch (Exception exp)
            {
                MessageBox.Show("Ошибка. Невозможно подключится к БД.\n Подробнее: " + exp.Message);
            }
            finally { sqlConnection.Close(); }
        }

        /* Public classes*/
        public DataSet get_patients(bool edit = false) {
            try
            {
                sqlConnection.Open();
                string query = edit ? PATIENTS_GET : PATIENTS_GET_CHOOSE;
                SQLiteDataAdapter sqlDataAdapter = new SQLiteDataAdapter(query, sqlConnection);
                SQLiteCommandBuilder sqlBuilder = new SQLiteCommandBuilder(sqlDataAdapter);
                DataSet dataSet = new DataSet();
                sqlDataAdapter.Fill(dataSet, "Table");
                return dataSet;
            }
            catch (Exception e) { throw e; }
            finally { sqlConnection.Close(); }
        }

        /*  Record block    */
        public DataSet get_records(Patient patient)
        {
            try
            {
                SQLiteCommand comm = new SQLiteCommand(RECORD_GET_BY_PATIENT, sqlConnection);
                comm.Parameters.AddWithValue("Id", patient.id);

                SQLiteDataAdapter sqlDataAdapter = new SQLiteDataAdapter(comm);
                SQLiteCommandBuilder sqlBuilder = new SQLiteCommandBuilder(sqlDataAdapter);
                DataSet dataSet = new DataSet();

                sqlDataAdapter.Fill(dataSet, "Data");
                return dataSet;
            }
            catch (Exception e) { throw e; }
            finally { sqlConnection.Close(); }

        }
        public void fill_record(Record record)
        {
            try
            {
                sqlConnection.Open();
                SQLiteCommand command = new SQLiteCommand(RECORD_GET, sqlConnection);
                command.Parameters.AddWithValue("Id", record.id);
                var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        record.time = reader.GetDateTime(1);
                        record.duration = reader.GetFloat(2);
                        record.comments = reader.GetString(3);
                        Patient _patient = new Patient(reader.GetInt32(4));
                        _patient.get();
                        record.patient = _patient;
                    }
                }
            }
            catch (Exception e) { throw e; }
            finally { sqlConnection.Close(); }
        }
        public void insert_record(Record record)
        {
            try
            {
                sqlConnection.Open();
                SQLiteCommand command = new SQLiteCommand(RECORD_ADDTITION, sqlConnection);
                command.Parameters.AddWithValue("Id", record.id);
                command.Parameters.AddWithValue("Время", record.time);
                command.Parameters.AddWithValue("Длительность", record.duration);
                command.Parameters.AddWithValue("Примечание", record.comments);
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
                SQLiteCommand command = new SQLiteCommand(RECORD_UPDATE, sqlConnection);
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
        public void delete_record(Record record)
        {
            try
            {
                sqlConnection.Open();
                SQLiteCommand command = new SQLiteCommand(RECORD_DELETE, sqlConnection);
                command.Parameters.AddWithValue("Id", record.id);
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
                SQLiteCommand command = new SQLiteCommand(PATIENT_GET, sqlConnection);
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
                SQLiteCommand command = new SQLiteCommand(PATIENT_INSERT, sqlConnection);
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
                SQLiteCommand command = new SQLiteCommand(PATIENT_UPDATE, sqlConnection);
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
        public void delete_patient(Patient patient)
        {
            try
            {
                sqlConnection.Open();
                SQLiteCommand command = new SQLiteCommand(PATIENT_UPDATE, sqlConnection);
                command.Parameters.AddWithValue("Id", patient.id);
                command.ExecuteNonQuery();
            }
            catch (Exception e) { throw e; }
            finally { sqlConnection.Close(); }
        }
    }
}
