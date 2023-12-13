using Application.Repository;
using NUnit.Framework;
using Moq;
using Infrastructure.ServiceManager;
using Application;
using Domain.Models;
using Application.DTO;
using Infrastructure.DataAccess;

namespace Infrastructure.UnitTests.ServiceManager;

[TestFixture]
public class PatientManagerTest
{
    private Mock<IPatientRepository> _patientRepository;
    private Mock<IConvertDTO> _convertDTO;
    private PagedCollection<List<Patient>> _patientsCollection;
    private NullReferenceException? _expectedException;
    private PatientsDTO _patientsDTO;
    private PatientDTO _patientDTO;
    private List<PatientUploadTvpDTO> _patientUploadTvpDTO;
    private DateTime _birthDateValue;

    [SetUp]
    public void Setup()
    {
        _birthDateValue = DateTime.Now;
        _patientRepository = new Mock<IPatientRepository>();
        _convertDTO = new Mock<IConvertDTO>();
        _expectedException = new NullReferenceException("The unit test threw an exception");
        _patientsDTO = new PatientsDTO
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
                new()
                {
                    Id = 2,
                    FirstName = "Test",
                    LastName = "Test",
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now,
                    BirthDate = DateTime.Now,
                    GenderDescription = "Test",
                },
            }
        };
        _patientsCollection = new PagedCollection<List<Patient>>
        {
            CollectionTotal = 100,
            PageNumber = 1,
            PageSize = 10,
            Collection = new List<Patient>
            {
                new()
                {
                    PatientId = 1,
                    FirstName = "Test",
                    LastName = "Test",
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now,
                    BirthDate = _birthDateValue,
                    GenderDescription = "Test",
                    IsActive = true
                },
                new()
                {
                    PatientId = 2,
                    FirstName = "Test",
                    LastName = "Test",
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now,
                    BirthDate = _birthDateValue,
                    GenderDescription = "Test",
                    IsActive = true
                },
            }
        };
        _patientDTO = new PatientDTO
        {
            Id = 1,
            FirstName = "Test",
            LastName = "Test",
            DateCreated = DateTime.Now,
            DateUpdated = DateTime.Now,
            BirthDate = _birthDateValue,
            GenderDescription = "Test"
        };
        _patientUploadTvpDTO = new List<PatientUploadTvpDTO>
        {
            new()
            {
                FirstName = "Test",
                LastName = "Test",
                BirthDate = _birthDateValue,
                GenderDescription = "Test"
            },
            new()
            {
                FirstName = "Test",
                LastName = "Test",
                BirthDate = _birthDateValue,
                GenderDescription = "Test"
            }
        };
    }

    [Test(Description = "GetPatients returns expected collection")]
    public async Task GetPatientsSuccess()
    {
        // Arrange
        _patientRepository.Reset();
        _patientRepository.Setup(r => r.GetPatients(1, 10, null, null, null)).Returns(Task.FromResult(_patientsCollection));
        _convertDTO.Setup(c => c.ConvertToPatientsDTO(_patientsCollection)).Returns(Task.FromResult(_patientsDTO));
        var sut = new PatientManager(_patientRepository.Object, _convertDTO.Object);

        // Act
        var response = await sut.GetPatients(1, 10, null, null, null);

        // Assert
        Assert.That(response, Is.Not.Null, "response != null");
        Assert.Multiple(() =>
        {
            Assert.That(response.Patients!.GetType(), Is.EqualTo(typeof(List<PatientDTO>)), "response.Result.Patients.GetType()");
            Assert.That(response.GetType(), Is.EqualTo(typeof(PatientsDTO)), "response.Result.Patients.GetType()");
            Assert.That(response.Patients, Has.Count.EqualTo(2), "response.Result.Patients.Count");
        });
        Assert.Multiple(() =>
        {
            Assert.That(response.Patients[0].Id, Is.EqualTo(1), "response.Result.Patients[0].PatientId");
            Assert.That(response.CollectionTotal, Is.EqualTo(_patientsDTO.CollectionTotal), "response.Result.CollectionTotal");
            Assert.That(response.PageNumber, Is.EqualTo(_patientsDTO.PageNumber), "response.Result.PageNumber");
            Assert.That(response.PageSize, Is.EqualTo(_patientsDTO.PageSize), "response.Result.PageSize");
        });
        _patientRepository.Verify(r => r.GetPatients(1, 10, null, null, null), Times.Once, "Expected method to be called once");
    }

    [Test(Description = "GetPatients throws expected exception")]
    public void GetPatientsThrowsException()
    {
        // Arrange
        _patientRepository.Reset();
        _patientRepository.Setup(r => r.GetPatients(1, 10, null, null, null)).Throws(_expectedException!);
        var sut = new PatientManager(_patientRepository.Object, _convertDTO.Object);

        // Act
        //var response = await sut.GetPatients(1, 10);

        // Assert
        NullReferenceException? nullReferenceException = Assert.ThrowsAsync<NullReferenceException>(() => sut.GetPatients(1, 10, null, null, null));
        Assert.That(nullReferenceException.Message, Is.EqualTo(_expectedException.Message));
        _patientRepository.Verify(r => r.GetPatients(1, 10, null, null, null), Times.Once, "Expected method to be called once");
    }

    [Test(Description = "UpsertPatient returns success")]
    public async Task UpsertPatientSuccess()
    {
        // Arrange
        var patientItem = _patientsCollection.Collection[0];

        _convertDTO.Reset();
        _patientRepository.Reset();
        _convertDTO.Setup(c => c.ConvertToPatient(_patientDTO)).Returns(Task.FromResult(patientItem));
        _convertDTO.Setup(c => c.ConvertToPatientDTO(patientItem)).Returns(Task.FromResult(_patientDTO));
        _patientRepository.Setup(r => r.UpsertPatient(patientItem)).Returns(Task.FromResult(patientItem));
        var sut = new PatientManager(_patientRepository.Object, _convertDTO.Object);

        // Act
        var response = await sut.UpsertPatient(_patientDTO);

        // Assert
        Assert.That(response, Is.Not.Null, "response != null");
        Assert.Multiple(() =>
        {
            Assert.That(response.GetType(), Is.EqualTo(typeof(PatientDTO)), "response.GetType()");
            Assert.That(response.Id, Is.EqualTo(patientItem.PatientId), "response.Id");
            Assert.That(response.FirstName, Is.EqualTo(patientItem.FirstName), "response.FirstName");
            Assert.That(response.LastName, Is.EqualTo(patientItem.LastName), "response.LastName");
            Assert.That(response.BirthDate, Is.EqualTo(patientItem.BirthDate), "response.BirthDate");
            Assert.That(response.GenderDescription, Is.EqualTo(patientItem.GenderDescription), "response.BirthDate");
        });
    }


    [Test(Description = "UpsertPatient throws expected exception")]
    public void UpsertPatientThrowsException()
    {
        // Arrange
        var patientItem = _patientsCollection.Collection[0];
        _patientRepository.Reset();
        _convertDTO.Reset();
        _convertDTO.Setup(c => c.ConvertToPatient(_patientDTO)).Returns(Task.FromResult(patientItem));
        _convertDTO.Setup(c => c.ConvertToPatientDTO(patientItem)).Returns(Task.FromResult(_patientDTO));
        _patientRepository.Setup(r => r.UpsertPatient(patientItem)).Throws(_expectedException!);
        var sut = new PatientManager(_patientRepository.Object, _convertDTO.Object);

        // Act
        //var response = await sut.UpsertPatient(_patientDTO);

        // Assert
        NullReferenceException? nullReferenceException = Assert.ThrowsAsync<NullReferenceException>(() => sut.UpsertPatient(_patientDTO));
        Assert.That(nullReferenceException.Message, Is.EqualTo(_expectedException.Message));
    }

    [Test(Description = "ImportPatients throws expected exception")]
    public void ImportPatientsThrowsException()
    {
        // Arrange
        var patientItem = _patientsCollection.Collection[0];
        _patientRepository.Reset();
        _convertDTO.Setup(c => c.ConvertToPatient(_patientDTO)).Returns(Task.FromResult(patientItem));
        _convertDTO.Setup(c => c.ConvertToPatientDTO(patientItem)).Returns(Task.FromResult(_patientDTO));
        _patientRepository.Setup(r => r.ImportPatients(_patientUploadTvpDTO)).Throws(_expectedException!);
        var sut = new PatientManager(_patientRepository.Object, _convertDTO.Object);

        // Act
        //var response = await sut.GetPatients(1, 10);

        // Assert
        NullReferenceException? nullReferenceException = Assert.ThrowsAsync<NullReferenceException>(() => sut.ImportPatients(_patientUploadTvpDTO));
        Assert.That(nullReferenceException.Message, Is.EqualTo(_expectedException.Message));
    }


    [Test(Description = "ImportPatients returns Success")]
    public async Task ImportPatientsSuccess()
    {
        // Arrange
        _patientRepository.Reset();
        _patientRepository.Setup(r => r.ImportPatients(_patientUploadTvpDTO)).Returns(Task.FromResult(true));
        var sut = new PatientManager(_patientRepository.Object, _convertDTO.Object);

        // Act
        var response = await sut.ImportPatients(_patientUploadTvpDTO);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(response.GetType(), Is.EqualTo(typeof(bool)), "response.GetType()");
            Assert.That(response, Is.EqualTo(true), "response");
        });
        _patientRepository.Verify(r => r.ImportPatients(_patientUploadTvpDTO), Times.Once, "Expected method to be called once");
    }
}
