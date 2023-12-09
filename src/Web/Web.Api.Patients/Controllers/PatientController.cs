using Microsoft.AspNetCore.Mvc;
using NLog;


namespace Web.Api.Patients.Controllers;

[ApiController]
[Route("[controller]")]
public class PatientController : ControllerBase
{
    private readonly NLog.ILogger _logger = LogManager.GetCurrentClassLogger();


    [HttpGet(Name = "GetPatients")]
    public IEnumerable<WeatherForecast> Get()
    {
        //var ex = new Exception("Test Exception");
        //_logger.Error(ex);

        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            //Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}
