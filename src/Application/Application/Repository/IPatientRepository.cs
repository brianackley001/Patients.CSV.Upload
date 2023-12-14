
using Application.DTO;
using Domain.Models;

namespace Application.Repository;
public interface IPatientRepository
{
    Task<PagedCollection<List<Patient>>> GetPatients(int pageNumber, int pageSize, string? searchTerm, string? sortBy, bool? sortAsc);
    Task<Patient> UpsertPatient(Patient patient);
    Task<bool> ImportPatients(List<PatientUploadTvpDTO> patients);
}
