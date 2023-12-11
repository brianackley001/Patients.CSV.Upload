using Domain.Models;

namespace Application.DTO;
public interface IConvertDTO
{
     Task<PatientDTO> ConvertToPatientDTO(Patient patient);
    //public PatientsDTO ConvertToPatientsDTO(PagedCollection<List<Patient>> patients);
}
