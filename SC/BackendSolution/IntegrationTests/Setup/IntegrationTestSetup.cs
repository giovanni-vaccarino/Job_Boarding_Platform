using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Data.Entities;
using backend.Shared.Enums;
using Microsoft.AspNetCore.Hosting;

namespace IntegrationTests.Setup;

public class IntegrationTestSetup : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
            if (descriptor != null) services.Remove(descriptor);

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase("IntegrationTestDb");
            });

            var serviceProvider = services.BuildServiceProvider();

            using (var scope = serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.Database.EnsureCreated();
                SeedDatabase(dbContext);
            }
        });
    }

    private void SeedDatabase(AppDbContext dbContext)
    {
        var user = new User
        {
            Id = 1,
            Email = "company1@gmail.com",
            Verified = true,
            PasswordHash = ""
        };
        dbContext.Add(user);

        var company = new Company
        {
            Id = 1,
            Name = "Company1",
            VatNumber = "12345678901",
            Website = "https://testcompany.com",
            UserId = 1,
            User = user
        };
        dbContext.Companies.Add(company);

        dbContext.Internships.Add(new Internship
        {
            Id = 1,
            Title = "Software Engineering Internship",
            Description = "An internship for software engineering students.",
            Duration = DurationType.ThreeToSixMonths,
            ApplicationDeadline = new DateOnly(2022, 12, 31),
            Location = "Remote",
            CompanyId = 1,
            Company = company
        });

        dbContext.SaveChanges();
    }
}