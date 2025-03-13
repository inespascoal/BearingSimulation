
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseDefaultFiles(); 
app.UseStaticFiles();  

app.MapPost("/run", async (HttpContext context) =>
{
    try
    {
    
        var request = await JsonSerializer.DeserializeAsync<SimulationRequest>(context.Request.Body);
        
        if (request == null)
        {
            return Results.Problem("Error: Invalid data.");
        }
        if (request.Setpoint == null || request.Setpoint[0] == 0)
        {
            request.Setpoint = new int[] { 100, 200 };
            //Console.WriteLine("Error");
        }

        if (request.Duration == null || request.Duration[0] == 0)
        {
            request.Duration = new int[] { 10, 20 };
            //Console.WriteLine("Error");
        }

        if (request.Setpoint.Length != request.Duration.Length )
        {
            Console.WriteLine("Error: The setpoint and duration must have the same length");
        }

        var simulation = new Simulation();
        simulation.run(request.Setpoint, request.Duration);
        
        
        return Results.Ok("Initialised simulation!");
    }
    catch (Exception ex)
    {
        return Results.Problem("Error: " + ex.Message);
    }
});

app.Run();

class SimulationRequest
{
    public int[]? Setpoint { get; set; }
    public int[]? Duration { get; set; }
}
