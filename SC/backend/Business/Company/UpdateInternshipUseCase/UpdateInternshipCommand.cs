using backend.Service.Contracts.Company;
using backend.Service.Contracts.Internship;
using MediatR;

namespace backend.Business.Company.UpdateInternshipUseCase;

public record UpdateInternshipCommand(int Id, int InternshipId, UpdateInternshipDto Dto) : IRequest<InternshipDto>;