using Domain.Models;

namespace Application.DTO;
public class ConvertDTO : IConvertDTO
{
    public async Task<PatientDTO> ConvertToPatientDTO(Patient patient)
    {
        var patientDTO = new PatientDTO();
        await Task.Run(() => patientDTO = new PatientDTO()
        {
            // Allows for data mapping to decouple backend data entities from front end models
            Id = patient.PatientId,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            GenderDescription = patient.GenderDescription,
            BirthDate = patient.BirthDate,
            DateCreated = patient.DateCreated,
            DateUpdated = patient.DateUpdated
        }).ConfigureAwait(true);
        return patientDTO;
    }

    public async Task<PatientsDTO> ConvertToPatientsDTO(PagedCollection<List<Patient>> patients)
    {
        var patientsDTO = new PatientsDTO();
        List<PatientDTO> convertedCollection = patients.Collection!.ConvertAll<PatientDTO>(p => new PatientDTO
        {
            Id = p.PatientId,
            FirstName = p.FirstName,
            LastName = p.LastName,
            GenderDescription = p.GenderDescription,
            BirthDate = p.BirthDate,
            DateCreated = p.DateCreated,
            DateUpdated = p.DateUpdated
        });
        await Task.Run(() => patientsDTO = new PatientsDTO()
        {
            // Allows for data mapping to decouple backend data entities from front end models
            Patients = convertedCollection,
            PageNumber = patients.PageNumber,
            PageSize = patients.PageSize,
            CollectionTotal = patients.CollectionTotal
        }).ConfigureAwait(true);
        return patientsDTO;
    }
}
