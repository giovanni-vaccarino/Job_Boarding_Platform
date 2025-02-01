using backend.Data;
using Microsoft.EntityFrameworkCore;

namespace backend.Shared.MatchingBackgroundService;

public class InternshipMatchingTask : IBackgroundTask
{
    private readonly int _internshipId;
    private readonly IServiceProvider _serviceProvider;

    public InternshipMatchingTask(int internshipId, IServiceProvider serviceProvider)
    {
        _internshipId = internshipId;
        _serviceProvider = serviceProvider;
    }
    
    public async Task ExecuteAsync()
    {
        await Task.Delay(15000);
        Console.WriteLine($"Starting matching for internship ID: {_internshipId}");

        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var internship = await dbContext.Internships
            .Where(i => i.Id == _internshipId)
            .FirstOrDefaultAsync();

        if (internship == null)
        {
            Console.WriteLine($"No internship found with ID: {_internshipId}");
            return;
        }

        var students = await dbContext.Students
            .Select(s => new { s.Id, s.Skills, s.Interests })
            .ToListAsync();
        
        if (!students.Any())
        {
            Console.WriteLine("No students found for matching.");
            return;
        }
        
        var internshipRequirements = internship.Requirements;
        const double threshold = 3.0;
        
        var studentScores = students
            .Select(student => new
            {
                StudentId = student.Id,
                Score = CalculateSimilarityScore(internshipRequirements, student.Skills, student.Interests)
            })
            .OrderByDescending(s => s.Score)
            .ToList();

        Console.WriteLine("Scores for all students:");
        foreach (var studentScore in studentScores)
        {
            Console.WriteLine($"Student ID: {studentScore.StudentId}, Score: {studentScore.Score:F2}");
        }

        
        
        var matchedStudents = studentScores
            .Where(s => s.Score > threshold)
            .Take(10)
            .Select(s => s.StudentId)
            .ToList();

        Console.WriteLine($"Matched students for internship ID {_internshipId}: {string.Join(", ", matchedStudents)}");

        if (matchedStudents.Count > 0)
        {
            var existingMatches = await dbContext.Matches
                .Where(m => m.InternshipId == internship.Id && matchedStudents.Contains(m.StudentId))
                .Select(m => new { m.StudentId, m.InternshipId })
                .ToListAsync();

            var matchesToInsert = matchedStudents
                .Where(studentId => !existingMatches.Any(e => e.StudentId == studentId && e.InternshipId == internship.Id))
                .Select(studentId => new Data.Entities.Match
                {
                    InternshipId = internship.Id,
                    StudentId = studentId,
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