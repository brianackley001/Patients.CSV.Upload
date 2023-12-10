using Domain.Models;

namespace Infrastructure.DTO;
public class PatientsDTO
{
    public PatientsDTO(List<Patient>? patients, int pageNumber, int pageSize, int collectionTotal)
    {
        // Allows for data mapping to decouple backend data entities from front end models
        PageNumber = pageNumber;
        PageSize = pageSize;
        CollectionTotal = collectionTotal;

        // Convert each item in List<Patient> into a PatientDTO
        Patients = new List<PatientDTO>();
        patients?.ForEach(p => Patients.Add(new PatientDTO(p)));
        
    }

    public List<PatientDTO>? Patients { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }   
    public int CollectionTotal { get; set; }    
}
