using System.ComponentModel.DataAnnotations;

namespace backend.Service.Contracts.Company;

public class MatchDetailsDto
{
    //TODO it's just a list of applicants could be useless
    [Required]
    public List<SingleMatchesDetails> matches { get; set; }
}