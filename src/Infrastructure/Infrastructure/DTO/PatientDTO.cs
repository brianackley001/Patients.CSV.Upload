using Domain.Models;

namespace Infrastructure.DTO;
public class PatientDTO
{
    public PatientDTO(Patient patient)
    {
        // Allows for data mapping to decouple backend data entities from front end models
        Id = patient.Id;
        FirstName = patient.FirstName;
        LastName = patient.LastName;
        GenderDescription = patient.GenderDescription;
        DateOfBirth = patient.DateOfBirth;
        CreatedDate = patient.CreatedDate;
        UpdatedDate = patient.UpdatedDate;
    }

    public int Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string GenderDescription { get; set; } = default!;
    public DateTime? DateOfBirth { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}
