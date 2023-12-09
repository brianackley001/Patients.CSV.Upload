namespace Domain.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? GenderDescription { get; set; } = "Rather Not Say";
        public DateTime? CreatedDate { get; set; } = default;
        public DateTime? UpdatedDate { get; set; } = default;
        public DateTime BirthDate { get; set; }
        public bool IsActive { get; set; }
    }
}
