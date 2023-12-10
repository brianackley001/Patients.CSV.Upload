using Application.DTO;
using Application.Manager;
using Application.Repository;
using NLog;

namespace Infrastructure.ServiceManager;
public class PatientManager : IPatientManager
{
    private readonly IPatientRepository _patientRepository;
    private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
    public PatientManager(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
    }

    public async Task<PatientsDTO> GetPatients(int pageNumber, int pageSize)
    {
        try
        {
            var patientsCollection = await _patientRepository.GetPatients(pageNumber, pageSize);
            var patientsDto = new PatientsDTO(
                patientsCollection.Collection, 
                patientsCollection.PageNumber, 
                patientsCollection.PageSize, 
                patientsCollection.CollectionTotal);

            return patientsDto;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error in GetPatients");
            throw;
        }
    }
}
