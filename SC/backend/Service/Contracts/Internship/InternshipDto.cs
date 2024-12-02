using System.Runtime.InteropServices.JavaScript;
using backend.Shared.Enums;

namespace backend.Service.Contracts.Internship;

public class InternshipDto
{
    public required int Id { get; set; }
    
    public required DateTime DateCreated { get; set; }

    public required string Title { get; set; }
   
    public required DurationType Duration { get; set; }
    
    public required string Description { get; set; }
   
    public required DateOnly ApplicationDeadline { get; set; }
    
    public required string Location { get; set; }
   
    public JobCategory? JobCategory { get; set; }
   
    public JobType? JobType { get; set; }
    
    public List<string> Requirements { get; set; } = new List<string>();
}