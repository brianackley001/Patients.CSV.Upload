using Microsoft.AspNetCore.Mvc;
using Moq;
using Application.Manager;
using NLog;
using Application.DTO;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework.Internal;
using ILogger = NLog.ILogger;
using Web.Api.Patients.Controllers;

namespace Web.Api.Patients.Tests;

public class ImportControllerTests
{
    private Mock<IPatientManager>? _patientManager;
    private Mock<ILogger>? _mockLogger;
    private NullReferenceException? _expectedException;
    private List<PatientUploadTvpDTO>? _tvpItems;
    [SetUp]
    public void Setup()
    {
        _expectedException = new NullReferenceException("The unit test threw an exception");
        _patientManager = new Mock<IPatientManager>();
        _mockLogger = new Mock<ILogger>();
        _tvpItems = new List<PatientUploadTvpDTO>
            {
                new PatientUploadTvpDTO
                {
                     BirthDate = DateTime.Now,
                     FirstName = "Test",
                     LastName = "Test",
                     GenderDescription = "Test"
                },
                new PatientUploadTvpDTO
                {
                     BirthDate = DateTime.Now,
                     FirstName = "Test2",
                     LastName = "Test2",
                     GenderDescription = "Test2"
                }
            };
    }

    [Test (Description = "Import patients collection results in 200 Success")]
    public async Task ImportPatientsOKResult()
    {
        // Arrange
         _patientManager.Reset();
        _patientManager.Setup(m => m.ImportPatients(It.IsAny<List<PatientUploadTvpDTO>>())).Returns(Task.FromResult(true));
        var sut = new ImportController(_patientManager.Object);

        // Act
        var response = await sut.ImportPatientList(_tvpItems);
        OkObjectResult? okObjectResult = response as OkObjectResult;
        bool? responseModel = okObjectResult?.Value as bool?;

        // Assert
        Assert.That(response, Is.Not.Null, "response != null");
        Assert.That(responseModel, Is.Not.Null, "responseModel != null");
        _patientManager.Verify(m => m.ImportPatients(_tvpItems), Times.Once, "Expected method to be called once");
    }

    [Test (Description = "Import patients collection results in 500 Internal Server Error")]
    public async Task ImportPatientsInternalServerErrorResult()
    {
        // Arrange
        _patientManager.Reset();
        _mockLogger.Reset();
        _patientManager.Setup(m => m.ImportPatients(It.IsAny<List<PatientUploadTvpDTO>>())).Throws(_expectedException);
        var sut = new ImportController(_patientManager.Object);

        // Act
        var response = await sut.ImportPatientList(_tvpItems);
        var statusCodeResult = response as StatusCodeResult;
        ObjectResult? objectResult = response as ObjectResult;
        bool? responseModel = objectResult?.Value as bool?;

        // Assert
        Assert.That(statusCodeResult, Is.Not.Null, "statusCodeResult != null");
        Assert.That(statusCodeResult.StatusCode == 500, Is.True, "StatusCode == 500");
        _patientManager.Verify(m => m.ImportPatients(_tvpItems), Times.Once, "Expected method to be called once");
    }
}
