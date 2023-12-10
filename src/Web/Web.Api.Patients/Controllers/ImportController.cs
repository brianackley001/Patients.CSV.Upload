using Microsoft.AspNetCore.Mvc;
using NLog;
using Application.Manager;
using Application.DTO;
using System.Web.Http;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;
using FromBodyAttribute = Microsoft.AspNetCore.Mvc.FromBodyAttribute;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;

namespace Web.Api.Patients.Controllers;
[Produces("application/json")]
[ApiController]
[Route("[controller]")]
public class ImportController : ControllerBase
{
    private readonly NLog.ILogger _logger = LogManager.GetCurrentClassLogger();
    private readonly IPatientManager _patientManager;


    public ImportController(IPatientManager patientManager)
    {
        _patientManager = patientManager;
    }

    [HttpPost("patient")]
    public async Task<IActionResult> ImportPatientList([FromBody] List<PatientUploadTvpDTO> patients)
    {
        try
        {
            var importResult = await _patientManager.ImportPatients(patients).ConfigureAwait(true);
            return Ok(importResult);
        }
        catch (Exception ex)
        {
            // Log Exception
            _logger.Error(ex, "Error importing Patients.");
            return StatusCode(500);
        }
    }
}
