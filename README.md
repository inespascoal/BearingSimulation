
# Bearing Simulation


This project is a web-based Bearing Simulation system that allows users to input simulation parameters and execute a predefined simulation model. The results are stored in an SQLite database.



The application:

Accepts setpoint (RPM) and duration (s) values from the user.

Runs a simulation where temperature and stress levels are calculated over time and it takes into account the sensors accuracy for rotation speed, temperature, and stress levels.

Stores simulation data in an SQLite database.



## Getting Started

**1. Clone the Repository**
```bash
    git clone https://github.com/inespascoal/BearingSimulation.git
   
    cd BearingSimulation
```

**2. Install Dependencies**

Ensure you have the .NET SDK installed. If not, install it from dotnet.microsoft.com.

    dotnet restore


**3. Run the Application**

Start the ASP.NET Core server:

    dotnet run


If the server starts successfully, it should output something like:
```bash
Now listening on: http://localhost:5000

Application started. Press Ctrl+C to shut down.
```

**4. Access the Web Interface**

Open your browser and go to:
```bash
http://localhost:5000
```
You will see an interface where you can enter setpoint and duration values and then run a simulation.


**5. Access the DataBase**
In the terminal:
```bash
sqlite3 BearingSimulations.db
.tables
SELECT * FROM Simulations;
```


## Assumptions & Limitations

Users must input valid numeric values; otherwise, defaults are used.

The database only stores simulations but does not provide an interface to view these simulations.

The simulation is an approximation of real-world physics:
```bash
T(t) = T_env + alpha * w^2 * (1 - exp (-t/tau) )
Stress = rho * r * w^2 
```
**Note: as some characteristics were not specified, some values were considered arbitrary.**



## Overview

Implementation of a bearing simulation system using an SQLite database to store the simulation results and a web interface to interact with the user. The approach follows a modular model, separating responsibilities into different classes and files for greater organization and scalability.

The project was divided into 4 parts. One defines the characteristics of the bearing to be simulated; the second defines the database that will contain the simulated data; the third defines the simulation itself, how each input will affect the data; and finally a web interface was created to interact with the user, allowing them to enter the data they wish to analyse.



## Project Structure

```bash
/BearingSimulation/
├── Bearing.cs          # contains the characteristics of bearing 
├── DatabaseManager.cs  # creates and inserts the value on database 
├── Simulation.cs       # defines simulation parameters
├── Program.cs          # creates ASP.NET Core application
├── wwwroot/            
     ├──  index.html     # web interface
└── WebBearing.csproj   # contains the project configurations

```
