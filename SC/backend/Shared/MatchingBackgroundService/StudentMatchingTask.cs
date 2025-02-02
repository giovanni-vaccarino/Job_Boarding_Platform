using backend.Data;
using backend.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace backend.Shared.MatchingBackgroundService;

public class StudentMatchingTask : IBackgroundTask
{
    private readonly int _studentId;
    private readonly IServiceProvider _serviceProvider;

    public StudentMatchingTask(int studentId, IServiceProvider serviceProvider)
    {
        _studentId = studentId;
        _serviceProvider = serviceProvider;
    }

    
    public async Task ExecuteAsync()
    {
        await Task.Delay(15000);

        Console.WriteLine($"Performing statistical analysis for student {_studentId}");
        
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var student = await dbContext.Students
            .Select(s => new { s.Id, s.Skills, s.Interests })
            .Where(s => s.Id == _studentId)
            .FirstOrDefaultAsync();

        if (student == null)
        {
            Console.WriteLine($"No student found with ID: {_studentId}");
            return;
        }
        
        var internships = await dbContext.Internships
            .Select(i => new { i.Id, i.Requirements, i.CompanyId })
            .ToListAsync();
        
        if (!internships.Any())
        {
            Console.WriteLine("No internships found for matching.");
            return;
        }
        
        const double threshold = 3.0;
        
        var internshipScores = internships
            .Select(internship =>
            {
                var avgCompanyScore = dbContext.Applications
                    .Where(a => a.Internship.CompanyId == internship.CompanyId)
                    .SelectMany(a => a.InternshipFeedbacks)
                    .Where(f => f.Actor == ProfileType.Student) 
                    .Average(f => (double?)((int)f.Rating + 1)) ?? 2.5;
                
                double similarityScore = CalculateSimilarityScore(internship.Requirements, student.Skills, student.Interests);
                double finalScore = similarityScore * (avgCompanyScore * 2 / 5);

                return (InternshipId: internship.Id, Score: finalScore);
            })
            .OrderByDescending(s => s.Score)
            .ToList();

        Console.WriteLine("Scores for all internships:");
        foreach (var internshipScore in internshipScores)
        {
            Console.WriteLine($"Internship ID: {internshipScore.InternshipId}, Score: {internshipScore.Score:F2}");
        }

        var matchedInternships = internshipScores
            .Where(s => s.Score > threshold)
            .Take(10)
            .Select(s => s.InternshipId)
            .ToList();

        Console.WriteLine($"Matched internships for student ID {_studentId}: {string.Join(", ", matchedInternships)}");

        if (matchedInternships.Count > 0)
        {
            var existingMatches = await dbContext.Matches
                .Where(m => m.StudentId == _studentId && matchedInternships.Contains(m.InternshipId))
                .Select(m => new { m.StudentId, m.InternshipId })
                .ToListAsync();

            var matchesToInsert = matchedInternships
                .Where(internshipId => !existingMatches.Any(e => e.InternshipId == internshipId && e.StudentId == _studentId))
                .Select(internshipId => new Data.Entities.Match
                {
                    InternshipId = internshipId,
                    StudentId = _studentId,
                    HasInvite = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }).ToList();

            if (matchesToInsert.Any())
            {
                await dbContext.Matches.AddRangeAsync(matchesToInsert);
                await dbContext.SaveChangesAsync();
            }
        }
    }
    
    private static double CalculateSimilarityScore(List<string> requirements, List<string> studentSkills, List<string> studentInterests)
    {
        const double skillWeight = 2.0; 
        const double interestWeight = 1.0;
        const double similarityThreshold = 0.8;

        int skillMatches = CountSimilarItems(requirements, studentSkills, similarityThreshold);
        int interestMatches = CountSimilarItems(requirements, studentInterests, similarityThreshold);

        return (skillMatches * skillWeight) + (interestMatches * interestWeight);
    }
    
    private static int CountSimilarItems(List<string> list1, List<string> list2, double similarityThreshold)
    {
        int count = 0;
        foreach (var item1 in list1)
        {
            foreach (var item2 in list2)
            {
                if (GetSimilarity(item1, item2) >= similarityThreshold)
                {
                    count++;
                    break;
                }
            }
        }
        return count;
    }

    private static double GetSimilarity(string str1, string str2)
    {
        str1 = str1.ToLower();
        str2 = str2.ToLower();
        
        if (str1 == str2) return 1.0;
        if (str1.Contains(str2) || str2.Contains(str1)) return 0.8;

        return 0.0;
    }
}