﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace backend.Data.Entities;

public class Internship : EntityBase
{
   [ForeignKey("CompanyId")]
   public required int CompanyId { get; set; }
   public required Company Company { get; set; }
   
   public List<Application>? Applications { get; set; }
   
   //Attributes of the class
   
   [MaxLength(255)]
   public string? Title { get; set; }
   
   [MaxLength(255)]
   [Range (1, int.MaxValue, ErrorMessage = "The value needs to be greater than 0")]
   public string? DurationInWeeks { get; set; }
   
   public JobCategory? JobCategory { get; set; }
   
   public JobType? JobType { get; set; }
   
   [MaxLength(255)]
   public string? Description { get; set; }
   
   [MaxLength(255)]
   public List<string>? Requirements { get; set; }
   
   public required DateOnly StartDate { get; set; }
   
   public required DateOnly EndDate { get; set; }
   
   [Range(0, int.MaxValue, ErrorMessage = "The value needs to be greater than 0")]
   public required int NumberOfApplicants { get; set; }
   
   [MaxLength(255)]
   public required string Location { get; set; }
   

}