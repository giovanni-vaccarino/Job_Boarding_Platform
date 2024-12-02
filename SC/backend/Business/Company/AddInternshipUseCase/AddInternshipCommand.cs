using backend.Service.Contracts.Company;
using backend.Service.Contracts.Internship;
using MediatR;

namespace backend.Business.Company.AddInternshipUseCase;

public record AddInternshipCommand(int Id, AddInternshipDto Dto) : IRequest<InternshipDto>;