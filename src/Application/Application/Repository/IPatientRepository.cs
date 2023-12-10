
using Domain.Models;

namespace Application.Repository;
public interface IPatientRepository
{
    Task<PagedCollection<List<Patient>>> GetPatients(int pageNumber, int pageSize);
    Task<Patient> UpsertPatient(Patient patient);
    Task<bool> ImportPatients(List<Patient> patients);
}
