﻿using System.Runtime.InteropServices.JavaScript;
using backend.Service.Contracts.Feedback;
using backend.Shared.Enums;

namespace backend.Service.Contracts.Internship;

public class InternshipDto
{
    public int Id { get; set; }
    
    public required DateTime DateCreated { get; set; }

    public required string Title { get; set; }
   
    public required DurationType Duration { get; set; }
    
    public required string Description { get; set; }
   
    public required DateOnly ApplicationDeadline { get; set; }
    
    public required string Location { get; set; }
   
    public JobCategory? JobCategory { get; set; }
   
    public JobType? JobType { get; set; }
    
    public List<string> Requirements { get; set; } = new List<string>();
    
    public int NumberOfApplications { get; set; }
    
    
    public required int companyId { get; set; }
    
    public string? CompanyName { get; set; }
    
    public List<FeedbackResponseDto>? Feedbacks { get; set; }
}