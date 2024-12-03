﻿using backend.Shared.Enums;

namespace backend.Service.Contracts.Internship;

public class ApplicationDto
{
    public required int Id { get; set; }
    
    public DateTime SubmissionDate { get; set; }
    
    public required ApplicationStatus ApplicationStatus { get; set; }
    
    public required InternshipDto Internship { get; set; }
}

