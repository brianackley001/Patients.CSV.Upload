using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO;

namespace Application.Manager;
public interface IPatientManager
{
    Task<PatientsDTO> GetPatients(int pageNumber, int pageSize);
}
