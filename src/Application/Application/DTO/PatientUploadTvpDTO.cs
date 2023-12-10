using System.Text.Json.Serialization;

namespace Application.DTO;
public class PatientUploadTvpDTO
{

    [JsonConstructor]
    public PatientUploadTvpDTO()
    {
    }

    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string GenderDescription { get; set; } = default!;
    public DateTime BirthDate { get; set; }

}
