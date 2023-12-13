using Application.DTO;
using Application.Manager;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Web.Api.Patients.Controllers;

namespace Web.Api.Patients.Tests;
internal class PatientControllerTest
{
    private Mock<IPatientManager>? _patientManager;
    private NullReferenceException? _expectedException;
    private PatientsDTO _expectedPatientsDTO;
    private PatientDTO _expectedPatientDTO;

    [SetUp]
    public void Setup()
    {
        _expectedException = new NullReferenceException("The unit test threw an exception");
        _patientManager = new Mock<IPatientManager>(MockBehavior.Strict);
        _expectedPatientsDTO = new PatientsDTO
        {
            CollectionTotal = 100,
            PageNumber = 1,
            PageSize = 10,
            Patients = new List<PatientDTO>
            {
                new() {
                    Id = 1,
                    FirstName = "Test",
                    LastName = "Test",
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now,
                    BirthDate = DateTime.Now,
                    GenderDescription = "Test"
                },
                new() {
                    Id = 2,
                    FirstName = "Test",
                    LastName = "Test",
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now,
                    BirthDate = DateTime.Now,
                    GenderDescription = "Test"
                },
            }
        };
        _expectedPatientDTO = new PatientDTO
        {
            Id = 100,
            FirstName = "Test",
            LastName = "Test",
            DateCreated = DateTime.Now,
            DateUpdated = DateTime.Now,
            BirthDate = DateTime.Now,
            GenderDescription = "Test"
        };
    }

    [Test(Description = "GET patients collection results in 200 Success")]
    public async Task GetPatientsOKResult()
    {
        // Arrange
        _patientManager.Reset();
        _patientManager.Setup(m => m.GetPatients(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<bool?>())).Returns(Task.FromResult(_expectedPatientsDTO));
        var sut = new PatientController(_patientManager.Object);

        // Act
        var response = await sut.Get(1, 10, "test", "LastName", false);
        OkObjectResult? okObjectResult = response as OkObjectResult;
        PatientsDTO? responseModel = okObjectResult?.Value as PatientsDTO;

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(response, Is.Not.Null, "response != null");
            Assert.That(responseModel, Is.Not.Null, "responseModel != null");
        });
        _patientManager.Verify(m => m.GetPatients(1, 10,"test", "LastName", false), Times.Once, "Expected method to be called once");
    }

    [Test(Description = "GET patients collection results in 500 Internal Server Error")]
    public async Task GetPatientsInternalServerErrorResult()
    {
        // Arrange
        _patientManager.Reset();
        _patientManager.Setup(m => m.GetPatients(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<bool?>())).Throws(_expectedException!);
        var sut = new PatientController(_patientManager.Object);

        // Act
        var response = await sut.Get(1, 10, "test", "LastName", false);
        var statusCodeResult = response as StatusCodeResult;
        ObjectResult? objectResult = response as ObjectResult;
        bool? responseModel = objectResult?.Value as bool?;

        // Assert
        Assert.That(statusCodeResult, Is.Not.Null, "statusCodeResult != null");
        Assert.That(statusCodeResult.StatusCode, Is.EqualTo(500), "StatusCode == 500");
        _patientManager.Verify(m => m.GetPatients(1, 10, "test", "LastName", false), Times.Once, "Expected method to be called once");
    }

    [Test(Description = "POST patient results in 200 Success")]
    public async Task PostPatientOKResult()
    {
        // Arrange
        _patientManager.Reset();
        _patientManager.Setup(m => m.UpsertPatient(It.IsAny<PatientDTO>())).Returns(Task.FromResult(_expectedPatientDTO));
        var sut = new PatientController(_patientManager.Object);

        // Act
        var response = await sut.Post(_expectedPatientDTO);
        OkObjectResult? okObjectResult = response as OkObjectResult;
        PatientDTO? responseModel = okObjectResult?.Value as PatientDTO;

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(response, Is.Not.Null, "response != null");
            Assert.That(responseModel, Is.Not.Null, "responseModel != null");
        });
        _patientManager.Verify(m => m.UpsertPatient(_expectedPatientDTO), Times.Once, "Expected method to be called once");
    }

    [Test(Description = "POST patient  results in 500 Internal Server Error")]
    public async Task PostPatientInternalServerErrorResult()
    {
        // Arrange
        _patientManager.Reset();
        _patientManager.Setup(m => m.UpsertPatient(It.IsAny<PatientDTO>())).Throws(_expectedException!);
        var sut = new PatientController(_patientManager.Object);

        // Act
        var response = await sut.Post(_expectedPatientDTO);
        var statusCodeResult = response as StatusCodeResult;
        ObjectResult? objectResult = response as ObjectResult;
        bool? responseModel = objectResult?.Value as bool?;

        // Assert
        Assert.That(statusCodeResult, Is.Not.Null, "statusCodeResult != null");
        Assert.That(statusCodeResult.StatusCode, Is.EqualTo(500), "StatusCode == 500");
        _patientManager.Verify(m => m.UpsertPatient(_expectedPatientDTO), Times.Once, "Expected method to be called once");
    }

}
