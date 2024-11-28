using System.ComponentModel.DataAnnotations;

namespace backend.Service.Contracts.Company;

public class ApplicantsToInternshipDto
{
    //TODO it's just a list of applicants could be useless
    [Required]
    public List<SingleApplicantToInternshipDto> Applicants { get; set; }
}