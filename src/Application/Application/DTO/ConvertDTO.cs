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

    //public PatientsDTO ConvertToPatientsDTO(PagedCollection<List<Patient>> patients)
    //{
    //    return new PatientsDTO(patients);
    //}
}
