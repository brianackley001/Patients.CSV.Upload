using Microsoft.AspNetCore.Mvc;
using NLog;
using Application.Manager;
using Application.DTO;
using System.Web.Http;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using FromBodyAttribute = Microsoft.AspNetCore.Mvc.FromBodyAttribute;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;

namespace Web.Api.Patients.Controllers;

[Produces("application/json")]
[ApiController]
[Route("[controller]")]
public class PatientController : ControllerBase
{
    private readonly NLog.ILogger _logger = LogManager.GetCurrentClassLogger();
    private readonly IPatientManager _patientManager;


    public PatientController(IPatientManager patientManager)
    {
        _patientManager = patientManager;
    }

    [HttpGet(Name = "GetPatients") ]
    public async Task<IActionResult> Get([FromUri] int pageNumber, [FromUri] int pageSize)
    {
        try
        {
            var patients = await _patientManager.GetPatients(pageNumber, pageSize).ConfigureAwait(true);
            return Ok(patients);
        }
        catch (Exception ex)
        {
            // Log Exception
            _logger.Error(ex, "Error retrieving Patients.");
            return StatusCode(500);
        }
    }

    [HttpPost(Name = "UpsertPatient")]
    public async Task<IActionResult> Post([FromBody] PatientDTO patient)
    {
        try
        {
            var upsertResult = await _patientManager.UpsertPatient(patient).ConfigureAwait(true);
            return Ok(upsertResult);
        }
        catch (Exception ex)
        {
            // Log Exception
            _logger.Error(ex, "Error upserting Patient.");
            return StatusCode(500);
        }
    }   
}
