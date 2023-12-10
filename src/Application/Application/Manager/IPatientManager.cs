using Application.DTO;
using Domain.Models;

namespace Application.Manager;
public interface IPatientManager
{
    Task<PatientsDTO> GetPatients(int pageNumber, int pageSize); 
    Task<PatientDTO> UpsertPatient(PatientDTO patient);
    Task<bool> ImportPatients(List<Patient> patients);
}
