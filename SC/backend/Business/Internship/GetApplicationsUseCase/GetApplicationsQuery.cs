using backend.Service.Contracts.Internship;
using MediatR;

namespace backend.Business.Internship.GetApplicationsUseCase;

public record GetApplicationsQuery(int InternshipId) : IRequest<List<ApplicationDto>>;