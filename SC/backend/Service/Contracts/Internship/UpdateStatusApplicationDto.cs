using backend.Shared.Enums;

namespace backend.Service.Contracts.Internship;

public class UpdateStatusApplicationDto
{
    public ApplicationStatus Status { get; set; }
}