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
        
  
        double tau = 10; // (s)
        double alpha = 0.0002; // ( s^2 / (K * rad^2) )
        
     

        for (int i = 0; i < setpoint.Length; i++)
        {
        
            // random variation for sensors measurement
            Random variations = new Random();

            double period = 20;
            double step_time_size = duration[i]/period;
            double step_time = 0;
            
            // Console.WriteLine("\nSimulation Results:");
            // Console.WriteLine("Timestamp\t\tRPM\tTemperature\tStress");

            while (step_time < duration[i])
            {
                TestBearing.Temperature = TestBearing.Temperature + alpha * Math.Pow( 2 * Math.PI * setpoint[i]/60 , 2) * (1-Math.Exp(-step_time)/tau) + (variations.NextDouble() * 0.04 - 0.02);  // temp = env_temp + alpha * w^2 * (1 - exp (-t/tau))
                TestBearing.StressLevel =  Math.Pow( 2 * Math.PI * setpoint[i]/60 , 2) + (variations.NextDouble() * 4 - 2); // stress = (material density) * (radius) * w^2
                TestBearing.RotationSpeed = setpoint[i] + (variations.NextDouble() * 4 - 2); // variation of +/- 2 rpm in sensors
                
                string timestampString = Timestamp.AddSeconds(step_time).ToString("yyyy-MM-dd HH:mm:ss");


                // Console.WriteLine($"{timestampString}\t{TestBearing.RotationSpeed:F2}\t{TestBearing.Temperature:F2}°C\t{TestBearing.StressLevel:F2}Pa");
                dbManager.InsertSimulationData(CurrentSimulation_ID, timestampString, TestBearing.RotationSpeed, TestBearing.Temperature, TestBearing.StressLevel);
                step_time += step_time_size;

            }
        }
    }
}

