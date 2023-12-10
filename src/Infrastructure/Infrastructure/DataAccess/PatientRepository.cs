using Application;
using System.Data;
using Application.Repository;
using Domain.Models;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using Dapper;

namespace Infrastructure.DataAccess;
public class PatientRepository : IPatientRepository
{
    private readonly string? _connectionString;

    public PatientRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Patients_ConnectionString");
    }

    public async Task<PagedCollection<List<Patient>>> GetPatients(int pageNumber, int pageSize)
    {
        var collectionTotal = 0;
        var patientCollection = new List<Patient>();
        var pagedCollection = new PagedCollection<List<Patient>> { 
            Collection = new List<Patient>(), 
            CollectionTotal = 0, 
            PageNumber = pageNumber, 
            PageSize = pageSize 
        };

        await Task.Run(() =>
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var parameter = new DynamicParameters();
                parameter.Add("@pageNum", pageNumber, DbType.Int32, ParameterDirection.Input);
                parameter.Add("@pageSize", pageSize, DbType.Int32, ParameterDirection.Input);
                parameter.Add("@collectionTotal", 0, DbType.Int32, ParameterDirection.Output);

                patientCollection = connection
                    .Query<Patient>("usp_GetPatients", parameter, commandType: CommandType.StoredProcedure).ToList();

                collectionTotal = parameter.Get<int>("@collectionTotal");
            }
        }).ConfigureAwait(true);

        pagedCollection.Collection = patientCollection;
        pagedCollection.CollectionTotal = collectionTotal;

        return pagedCollection;
    }
}
