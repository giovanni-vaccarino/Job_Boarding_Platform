using AutoMapper;
using backend.Data;
using backend.Service.Profiles;
using backend.Shared.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace UnitTests.UseCases;

public class IsolatedUseCaseTestServices<TUseCase> where TUseCase : class
{
    public IsolatedUseCaseTestServices(string dbName)
    {
        SetupMocks();
        SetupDbContext(dbName);
        SetupSecurityContext();
    }
    
    public AppDbContext DbContext { get; private set; }
    public SecurityContext SecurityContext { get; private set; }
    public Mock<ILogger<TUseCase>> LoggerMock { get; private set; }
    public Mock<IConfiguration> ConfigurationMock { get; private set; }
    public Mock<IHttpContextAccessor> HttpContextAccessorMock { get; private set; }
    public IMapper Mapper { get; private set; }

    private void SetupDbContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase($"{dbName} - {Guid.NewGuid()}")
            .Options;

        DbContext = new AppDbContext(options);
    }

    private void SetupMocks()
    {
        ConfigurationMock = new Mock<IConfiguration>();
        LoggerMock = new Mock<ILogger<TUseCase>>();
        HttpContextAccessorMock = new Mock<IHttpContextAccessor>();
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<StudentMappingProfile>();
            // Add any other profiles you might need here
        });

        Mapper = config.CreateMapper();
    }

    private void SetupSecurityContext()
    {
        var inMemoryConfig = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                { "Jwt:Key", ")3Lh!m@}9QW$~A%{<d]N6R^p:|r_k&H8x7U+FjG#TzX5/v2B*LZsJqY01" },
                { "Jwt:ExpiryAccessToken", "60" },
                { "Jwt:ExpiryRefreshToken", "7" }
            })
            .Build();

        var securityLogger = new Mock<ILogger<SecurityContext>>();
        SecurityContext = new SecurityContext(inMemoryConfig, securityLogger.Object);
    }


}