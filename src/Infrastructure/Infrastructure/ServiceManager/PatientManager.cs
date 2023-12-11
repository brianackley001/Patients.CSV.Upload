using Application.DTO;
using Application.Manager;
using Application.Repository;
using Domain.Models;
using NLog;

namespace Infrastructure.ServiceManager;
public class PatientManager : IPatientManager
{
    private readonly IPatientRepository _patientRepository;
    private readonly IConvertDTO _convertDTO;
    private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
    public PatientManager(IPatientRepository patientRepository, IConvertDTO convertDTO)
    {
        _patientRepository = patientRepository;
        _convertDTO = convertDTO;
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

    public Task<bool> ImportPatients(List<PatientUploadTvpDTO> patients)
    {
        // No business logic conversions needed here between Web Layer DTO & Domain models - 
        try
        {
            return _patientRepository.ImportPatients(patients);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error in ImportPatients");
            throw;
        }
    }

    public async Task<PatientDTO> UpsertPatient(PatientDTO patientDTO)
    {
        // Allow for business logic conversion here between Web Layer DTO & Domain models
        var patient = new Patient()
        {
            PatientId = patientDTO.Id,
            FirstName = patientDTO.FirstName,
            LastName = patientDTO.LastName,
            DateCreated = (DateTime)patientDTO.DateCreated,   
            DateUpdated = (DateTime)patientDTO.DateUpdated,
            BirthDate = (DateTime)patientDTO.BirthDate,
            GenderDescription = patientDTO.GenderDescription
        };
        try
        {
            var upsertResult = await _patientRepository.UpsertPatient(patient);

            //convert domain model back to DTO for Web Layer
            var dto = await _convertDTO.ConvertToPatientDTO(patient);
            return dto;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error in UpsertPatient");
            throw;
        }
    }
}
