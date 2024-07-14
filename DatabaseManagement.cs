using System.Configuration;
using System.Data.SQLite;
using System.Data;

namespace CodingTracker
{
    internal class DatabaseManagement
    {
        public static void DatabaseSetup()
        {
            string dbFileName = "hoursData.db";
            string dbFilePath = Path.Combine(Directory.GetCurrentDirectory(), dbFileName);

           
            if (!File.Exists(dbFilePath))
            {
                SQLiteConnection.CreateFile("hoursData.db");
            }
            string connectionString = ConfigurationManager.AppSettings["ConnectionString"];
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS HoursTable (
                    Date TEXT PRIMARY KEY,
                    Hours TEXT
                )";

                using (var command = new SQLiteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public static SQLiteConnection ConnectToDB()
        {
            string connectionString = ConfigurationManager.AppSettings["ConnectionString"];
            var connection = new SQLiteConnection(connectionString);
            connection.Open();
            return connection;
        }
        public static void CloseDBConnection(SQLiteConnection connection)
        {
            if (connection != null && connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }

        public static string ReadFromDB(SQLiteConnection connection, string date)
        {
            if (!ProcessInput.CheckDate(date)) { return "Invalid date provided"; }

            string hours = "You have not entered a value for this date";

            using (SQLiteCommand sqlCommand = new SQLiteCommand("SELECT Hours FROM HoursTable WHERE Date = @DATE", connection))
            {
                sqlCommand.Parameters.AddWithValue("@DATE", date);

                using (SQLiteDataReader reader = sqlCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        hours = reader["Hours"].ToString();
                    }
                }
            }
            if (hours == "You have not entered a value for this date") return hours;

            return "You coded for " + hours +" hours on "+ date + '\n';
        }


        public static string InsertIntoDB(SQLiteConnection connection, string date, string hours)
        {
            if (!ProcessInput.CheckDate(date)) { return "Invalid date provided"; }
            if (!ProcessInput.CheckHours(hours)) { return "Invalid hours provided"; }

            if (ExistsInDB(connection, date))
            {
                return "You have already added a value for this date, please update instead";
            }
            else
            {
                using (SQLiteCommand sqlCommand = new SQLiteCommand("INSERT INTO HoursTable (Date, Hours) VALUES (@DATE, @HOURS)", connection))
                {
                    sqlCommand.Parameters.AddWithValue("@DATE", date);
                    sqlCommand.Parameters.AddWithValue("@HOURS", hours);
                    int numR = sqlCommand.ExecuteNonQuery();
                    if (numR == 1)
                    {
                        return "Succesfully added hours data for the date";
                    }
                    else
                    {
                        return "Unsuccesfully added hours data for the date";
                    }

                }
            }   
        }

        public static string UpdateDB(SQLiteConnection connection, string date, string hours)
        {
            if (!ProcessInput.CheckDate(date)) { return "Invalid date provided"; }
            if (!ProcessInput.CheckHours(hours)) { return "Invalid hours provided"; }

            using (SQLiteCommand sqlCommand = new SQLiteCommand("UPDATE HoursTable SET Hours = @HOURS WHERE Date = @DATE", connection))
            {
                sqlCommand.Parameters.AddWithValue("@DATE", date);
                sqlCommand.Parameters.AddWithValue("@HOURS", hours);
                int numR = sqlCommand.ExecuteNonQuery();
                if (numR == 1)
                {
                    return "Succesfully updated hours data for the date";
                }
                else
                {
                    return "There was no value found for the date, insert one first";
                }
            }
        }

        public static string DeleteFromDB(SQLiteConnection connection, string date)
        {
            if (!ProcessInput.CheckDate(date)) { return "Invalid date provided"; }

            using (SQLiteCommand sqlCommand = new SQLiteCommand("DELETE FROM HoursTable WHERE Date = @DATE", connection))
            {
                sqlCommand.Parameters.AddWithValue("@DATE", date);
                int numR = sqlCommand.ExecuteNonQuery();
                if (numR == 1)
                {
                    return "Succesfully deleted hours data for the date";
                }
                else
                {
                    return "There was no value found for the date";
                }
            }
        }

        public static bool ExistsInDB(SQLiteConnection connection, string date)
        {
            bool exists = false;
            using (SQLiteCommand sqlCommand = new SQLiteCommand("SELECT Hours FROM HoursTable WHERE Date = @DATE", connection))
            {
                sqlCommand.Parameters.AddWithValue("@DATE", date);

                using (SQLiteDataReader reader = sqlCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        exists = true;
                    }
                }
            }
            return exists;
        }

    }
}
