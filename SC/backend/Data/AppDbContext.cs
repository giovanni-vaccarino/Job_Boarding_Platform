using System.Text.Json;
using backend.Data.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace backend.Data;

using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Student> Students { get; set; } = null!;
    public DbSet<Company> Companies { get; set; } = null!;
    public DbSet<Job> Jobs { get; set; } = null!;
    public DbSet<Application> Applications { get; set; } = null!;
    public DbSet<Question> Questions { get; set; } = null!;
    public DbSet<JobQuestion> JobQuestions { get; set; } = null!;
    public DbSet<Answer> Answers { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);
    
    var listValueComparer = new ValueComparer<List<string>>(
        (c1, c2) => c1 != null && c2 != null && c1.SequenceEqual(c2), 
        c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
        c => c != null ? new List<string>(c) : new List<string>());

    // User ↔ Student 
    modelBuilder.Entity<User>()
        .HasOne(u => u.Student)
        .WithOne(s => s.User)
        .HasForeignKey<Student>(s => s.UserId)
        .OnDelete(DeleteBehavior.Cascade);

    // User ↔ Company 
    modelBuilder.Entity<User>()
        .HasOne(u => u.Company)
        .WithOne(c => c.User)
        .HasForeignKey<Company>(c => c.UserId)
        .OnDelete(DeleteBehavior.Cascade);

    // Company ↔ Jobs 
    modelBuilder.Entity<Company>()
        .HasMany(c => c.Jobs)
        .WithOne(j => j.Company)
        .HasForeignKey(j => j.CompanyId)
        .OnDelete(DeleteBehavior.Cascade);

    // Company ↔ Questions
    modelBuilder.Entity<Company>()
        .HasMany(c => c.Questions)
        .WithOne(q => q.Company)
        .HasForeignKey(q => q.CompanyId)
        .OnDelete(DeleteBehavior.Cascade);

    // Job ↔ JobQuestions 
    modelBuilder.Entity<Job>()
        .HasMany(j => j.JobQuestions)
        .WithOne(jq => jq.Job)
        .HasForeignKey(jq => jq.JobId)
        .OnDelete(DeleteBehavior.Cascade);

    // Question ↔ JobQuestions 
    modelBuilder.Entity<Question>()
        .HasMany(q => q.JobQuestions)
        .WithOne(jq => jq.Question)
        .HasForeignKey(jq => jq.QuestionId)
        .OnDelete(DeleteBehavior.Cascade);

    // Application ↔ Student 
    modelBuilder.Entity<Application>()
        .HasOne(a => a.Student)
        .WithMany(s => s.Applications)
        .HasForeignKey(a => a.StudentId)
        .OnDelete(DeleteBehavior.Cascade);

    // Application ↔ Job 
    modelBuilder.Entity<Application>()
        .HasOne(a => a.Job)
        .WithMany(j => j.Applications)
        .HasForeignKey(a => a.JobId)
        .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<Application>()
        .HasIndex(a => new { a.StudentId, a.JobId })
        .IsUnique();

    modelBuilder.Entity<JobQuestion>()
        .HasIndex(jq => new { jq.JobId, jq.QuestionId })
        .IsUnique();

    modelBuilder.Entity<Student>(entity =>
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

    modelBuilder.Entity<Job>(entity =>
    {
        entity.Property(j => j.Requirements)
            .HasConversion(
                v => JsonSerializer.Serialize(v, new JsonSerializerOptions()),
                v => JsonSerializer.Deserialize<List<string>>(v, new JsonSerializerOptions()) ?? new List<string>()
            ).Metadata.SetValueComparer(listValueComparer);
    });

    modelBuilder.Entity<Answer>()
        .Property(a => a.StudentAnswer)
        .HasConversion(
            v => JsonSerializer.Serialize(v, new JsonSerializerOptions()),
            v => JsonSerializer.Deserialize<List<string>>(v, new JsonSerializerOptions()) ?? new List<string>()
        ).Metadata.SetValueComparer(listValueComparer);
}
}
