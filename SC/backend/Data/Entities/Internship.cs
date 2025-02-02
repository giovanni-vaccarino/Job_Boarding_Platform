using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Shared.Enums;

namespace backend.Data.Entities;


public class Internship : EntityBase
{
   [MaxLength(255)]
   [Required]
   public required string Title { get; set; }
   
   [Required]
   public required DurationType Duration { get; set; }
   
   [Required]
   [MaxLength(1000)]
   public required string Description { get; set; }
   
   [Required]
   public required DateOnly ApplicationDeadline { get; set; }
   
   [MaxLength(255)]
   [Required]
   public required string Location { get; set; }
   
   public JobCategory? JobCategory { get; set; }
   
   public JobType? JobType { get; set; }

   [MaxLength(255)] 
   public List<string> Requirements { get; set; } = new List<string>();

   [ForeignKey("CompanyId")]
   public required int CompanyId { get; set; }
   public required Company Company { get; set; }

   public List<Application> Applications { get; set; } = new List<Application>();
   
   public List<InternshipQuestion> InternshipQuestions { get; set; } = new List<InternshipQuestion>(); 
   
   public List<Match> Matches { get; set; } = new List<Match>();
}