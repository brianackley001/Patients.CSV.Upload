using Newtonsoft.Json;

namespace Domain.Models;
public class Patient
{
    public int PatientId { get; set; }
    [JsonProperty("firstName")]
    public string FirstName { get; set; } = "Unknown";
    [JsonProperty("lastName")]
    public string LastName { get; set; } = "Unknown";
    [JsonProperty("genderDescription")]
    public string GenderDescription { get; set; } = "Unknown";
    public DateTime BirthDate { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateUpdated { get; set; }
    public bool IsActive { get; set; }
}
