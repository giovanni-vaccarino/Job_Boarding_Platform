using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using backend.Service.Contracts.Auth;
using backend.Service.Contracts.Company;
using backend.Service.Contracts.Internship;
using backend.Shared.Enums;
using FluentAssertions;
using IntegrationTests.Setup;

namespace IntegrationTests.Company;

public class CompanyIntegrationTest : IClassFixture<IntegrationTestSetup>
{
    private readonly HttpClient _client;

    public CompanyIntegrationTest(IntegrationTestSetup factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CompanyCanCreateInternshipAndAddQuestion()
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
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", loggedUser.AccessToken);
        
        //Retrieve the company profile
        var companyProfileResponse = await _client.GetAsync($"/api/company/{loggedUser.ProfileId}");
        
        companyProfileResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var companyProfile = await companyProfileResponse.Content.ReadFromJsonAsync<CompanyDto>();
        
         
        // Edit the student profile
        
        var companyInfo = new UpdateCompanyProfileDto()
        {
            Name = "Company1",
            Vat = "12345678910",
            Website = "https://www.company1.com",
        };
        
        var updateCompanyResponse = await _client.PutAsJsonAsync($"/api/company/{loggedUser.ProfileId}", companyInfo);
        
        updateCompanyResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        //Retrieve the internship of the company
        
        
        //Retrieve question of the company
        
        
    }
    
}