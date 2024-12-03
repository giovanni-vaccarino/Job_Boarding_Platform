using backend.Service.Contracts.Company;
using backend.Service.Contracts.Internship;
using MediatR;

namespace backend.Business.Internship.GetInternshipDetailsUseCase;

public record GetInternshipDetailsQuery(int Id): IRequest<InternshipDto>;