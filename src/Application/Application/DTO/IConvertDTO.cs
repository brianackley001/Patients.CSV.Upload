using Domain.Models;

namespace Application.DTO;
public interface IConvertDTO
{
     Task<PatientDTO> ConvertToPatientDTO(Patient patient);
     Task<PatientsDTO> ConvertToPatientsDTO(PagedCollection<List<Patient>> patients);
}
