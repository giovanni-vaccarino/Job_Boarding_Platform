using backend.Service.Contracts.Internship;
using MediatR;

namespace backend.Business.Internship.ApplyToInternshipUseCase;

public record ApplyToInternshipCommand(int StudentId, int InternshipId) : IRequest<ApplicationDto>;