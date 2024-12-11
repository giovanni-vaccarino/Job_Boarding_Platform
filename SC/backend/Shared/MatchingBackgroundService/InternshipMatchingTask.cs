using backend.Data;
using backend.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms.Text;

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

        var students = await dbContext.Students.ToListAsync();
        if (!students.Any())
        {
            Console.WriteLine("No students found for matching.");
            return;
        }
        
        var mlContext = new MLContext();

        var results = new List<(int StudentId, double Score)>();

        foreach (var student in students)
        {
            var skillScore = CalculateSimilarity(mlContext, internship.Requirements, student.Skills);
            var interestScore = CalculateSimilarity(mlContext, internship.Requirements, student.Interests);

            // Weighted score: Skills = 2x, Interests = 1x
            var totalScore = (2 * skillScore) + interestScore;
            results.Add((student.Id, totalScore));
        }

        results = results.OrderByDescending(r => r.Score).ToList();
        
        Console.WriteLine("Matching Results:");
        Console.WriteLine("InternshipID | StudentID | Score");
        foreach (var result in results)
        {
            Console.WriteLine($"{_internshipId,12} | {result.StudentId,9} | {result.Score,5:F2}");
        }
    }
    
    private double CalculateSimilarity(MLContext mlContext, List<string> sourceList, List<string> targetList)
    {
        var allTexts = sourceList.Concat(targetList).ToList();
        
        var data = allTexts.Select(text => new TextData { Text = text }).ToList();
        
        var dataView = mlContext.Data.LoadFromEnumerable(data);
        
        // Step 1: Preprocess the text data using FeaturizeText
        var textPipeline = mlContext.Transforms.Text.ApplyWordEmbedding(
            outputColumnName: "Features",
            inputColumnName: "Text",
            modelKind: WordEmbeddingEstimator.PretrainedModelKind.GloVe50D);
        
        var transformer = textPipeline.Fit(dataView);

        var transformedData = transformer.Transform(dataView);

        var embeddings = mlContext.Data
            .CreateEnumerable<TransformedData>(transformedData, reuseRowObject: false)
            .Select(td => td.Features)
            .ToList();
        
        double similaritySum = 0;
        int similarityCount = 0;

        for (int i = 0; i < sourceList.Count; i++)
        {
            for (int j = sourceList.Count; j < allTexts.Count; j++)
            {
                similaritySum += CosineSimilarity(embeddings[i], embeddings[j]);
                similarityCount++;
            }
        }

        return similarityCount > 0 ? similaritySum / similarityCount : 0.0;
    }

    private double CosineSimilarity(float[] vector1, float[] vector2)
    {
        double dotProduct = vector1.Zip(vector2, (v1, v2) => v1 * v2).Sum();
        double magnitude1 = Math.Sqrt(vector1.Sum(v => v * v));
        double magnitude2 = Math.Sqrt(vector2.Sum(v => v * v));
        return magnitude1 > 0 && magnitude2 > 0 ? dotProduct / (magnitude1 * magnitude2) : 0.0;
    }
    
    public class TextData
    {
        [LoadColumn(0)] // Explicitly map this as the first column
        public string Text { get; set; } = string.Empty; // Initialize with default value
    }


    public class TransformedData
    {
        public string Text { get; set; }
        public float[] Features { get; set; }
    }
}