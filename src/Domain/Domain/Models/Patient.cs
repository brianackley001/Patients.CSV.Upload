namespace Domain.Models;
public class Patient
{
    public int PatientId { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string GenderDescription { get; set; } = default!;
    public DateTime BirthDate { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateUpdated { get; set; }
    public bool IsActive { get; set; }
}
