using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using backend.Service.Contracts.Auth;
using backend.Service.Contracts.Company;
using backend.Service.Contracts.Internship;
using backend.Shared.Enums;
using FluentAssertions;
using IntegrationTests.Setup;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Xunit;

namespace IntegrationTests.Internship;

public class InternshipIntegrationTest : IClassFixture<IntegrationTestSetup>
{
    private readonly HttpClient _client;

    public InternshipIntegrationTest(IntegrationTestSetup factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task InternshipCanBeCreatedAndQuestionAdded()
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters =
            {
                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
            }
        };
        // Login the company to the backend in order to retrieve the companyId
        var userLogin = new UserLoginDto()
        {
            Email = "company1@gmail.com",
            Password = "Password123!"
        };
        
        var loginResponse = await _client.PostAsJsonAsync("/api/authentication/login", userLogin);

        loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var loggedUser = await loginResponse.Content.ReadFromJsonAsync<TokenResponse>();

        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", loggedUser.AccessToken);

        // Create the internship
        var internship = new InternshipDto
        { 
            Title = "Software Engineering Internship",
            Description = "An internship for software engineering students.",
            Duration = DurationType.ThreeToSixMonths,
            ApplicationDeadline = new DateOnly(2026, 12, 31),
            DateCreated = new DateTime(2024, 12, 12),
            Location = "Remote",
            JobCategory = JobCategory.Engineering,
            JobType = JobType.FullTime,
            Requirements = new List<string> { "React", "Javascript" }
        };

        var addJobDetails = new AddJobDetailsDto()
        {
            Title = "Software Engineering Internship",
            Duration = DurationType.ThreeToSixMonths,
            Description = "An internship for software engineering students.",
            ApplicationDeadline = new DateOnly(2026, 12, 31),
            Location = "Remote",
            JobCategory = JobCategory.Engineering,
            JobType = JobType.FullTime,
            Requirements = new List<string> { "React", "Javascript" }
        };

        var questions = new AddQuestionDto()
        {
            QuestionType = QuestionType.MultipleChoice,
            Title = "What is your experience with React?",
            Options = new List<string> { "Beginner", "Intermediate", "Advanced" }
        };

        var addInternshipDto = new AddInternshipDto()
        {
            JobDetails = addJobDetails,
            Questions = new List<AddQuestionDto> { questions },
            ExistingQuestions = new List<int>()
        };

        var internshipResponse = await _client.PostAsJsonAsync($"/api/company/{loggedUser.ProfileId}/internships", addInternshipDto);

        internshipResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var createdInternship = await internshipResponse.Content.ReadFromJsonAsync<InternshipDto>(options);

        // Add a question to the internship
        var question = new QuestionDto()
        {
            QuestionType = QuestionType.TrueOrFalse,
            Title = "What is your experience with React?",
            Options = new List<string> { "True", "False" }
        };

        var questionResponse = await _client.PostAsJsonAsync("/api/question", question);

        //questionResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var createdQuestion = await questionResponse.Content.ReadFromJsonAsync<QuestionDto>(options);
        createdQuestion.Should().NotBeNull();
        createdQuestion.Title.Should().Be(question.Title);
    }
}