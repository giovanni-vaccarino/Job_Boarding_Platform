using System.ComponentModel.DataAnnotations;
using backend.Shared.Enums;

namespace backend.Service.Contracts.Company;

    public class AddJobDetailsCompanyDto
    {
        [Required]
        [MaxLength(255)]
        public required string JobTitle { get; set; }
        
        [Required]
        [MaxLength(255)]
        public required string JobLocation { get; set; }
        
        [Required]
        [DataType(DataType.DateTime)]
        public required DateTime ApplicationDeadline { get; set; }
        
        [Required]
        [MaxLength(255)]
        public required string JobDescription { get; set; }
        
        [Required]
        [MaxLength(255)]
        public JobCategory JobCategory { get; set; }
        
        [Required]
        [MaxLength(255)]
        public JobType JobType { get; set; } 
    }