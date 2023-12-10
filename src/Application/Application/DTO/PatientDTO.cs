using System.Text.Json.Serialization;
using Domain.Models;

namespace Application.DTO;
public class PatientDTO
{
    public PatientDTO(Patient patient)
    {
        // Allows for data mapping to decouple backend data entities from front end models
        Id = patient.PatientId;
        FirstName = patient.FirstName;
        LastName = patient.LastName;
        GenderDescription = patient.GenderDescription;
        BirthDate = patient.BirthDate;
        DateCreated = patient.DateCreated;
        DateUpdated = patient.DateUpdated;
    }

    [JsonConstructor]
    public PatientDTO()
    {
    }

    public int Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string GenderDescription { get; set; } = default!;
    public DateTime? BirthDate { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
}
