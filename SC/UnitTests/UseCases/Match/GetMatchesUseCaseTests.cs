using backend.Business.Match.GetMatchesUseCase;
using backend.Data;
using backend.Shared.Enums;

namespace UnitTests.UseCases.Match;

public class GetMatchesUseCaseTests
{
    private readonly AppDbContext _dbContext;
    private readonly IsolatedUseCaseTestServices<GetMatchesUseCase> _services;
    private readonly GetMatchesUseCase _getMatchesUseCase;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="GetMatchesUseCaseTests"/> class.
    /// Sets up the database context, the mapper and use case instance for testing.
    /// </summary>
    public GetMatchesUseCaseTests()
    {
        _services = new IsolatedUseCaseTestServices<GetMatchesUseCase>("GetMatchesUseCaseTests");
        _dbContext = _services.DbContext;
        _getMatchesUseCase = (GetMatchesUseCase)Activator.CreateInstance(
            typeof(GetMatchesUseCase), _dbContext, _services.Mapper)!;
    }
    
    // /// <summary>
    // /// Tests that student matches are correctly retrieved based on the profile ID.
    // /// </summary>
    // [Fact(DisplayName = "Successfully retrieve matches for a student")]
    // public async Task Should_Retrieve_Student_Matches_Successfully()
    // {
    //     var studentId = 1;
    //
    //     var matches = new List<backend.Data.Entities.Match>
    //     {
    //         new backend.Data.Entities.Match{HasInvite = false, StudentId = studentId, InternshipId = 100},
    //         new backend.Data.Entities.Match{HasInvite = true, StudentId = studentId, InternshipId = 200}
    //     };
    //     _dbContext.Matches.AddRange(matches);
    //     await _dbContext.SaveChangesAsync();
    //
    //     var query = new GetMatchesQuery (studentId, ProfileType.Student);
    //
    //     var result = await _getMatchesUseCase.Handle(query, CancellationToken.None);
    //
    //     Assert.NotNull(result);
    //     Assert.Equal(matches.Count, result.Count);
    //     Assert.All(result, dto => Assert.Equal(studentId, dto.Student.Id));
    // }
}