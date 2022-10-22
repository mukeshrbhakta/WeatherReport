using JbHifi.WeatherReport.DataLibrary.Implementations;
using JbHifi.WeatherReport.DataLibrary.Interfaces;
using JbHifi.WeatherReport.DataLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace JbHifi.WeatherReport.UnitTests.DataLibrary;

[TestClass]
public class AuditRepositoryTests
{
    private Mock<IWeatherReportDbFactory>? _weatherReportDbFactory;
    private Mock<WeatherReportDbContext>? _weatherReportDbContext;
    private Mock<DbSet<Audit>>? _auditDbSet;
    private const int AuditId1 = 1;
    private const int AuditId2 = 2;
    private const int AuditId3 = 3;
    private const int WeatherReportApiKeysId1 = 1;
    private const int WeatherReportApiKeysId2 = 2;
    private const int NewId = 100;


    [TestInitialize]
    public void Setup()
    {
        _weatherReportDbFactory = new Mock<IWeatherReportDbFactory>();
        _weatherReportDbContext = new Mock<WeatherReportDbContext>();
        _weatherReportDbFactory.Setup(a => a.GetDbContext()).Returns(_weatherReportDbContext.Object);

        var auditList = new List<Audit>()
        {
            new ()
            {
                Id = AuditId1,
                Weatherreportapikeysid = WeatherReportApiKeysId1,
                Createdby = "some user",
                Createddate = DateTime.UtcNow.AddMinutes(-2),
                Updatedby = "some user",
                Updateddate = DateTime.UtcNow
            },
            new ()
            {
                Id = AuditId2,
                Weatherreportapikeysid = WeatherReportApiKeysId1,
                Createdby = "some user",
                Createddate = DateTime.UtcNow.AddMinutes(-1),
                Updatedby = "some user",
                Updateddate = DateTime.UtcNow
            },
            new ()
            {
                Id = AuditId3,
                Weatherreportapikeysid = WeatherReportApiKeysId2,
                Createdby = "some user",
                Createddate = DateTime.UtcNow.AddMinutes(-1),
                Updatedby = "some user",
                Updateddate = DateTime.UtcNow
            }
        }.AsQueryable();

        _auditDbSet = new Mock<DbSet<Audit>>();
        _auditDbSet.As<IQueryable<Audit>>().Setup(m => m.Provider).Returns(auditList.Provider);
        _auditDbSet.As<IQueryable<Audit>>().Setup(m => m.Expression).Returns(auditList.Expression);
        _auditDbSet.As<IQueryable<Audit>>().Setup(m => m.ElementType).Returns(auditList.ElementType);
        _auditDbSet.As<IQueryable<Audit>>().Setup(m => m.GetEnumerator()).Returns(() => auditList.GetEnumerator());
        
        _weatherReportDbContext.Setup(a => a.Audits).Returns(_auditDbSet.Object!);

        _weatherReportDbContext.Setup(a => a.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(NewId);
    }
    
    [TestMethod]
    public async Task GetForWeatherReportApiKeyIdForPastHour_Success()
    {
        // Arrange 
        var auditRepository = new AuditRepository(_weatherReportDbFactory!.Object);

        // Act 
        var response = await auditRepository.GetForWeatherReportApiKeyIdForPastHour(WeatherReportApiKeysId1);

        // Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(2, response.Count());
    }
    
    [TestMethod]
    public async Task GetForWeatherReportApiKeyIdForPastHour_NonExistingId_Success()
    {
        // Arrange 
        var auditRepository = new AuditRepository(_weatherReportDbFactory!.Object);

        // Act 
        var response = await auditRepository.GetForWeatherReportApiKeyIdForPastHour(4);

        // Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(0, response.Count());
    }
    
    [TestMethod]
    public async Task Add_Success()
    {
        // Arrange 
        var auditRepository = new AuditRepository(_weatherReportDbFactory!.Object);
        var record = new Audit()
        {
            Id = NewId,
            Weatherreportapikeysid = WeatherReportApiKeysId2,
            Createdby = "some user",
            Createddate = DateTime.UtcNow.AddMinutes(-1),
            Updatedby = "some user",
            Updateddate = DateTime.UtcNow
        };
        
        // Act 
        var response = await auditRepository.Add(record);

        // Assert 
        Assert.AreEqual(NewId, response);
    }
    
    [TestMethod]
    [ExpectedException(typeof(NullReferenceException))]
    public async Task Add_Null_Fail()
    {
        // Arrange 
        var auditRepository = new AuditRepository(_weatherReportDbFactory!.Object);
        
        // Act 
        await auditRepository.Add(null);

        // Assert 
        Assert.IsTrue(false); // never get here
    }

}