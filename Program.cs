
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.Data.Sqlite;

class Bearing
{
    // Bearing Characteristics 
    public double RotationSpeed { get; set; } 
    public double StressLevel { get; set; }
    public double Temperature { get; set; }

    public Bearing(double initial_temperature = 22.0, double initial_rotationSpeed = 300, double initial_StressLevel = 0) 
    {
        Temperature = initial_temperature;
        RotationSpeed = initial_rotationSpeed;
        StressLevel = initial_StressLevel;
    }

    // step(setpoint, duration)
}




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
                    Temperature REAL
                );";
            SqliteCommand cmd = new SqliteCommand(createTableQuery, conn);
            cmd.ExecuteNonQuery();
        }
    }

    public void InsertSimulationData(int simulation_id, string timestamp, double rotationSpeed, double temperature)
    {
        using (SqliteConnection conn = new SqliteConnection(connectionString)) // context manager
        {
            conn.Open();
            string insertQuery = "INSERT INTO Simulations (Simulation_ID, Timestamp, RotationSpeed, Temperature) VALUES (@simulation_id, @timestamp, @rotationSpeed, @temperature)";
            using (SqliteCommand cmd = new SqliteCommand(insertQuery, conn)) 
            {
                cmd.Parameters.AddWithValue("@simulation_id", simulation_id);
                cmd.Parameters.AddWithValue("@timestamp", timestamp);
                cmd.Parameters.AddWithValue("@rotationSpeed", rotationSpeed);
                cmd.Parameters.AddWithValue("@temperature", temperature);
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





class Simulation
{
    private DatabaseManager dbManager;
    private Bearing TestBearing;
    private DateTime Timestamp;

    public Simulation ( int[]? setpoint = null, int[]? duration= null)//double setpoint = 300, double duration = 10)
    {

        dbManager = new DatabaseManager(); // chamar basedados da simulação
        int new_simulation_id = dbManager.GetLastSimulationID() + 1;

        TestBearing = new Bearing(); // criar novo bearing
        Timestamp = DateTime.UtcNow;

        // temperature variation 
        /*
        double max_temperature = 80.0;
        double k = 0.05;
        double alpha = 0.0001;
        */
        
        // If arrays == null, set default values
        if (setpoint == null)
        {
            setpoint = new int[] { 100, 200 };
            Console.WriteLine("Error");
        }

        if (duration == null)
        {
            duration = new int[] { 10, 20 };
            Console.WriteLine("Error");
        }

        if (setpoint.Length != duration.Length )
        {
            Console.WriteLine("Error");
        }

        for (int i = 0; i < setpoint.Length; i++)
        {
        
            // random variation for rotation speed meeasurement
            Random variations = new Random();

            double period = 20;
            double step_time_size = duration[i]/period;
            double step_time = 0;
            
            Console.WriteLine("\nResultados da Simulação:");
            Console.WriteLine("Timestamp\t\tRPM\tTemperatura");

            while (step_time < duration[i])
            {
                TestBearing.Temperature = duration[i]* setpoint[i] / 4;  //max_temperature - (max_temperature - TestBearing.Temperature) * Math.Exp(-k * step_time) + alpha * Math.Pow(TestBearing.RotationSpeed, 2);
                TestBearing.StressLevel = duration[i]* setpoint[i] / 2;
                TestBearing.RotationSpeed = setpoint[i] + (variations.NextDouble() * 4 - 2); // variation of +/- 2 rpm
                
                string timestampString = Timestamp.AddSeconds(step_time).ToString("yyyy-MM-dd HH:mm:ss");


                Console.WriteLine($"{timestampString}\t{TestBearing.RotationSpeed:F2}\t{TestBearing.Temperature:F2}°C");
                dbManager.InsertSimulationData(new_simulation_id, timestampString, TestBearing.RotationSpeed, TestBearing.Temperature);
                step_time += step_time_size;

            }
        }
    }
}



class Program
{
    static void Main()
    {

        Simulation TestSimulation = new Simulation();

    }
}

