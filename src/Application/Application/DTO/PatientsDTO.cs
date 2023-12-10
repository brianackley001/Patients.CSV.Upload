using System.Text.Json.Serialization;
using Domain.Models;

namespace Application.DTO;
public class PatientsDTO
{
    public PatientsDTO(List<Patient>? patients, int pageNumber, int pageSize, int collectionTotal)
    {
        // Allows for data mapping to decouple backend data entities from front end models
        Patients = patients;
        PageNumber = pageNumber;
        PageSize = pageSize;
        CollectionTotal = collectionTotal;
    }

    [JsonConstructor]
    public PatientsDTO()
    {
    }

    public List<Patient>? Patients { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }   
    public int CollectionTotal { get; set; }    
}
