using backend.Service.Contracts.Internship;
using MediatR;

namespace backend.Business.Student.GetApplicationsUseCase;

public record GetApplicationsQuery(int Id) : IRequest<List<ApplicationDto>>;