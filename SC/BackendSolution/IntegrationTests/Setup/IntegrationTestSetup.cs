using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Data.Entities;
using backend.Shared.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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

    private async void SeedDatabase(AppDbContext dbContext)
    {
        
        var modelBuilder = new ModelBuilder();
        ConfigureCustomComparers(modelBuilder);
        
        
        // Password123!
        var user0 = new User
        {
            Email = "student1@gmail.com",
            Verified = true,
            PasswordHash = "$2a$11$IX5tDdmE76eWqifK3mlC9uSkQtEZNF2vDVHKkTs.pA4t2nkn8VnZ2"
        };
        dbContext.Add(user0);
        await dbContext.SaveChangesAsync();
        
        var student0 = new backend.Data.Entities.Student
        {
            Name = "Student1",
            Cf = "VCCGNN03H30F158G",
            UserId = user0.Id,
            User = user0,
            CvPath = "https://testcv.com",
            Skills = new List<string> { "Javascript", "React" },
            Interests = new List<string> { "Backend", "Cloud computing" }
        };
        dbContext.Add(student0);
        await dbContext.SaveChangesAsync();
        
        var user = new User
        {
            Email = "company1@gmail.com",
            Verified = true,
            PasswordHash = "$2a$11$IX5tDdmE76eWqifK3mlC9uSkQtEZNF2vDVHKkTs.pA4t2nkn8VnZ2"
        };
        dbContext.Add(user);
        await dbContext.SaveChangesAsync();

        var company = new Company
        {
            Name = "Company1",
            VatNumber = "12345678901",
            Website = "https://testcompany.com",
            UserId = user.Id,
            User = user
        };
        dbContext.Companies.Add(company);
        await dbContext.SaveChangesAsync();
        

        var internship0 = new backend.Data.Entities.Internship
        {
            Title = "Software Engineering Internship",
            Description = "An internship for software engineering students.",
            Duration = DurationType.ThreeToSixMonths,
            ApplicationDeadline = new DateOnly(2026, 12, 31),
            Location = "Remote",
            CompanyId = company.Id,
            Company = company
        };
        
        dbContext.Internships.Add(internship0);
        await dbContext.SaveChangesAsync();
        
        var internship1 = new backend.Data.Entities.Internship
        {
            Title = "Data Science Internship",
            Description = "An internship for data science students.",
            Duration = DurationType.ThreeToSixMonths,
            ApplicationDeadline = new DateOnly(2026, 12, 31),
            Location = "Remote",
            CompanyId = company.Id,
            Company = company
        };
        
        
        dbContext.Internships.Add(internship1);
        await dbContext.SaveChangesAsync();

        var Application0 = new Application
        {
            InternshipId = internship0.Id,
            Internship = internship0,
            StudentId = student0.Id,
            Student = student0,
            ApplicationStatus = ApplicationStatus.Accepted
        };
        
        dbContext.Applications.Add(Application0);
        await dbContext.SaveChangesAsync();
        
        var Application1 = new Application
        {
            InternshipId = internship1.Id,
            Internship = internship1,
            StudentId = student0.Id,
            Student = student0,
            ApplicationStatus = ApplicationStatus.Screening
        };
        
        dbContext.Applications.Add(Application1);
        await dbContext.SaveChangesAsync();
        
        var question0 = new Question()
        {
            Title = "What is your experience with React?",
            Type = QuestionType.OpenQuestion,
            CompanyId = company.Id,
            Company = company
        };
        
        dbContext.Questions.Add(question0);
        await dbContext.SaveChangesAsync();
        
        var question1 = new Question()
        {
            Title = "What is your experience with Python?",
            Type = QuestionType.TrueOrFalse,
            Options = new List<string> { "True", "False" },
            CompanyId = company.Id,
            Company = company,
            InternshipQuestions = new List<InternshipQuestion>()
        };
        
        dbContext.Questions.Add(question1);
        await dbContext.SaveChangesAsync();
        
        var internshipQuestion0 = new InternshipQuestion()
        {
            InternshipId = internship0.Id,
            Internship = internship0,
            QuestionId = question0.Id,
            Question = question0
        };
        
        dbContext.InternshipQuestions.Add(internshipQuestion0);
        await dbContext.SaveChangesAsync();
        
        var internshipQuestion1 = new InternshipQuestion()
        {
            InternshipId = internship1.Id,
            Internship = internship1,
            QuestionId = question1.Id,
            Question = question1
        };
        
        dbContext.InternshipQuestions.Add(internshipQuestion1);
        await dbContext.SaveChangesAsync();
        
        


    }
    
    private void ConfigureCustomComparers(ModelBuilder modelBuilder)
    {
        var listValueComparer = new ValueComparer<List<string>>(
            (c1, c2) => c1 != null && c2 != null && c1.SequenceEqual(c2),
            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            c => c != null ? new List<string>(c) : new List<string>()
        );

        // Configure for `Skills` and `Interests`
        modelBuilder.Entity<backend.Data.Entities.Student>(entity =>
        {
            entity.Property(s => s.Skills)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, new JsonSerializerOptions()),
                    v => JsonSerializer.Deserialize<List<string>>(v, new JsonSerializerOptions()) ?? new List<string>()
                ).Metadata.SetValueComparer(listValueComparer);

            entity.Property(s => s.Interests)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, new JsonSerializerOptions()),
                    v => JsonSerializer.Deserialize<List<string>>(v, new JsonSerializerOptions()) ?? new List<string>()
                ).Metadata.SetValueComparer(listValueComparer);
        });

        modelBuilder.Entity<backend.Data.Entities.Internship>(entity =>
        {
            entity.Property(j => j.Requirements)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, new JsonSerializerOptions()),
                    v => JsonSerializer.Deserialize<List<string>>(v, new JsonSerializerOptions()) ?? new List<string>()
                ).Metadata.SetValueComparer(listValueComparer);
        });
        
        modelBuilder.Entity<backend.Data.Entities.Question>(entity =>
        {
            entity.Property(q => q.Options)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, new JsonSerializerOptions()),
                    v => JsonSerializer.Deserialize<List<string>>(v, new JsonSerializerOptions()) ?? new List<string>()
                ).Metadata.SetValueComparer(listValueComparer);
        });
    }

}