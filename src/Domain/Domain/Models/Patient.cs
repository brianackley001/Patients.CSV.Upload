namespace Domain.Models;
public class Patient
{
    public int Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string GenderDescription { get; set; } = default!;
    public DateTime? DateOfBirth { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}
