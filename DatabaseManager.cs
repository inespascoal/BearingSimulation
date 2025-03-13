using System;
using Microsoft.Data.Sqlite;

class DatabaseManager
{
    private string connectionString = "Data Source=BearingSimulations.db;";

    public DatabaseManager()
    {
        using (SqliteConnection conn = new SqliteConnection(connectionString))
        {
            conn.Open();
            string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS Simulations (
                    Register_ID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Simulation_ID INTEGER,
                    Timestamp TEXT,
                    RotationSpeed REAL,
                    Temperature REAL,
                    StressLevel REAL
                );";
            SqliteCommand cmd = new SqliteCommand(createTableQuery, conn);
            cmd.ExecuteNonQuery();
        }
    }

    public void InsertSimulationData(int simulation_id, string timestamp, double rotationSpeed, double temperature, double stresslevel)
    {
        using (SqliteConnection conn = new SqliteConnection(connectionString)) // context manager
        {
            conn.Open();
            string insertQuery = "INSERT INTO Simulations (Simulation_ID, Timestamp, RotationSpeed, Temperature, StressLevel) VALUES (@simulation_id, @timestamp, @rotationSpeed, @temperature, @stresslevel)";
            using (SqliteCommand cmd = new SqliteCommand(insertQuery, conn)) 
            {
                cmd.Parameters.AddWithValue("@simulation_id", simulation_id);
                cmd.Parameters.AddWithValue("@timestamp", timestamp);
                cmd.Parameters.AddWithValue("@rotationSpeed",(int)Math.Round(rotationSpeed,0));
                cmd.Parameters.AddWithValue("@temperature", Math.Round(temperature,2));
                cmd.Parameters.AddWithValue("@stresslevel", (int)Math.Round(stresslevel,0));
                cmd.ExecuteNonQuery();
            }
        }
    }
    public int GetLastSimulationID()
    {
        using (SqliteConnection conn = new SqliteConnection(connectionString)) // context manager
        {
            conn.Open();
            string insertQuery = "SELECT Simulation_ID FROM Simulations ORDER BY Register_ID DESC LIMIT 1";
            using (SqliteCommand cmd = new SqliteCommand(insertQuery, conn)) 
            {
                var result = cmd.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int lastID))
                {
                    return lastID; // Return the retrieved ID
                }
                else
                {
                    return -1; // Return -1 if no ID is found
                }
            }
        }
    }
}


