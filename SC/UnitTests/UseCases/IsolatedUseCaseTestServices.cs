﻿using AutoMapper;
using backend.Data;
using backend.Data.Entities;
using backend.Service.Profiles;
using backend.Shared.EmailService;
using backend.Shared.MatchingBackgroundService;
using backend.Shared.Security;
using backend.Shared.StorageService;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
        SetupServiceProvider();
    }
    
    public AppDbContext DbContext { get; private set; }
    public SecurityContext SecurityContext { get; private set; }
    public Mock<ILogger<TUseCase>> LoggerMock { get; private set; }
    public Mock<IConfiguration> ConfigurationMock { get; private set; }
    public Mock<IHttpContextAccessor> HttpContextAccessorMock { get; private set; }
    public IMapper Mapper { get; private set; }
    
    public Mock<IJobQueue> JobQueue { get; private set; }
    
    public Mock<IMediator> MediatorMock { get; private set; }
    
    public IInternshipMatchingTaskFactory InternshipMatchingTaskFactory { get; private set; }
    
    public IStudentMatchingTaskFactory StudentMatchingTaskFactory { get; private set; }
    
    public IServiceProvider ServiceProvider { get; private set; }
    
    public Mock<IS3Manager> S3ManagerMock { get; private set; }
    
    public Mock<IEmailService> EmailServiceMock { get; private set; }


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
        S3ManagerMock = new Mock<IS3Manager>();
        S3ManagerMock
            .Setup(s3 => s3.UploadFileAsync(It.IsAny<Stream>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);
        MediatorMock = new Mock<IMediator>();
        EmailServiceMock = new Mock<IEmailService>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<StudentMappingProfile>();
            cfg.AddProfile<CompanyMappingProfile>();
            cfg.AddProfile<InternshipMappingProfile>();
            cfg.AddProfile<QuestionMappingProfile>();
            cfg.AddProfile<ApplicationMappingProfile>();
        });

        Mapper = config.CreateMapper();
    }
    
    private void SetupServiceProvider()
    {
        JobQueue = new Mock<IJobQueue>();
        var services = new ServiceCollection();

        services.AddSingleton(DbContext);

        services.AddSingleton(ConfigurationMock.Object);
        services.AddSingleton(LoggerMock.Object);
        services.AddSingleton(HttpContextAccessorMock.Object);
        services.AddSingleton(S3ManagerMock.Object);

        services.AddSingleton(SecurityContext);

        services.AddSingleton<IInternshipMatchingTaskFactory, InternshipMatchingTaskFactory>();
        services.AddSingleton<IStudentMatchingTaskFactory, StudentMatchingTaskFactory>();
        services.AddSingleton<MatchingBackgroundService>();
        services.AddSingleton<IJobQueue>(provider => 
            provider.GetRequiredService<MatchingBackgroundService>());
        
        services.AddSingleton(provider => 
            provider.GetRequiredService<MatchingBackgroundService>());

        services.AddSingleton(Mapper);

        ServiceProvider = services.BuildServiceProvider();

        // Resolve Dependencies
        InternshipMatchingTaskFactory = ServiceProvider.GetRequiredService<IInternshipMatchingTaskFactory>();
        StudentMatchingTaskFactory = ServiceProvider.GetRequiredService<IStudentMatchingTaskFactory>();
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