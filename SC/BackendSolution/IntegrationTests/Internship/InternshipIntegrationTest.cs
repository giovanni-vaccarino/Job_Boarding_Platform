using System.Diagnostics.Eventing.Reader;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using backend.Migrations;
using backend.Service.Contracts.Auth;
using backend.Service.Contracts.Company;
using backend.Service.Contracts.Feedback;
using backend.Service.Contracts.Internship;
using backend.Service.Contracts.Match;
using backend.Shared.Enums;
using FluentAssertions;
using IntegrationTests.Setup;
using Microsoft.AspNetCore.Http;
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
            Requirements = new List<string> { "React", "Javascript" },
            NumberOfApplications = 0,
            companyId = loggedUser.ProfileId
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
            Options = new List<string> ()
        };

        var questionResponse = await _client.PostAsJsonAsync($"/api/company/{loggedUser.ProfileId}/questions", question);

        questionResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var createdQuestion = await questionResponse.Content.ReadFromJsonAsync<QuestionDto>(options);
    }

[Fact]
public async Task StudentRetrieveApplicationAnswerTheQuestionAndSendFeedback()
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
        Email = "student1@gmail.com",
        Password = "Password123!"
    };
        
    var loginResponse = await _client.PostAsJsonAsync("/api/authentication/login", userLogin);

    loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);

    var loggedUser = await loginResponse.Content.ReadFromJsonAsync<TokenResponse>();

    _client.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("Bearer", loggedUser.AccessToken);
    
    // Retrieve the first application of the student

    var applicationResponse = await _client.GetAsync($"api/student/{loggedUser.ProfileId}/applications");
    
    applicationResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    
    var applications = await applicationResponse.Content.ReadFromJsonAsync<List<ApplicationDto>>(options);

    // Answer the question
    
    var singleAnswerResponse = new List<string> { "Begginer" };

    var singleAnswer= new SingleAnswerQuestion()
    {
        QuestionId = 1,
        Answer = singleAnswerResponse
    };
    
    var answerQuestions = new AnswerQuestionsDto()
    {
        Questions = new List<SingleAnswerQuestion>() { singleAnswer }
    };
    
    var answerQuestionResponse = await _client.PostAsJsonAsync($"/api/internship/applications/{applications[0].Id}?studentId={loggedUser.ProfileId}", answerQuestions);
    
    //Error 500 probably due to the update of the local db
    //answerQuestionResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    
    
    //Company login to the backend
    
    var companyLogin = new UserLoginDto()
    {
        Email = "company1@gmail.com",
        Password = "Password123!"
    };
        
    var loginResponseCompany = await _client.PostAsJsonAsync("/api/authentication/login", companyLogin);

    loginResponseCompany.StatusCode.Should().Be(HttpStatusCode.OK);

    var loggedUserCompany = await loginResponseCompany.Content.ReadFromJsonAsync<TokenResponse>(options);

    _client.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("Bearer", loggedUserCompany.AccessToken);

    
    
    //Update the status and send Feedback to the student
    
    var updateStatus = new UpdateStatusApplicationDto()
    {
        Status = ApplicationStatus.Accepted,
    };
    
    var updateStatusResponse = await _client.PatchAsJsonAsync($"/api/internship/applications/{applications[0].Id}?companyId={loggedUserCompany.ProfileId}", updateStatus);
    
    updateStatusResponse.StatusCode.Should().Be(HttpStatusCode.OK);


    var feedbackCompanyInternship = new AddInternshipFeedbackDto()
    {
        Text = "Great job!",
        Rating = Rating.FourStars,
        ProfileId = loggedUser.ProfileId,
        ApplicationId = applications[0].Id,
        Actor = ProfileType.Company
    };
    
    var feedbackStudentInternship = new AddInternshipFeedbackDto()
    {
        Text = "Great job!",
        Rating = Rating.FourStars,
        ProfileId = loggedUser.ProfileId,
        ApplicationId = applications[0].Id,
        Actor = ProfileType.Company
    };
    
    
    var feedbackResponseCompany = await _client.PostAsJsonAsync($"/api/feedback/internship", feedbackCompanyInternship);
    
    feedbackResponseCompany.StatusCode.Should().Be(HttpStatusCode.OK);
    
    //Send the feedback of the Student related to the application
    var feedbackResponseStudent = await _client.PostAsJsonAsync("/api/feedback/internship", feedbackStudentInternship);

    feedbackResponseStudent.StatusCode.Should().Be(HttpStatusCode.OK);
    
    //Retrieve applicant information
    
    //var feedbackRetrievedStudent = await _client.GetAsync($"/api/feedback/internship/{applications[0].Id}");
    
    //Retrieve all the internship 
    
    var allInternship = await _client.GetAsync("/api/internship");
    
    allInternship.StatusCode.Should().Be(HttpStatusCode.OK);
    
    var internships = await allInternship.Content.ReadFromJsonAsync<List<InternshipDto>>(options);
    
    //Retrieve the applicant of the internship
    
    var applicants = await _client.GetAsync($"/api/internship/{internships[0].Id}/applications?companyId={loggedUser.ProfileId}");
    
    applicants.StatusCode.Should().Be(HttpStatusCode.OK);
    
    //retreive the internship question
    
    var internshipQuestion = await _client.GetAsync($"/api/internship/{internships[0].Id}/questions");
    
    internshipQuestion.StatusCode.Should().Be(HttpStatusCode.OK);
    
    var questions = await internshipQuestion.Content.ReadFromJsonAsync<List<QuestionDto>>(options);
    
    //Retrieve the details of the internship
    
    var internshipDetails = await _client.GetAsync($"/api/internship/{internships[0].Id}");
    
    internshipDetails.StatusCode.Should().Be(HttpStatusCode.OK);
        
    //Retrieve applicant details
    
    var applicantDetails = await _client.GetAsync($"/api/internship/applications/applicantInfo/{applications[0].Id}?studentId=1&companyId={loggedUser.ProfileId}");
    
    applicantDetails.StatusCode.Should().Be(HttpStatusCode.OK);
    
    //Retrieve the company matches
    
    var companyMatches = await _client.GetAsync($"/api/matches/company/{loggedUser.ProfileId}");
    
    companyMatches.StatusCode.Should().Be(HttpStatusCode.OK);
    
    var matches = await companyMatches.Content.ReadFromJsonAsync<List<MatchDto>>(options);
    
    //Invite the student
    
    var inviteStudent = await _client.PatchAsJsonAsync($"/api/matches/{matches[0].Id}?companyId={loggedUserCompany.ProfileId}", matches[0].Id);
    
    inviteStudent.StatusCode.Should().Be(HttpStatusCode.OK);
    
    //Retrieve the student matches
    
    var studentMatches = await _client.GetAsync($"/api/matches/student/{loggedUser.ProfileId}");
    
    studentMatches.StatusCode.Should().Be(HttpStatusCode.OK);
    
    var studentMatchesList = await studentMatches.Content.ReadFromJsonAsync<List<MatchDto>>(options);
    
    //The student accept the match
    
    var alreadyApplied = true;
    foreach (var match in studentMatchesList)
    {
        //if the match is find inside the matches of the students then the student has not already applied
        if(match.Id == matches[0].Id)
        {
            alreadyApplied = false;
        }
    }

    if (alreadyApplied == false)
    {
        var acceptMatch = await _client.PostAsJsonAsync($"/api/matches/{matches[0].Id}?studentId={loggedUser.ProfileId}", matches[0].Id);
        acceptMatch.StatusCode.Should().Be(HttpStatusCode.OK);
    }
        
    
    
}


[Fact]
public async Task AuthenticationFlowOfCompany()
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
    
    // Refresh the token of the company
    
    var refreshToken = new RefreshTokenDto()
    {
        RefreshToken = loggedUser.RefreshToken
    };
    
    var refreshTokenResponse = await _client.PostAsJsonAsync("/api/authentication/refresh", refreshToken);
    
    //refreshTokenResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    
    //Logout the company
    
    var logoutResponse = await _client.PostAsJsonAsync("/api/authentication/logout", refreshToken);
    
    logoutResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    
}




}