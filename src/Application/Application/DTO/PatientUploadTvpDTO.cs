using System.Text.Json.Serialization;

namespace Application.DTO;
public class PatientUploadTvpDTO
{

    [JsonConstructor]
    public PatientUploadTvpDTO()
    {
    }

    public string FirstName { get; set; } = "Unknown";
    public string LastName { get; set; } = "Unknown";
    public string GenderDescription { get; set; } = "Unknown";
    public DateTime BirthDate { get; set; }

}
