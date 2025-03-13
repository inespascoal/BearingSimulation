


class Simulation
{
    private DatabaseManager dbManager;
    private Bearing TestBearing;
    private DateTime Timestamp;
    private int CurrentSimulation_ID;

    public Simulation ()
    {

        dbManager = new DatabaseManager(); 
        CurrentSimulation_ID = dbManager.GetLastSimulationID() + 1;

        TestBearing = new Bearing();
        Timestamp = DateTime.UtcNow;
    }

    public void run( int[] setpoint , int[] duration)
    {

        // temperature variation 
        /*
        double max_temperature = 80.0;
        double k = 0.05;
        double alpha = 0.0001;
        */
     

        for (int i = 0; i < setpoint.Length; i++)
        {
        
            // random variation for rotation speed meeasurement
            Random variations = new Random();

            double period = 20;
            double step_time_size = duration[i]/period;
            double step_time = 0;
            
            Console.WriteLine("\nResultados da Simulação:");
            Console.WriteLine("Timestamp\t\tRPM\tTemperature\tStress");

            while (step_time < duration[i])
            {
                TestBearing.Temperature = duration[i]* setpoint[i] / 4;  //max_temperature - (max_temperature - TestBearing.Temperature) * Math.Exp(-k * step_time) + alpha * Math.Pow(TestBearing.RotationSpeed, 2);
                TestBearing.StressLevel = duration[i]* setpoint[i] / 2;
                TestBearing.RotationSpeed = setpoint[i] + (variations.NextDouble() * 4 - 2); // variation of +/- 2 rpm
                
                string timestampString = Timestamp.AddSeconds(step_time).ToString("yyyy-MM-dd HH:mm:ss");


                Console.WriteLine($"{timestampString}\t{TestBearing.RotationSpeed:F2}\t{TestBearing.Temperature:F2}°C\t{TestBearing.StressLevel:F2}Pa");
                dbManager.InsertSimulationData(CurrentSimulation_ID, timestampString, TestBearing.RotationSpeed, TestBearing.Temperature, TestBearing.StressLevel);
                step_time += step_time_size;

            }
        }
    }
}

