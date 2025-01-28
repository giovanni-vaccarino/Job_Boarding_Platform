using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using backend.Service.Contracts.Auth;
using backend.Service.Contracts.Internship;
using backend.Service.Contracts.Student;
using backend.Shared.Enums;
using FluentAssertions;
using IntegrationTests.Setup;
using Newtonsoft.Json;
using Xunit.Abstractions;
using JsonException = Newtonsoft.Json.JsonException;

namespace IntegrationTests.Student;

public class StudentIntegrationTests : IClassFixture<IntegrationTestSetup>
{
    private readonly HttpClient _client;

    public StudentIntegrationTests(IntegrationTestSetup factory)
    {
        _client = factory.CreateClient();
    }
    
    [Fact]
    public async Task StudentCanRegisterAndUpdateProfileSuccessfully()
    {
        // Register the student profile
        var studentRegistration = new UserRegisterDto
        {
            Email = "student1@test.com",
            Password = "Password123!",
            ConfirmPassword = "Password123!",
            ProfileType = ProfileType.Student
        };

        var creationResponse = await _client.PostAsJsonAsync("/api/authentication/register", studentRegistration);
        
        creationResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var createdUser = await creationResponse.Content.ReadFromJsonAsync<TokenResponse>();
        createdUser.Should().NotBeNull();
        createdUser.AccessToken.Should().NotBeNullOrEmpty();
        createdUser.RefreshToken.Should().NotBeNullOrEmpty();
        
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", createdUser.AccessToken);
        
        // Edit the student profile
        var studentInfo = new UpdateStudentDto
        {
            Name = "Gianni il Cuoco",
            Cf = "VCCGNN03H30F158G",
            Skills = new List<string> { "Javascript", "React" },
            Interests = new List<string> { "Backend", "Cloud computing" }
        };
        
        var updateResponse = await _client.PutAsJsonAsync($"/api/student/{createdUser.ProfileId}", studentInfo);
        
        updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var updatedStudent = await updateResponse.Content.ReadFromJsonAsync<StudentDto>();
        updatedStudent.Should().NotBeNull();
        updatedStudent.Cf.Should().Be(studentInfo.Cf);
        updatedStudent.Name.Should().Be(studentInfo.Name);
        updatedStudent.Skills.Should().BeEquivalentTo(studentInfo.Skills);
        updatedStudent.Interests.Should().BeEquivalentTo(studentInfo.Interests);
        
        // Load the student CV
        var emptyPdfContent = Encoding.UTF8.GetBytes("%PDF-1.4\n%%EOF");
        var content = new MultipartFormDataContent();

        var fileContent = new ByteArrayContent(emptyPdfContent);
        fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/pdf"); 
        content.Add(fileContent, "File", "cv.pdf");

        var cvLoadResponse = await _client.PostAsync($"/api/student/cv/{createdUser.ProfileId}", content);

        cvLoadResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    

    [Fact]
    public async Task StudentRetrieveInternshipDetails()
    {
        // Register the student profile
        var studentRegistration = new UserRegisterDto
        {
            Email = "student2@test.com",
            Password = "Password123!",
            ConfirmPassword = "Password123!",
            ProfileType = ProfileType.Student
        };
        
        var creationResponse = await _client.PostAsJsonAsync("/api/authentication/register", studentRegistration);

        creationResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var createdUser = await creationResponse.Content.ReadFromJsonAsync<TokenResponse>();
        createdUser.Should().NotBeNull();
        createdUser.AccessToken.Should().NotBeNullOrEmpty();
        createdUser.RefreshToken.Should().NotBeNullOrEmpty();

        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", createdUser.AccessToken);

        // Retrieve internship details
        var internshipId = 1; // Assuming an internship with ID 1 exists
        var internshipResponse = await _client.GetAsync($"/api/internship/{internshipId}");

        internshipResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task StudentCanRetrieveOwnProfile()
    {
        // Register the student profile
        var studentRegistration = new UserRegisterDto
        {
            Email = "student2@test.com",
            Password = "Password123!",
            ConfirmPassword = "Password123!",
            ProfileType = ProfileType.Student
        };
        
        var creationResponse = await _client.PostAsJsonAsync("/api/authentication/register", studentRegistration);

        creationResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var createdUser = await creationResponse.Content.ReadFromJsonAsync<TokenResponse>();
        createdUser.Should().NotBeNull();
        createdUser.AccessToken.Should().NotBeNullOrEmpty();
        createdUser.RefreshToken.Should().NotBeNullOrEmpty();

        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", createdUser.AccessToken);

        // Retrieve internship details
        var studentId = createdUser.ProfileId; // Assuming an internship with ID 1 exists
        var studentResponse = await _client.GetAsync($"/api/student/{studentId}");

        studentResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var student = await studentResponse.Content.ReadFromJsonAsync<StudentDto>();
    }
}