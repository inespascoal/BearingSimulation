class Bearing
{
    // Bearing Characteristics 
    public double RotationSpeed { get; set; }
    public double Temperature { get; set; }
    public double StressLevel { get; set; }

    public Bearing(double initial_temperature = 22.0, double initial_rotationSpeed = 300, double initial_StressLevel = 0) 
    {
        Temperature = initial_temperature;
        RotationSpeed = initial_rotationSpeed;
        StressLevel = initial_StressLevel;
    }

}
