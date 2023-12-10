using Application;
using System.Data;
using Application.Repository;
using Domain.Models;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using Dapper;
using NLog;

namespace Infrastructure.DataAccess;
public class PatientRepository : IPatientRepository
{
    private readonly string? _connectionString;
    private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

    public PatientRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Patients_ConnectionString");
    }

    public async Task<PagedCollection<List<Patient>>> GetPatients(int pageNumber, int pageSize)
    {
        var collectionTotal = 0;
        var patientCollection = new List<Patient>();
        var pagedCollection = new PagedCollection<List<Patient>>
        {
            Collection = new List<Patient>(),
            CollectionTotal = 0,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        await Task.Run(() =>
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    var parameter = new DynamicParameters();
                    if (pageNumber > 0)
                    {
                        //Nullable stored procedure parameter. Specify Business Logic for default value(s) in the Domain or Application layer,
                        //fall back to safety net of sproc default value(s) if necessary.
                        parameter.Add("@pageNum", pageNumber, DbType.Int32, ParameterDirection.Input);
                    }
                    if (pageSize > 0)
                    {
                        //Nullable stored procedure parameter. Specify Business Logic for default value(s) in the Domain or Application layer,
                        //fall back to safety net of sproc default value(s) if necessary.
                        parameter.Add("@pageSize", pageSize, DbType.Int32, ParameterDirection.Input);
                    }
                    parameter.Add("@collectionTotal", 0, DbType.Int32, ParameterDirection.Output);

                    patientCollection = connection
                        .Query<Patient>("usp_GetPatients", parameter, commandType: CommandType.StoredProcedure).ToList();

                    collectionTotal = parameter.Get<int>("@collectionTotal");
                }
                catch (Exception ex)
                {
                    // Log Exception
                    _logger.Error(ex, "Error retrieving patients");
                    throw;
                }
            }
        }).ConfigureAwait(true);

        pagedCollection.Collection = patientCollection;
        pagedCollection.CollectionTotal = collectionTotal;

        return pagedCollection;
    }

    public async Task<bool> ImportPatients(List<Patient> patients)
    {
        throw new NotImplementedException();
    }

    public async Task<Patient> UpsertPatient(Patient patient)
    {
        var result = 0;
        var returnValue = -1;

        await Task.Run(() =>
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    var parameter = new DynamicParameters();
                    if (patient.PatientId > 0)
                    {
                        //Upsert:  Update with existing ID value, or Insert new patient record with IDENTITY value.
                        parameter.Add("@id", patient.PatientId, DbType.Int32, ParameterDirection.Input);
                    }
                    parameter.Add("@firstName", patient.FirstName, DbType.String, ParameterDirection.Input);
                    parameter.Add("@lastName", patient.LastName, DbType.String, ParameterDirection.Input);
                    parameter.Add("@genderDescription", patient.GenderDescription, DbType.String, ParameterDirection.Input);
                    parameter.Add("@birthDate", patient.BirthDate, DbType.DateTime, ParameterDirection.Input);
                    parameter.Add("@isActive", patient.IsActive, DbType.Boolean, ParameterDirection.Input);
                    parameter.Add("@returnValue", returnValue, DbType.Int32, ParameterDirection.ReturnValue);

                    result = connection
                        .Execute("usp_UpsertPatient", parameter, commandType: CommandType.StoredProcedure);
                    returnValue = parameter.Get<int>("@returnValue");
                }
                catch (Exception ex)
                {
                    // Log Exception
                    _logger.Error(ex, "Error upserting patient");
                    throw;
                }
            }
        }).ConfigureAwait(true);

        // Insert scenario:
        patient.PatientId = returnValue;

        return patient;
    }
}
