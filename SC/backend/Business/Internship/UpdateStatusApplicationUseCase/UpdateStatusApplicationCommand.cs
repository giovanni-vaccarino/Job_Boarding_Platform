using backend.Service.Contracts.Internship;
using MediatR;

namespace backend.Business.Internship.UpdateStatusApplicationUseCase;

public record UpdateStatusApplicationCommand(int ApplicationId, UpdateStatusApplicationDto Dto) : IRequest<ApplicationDto>;