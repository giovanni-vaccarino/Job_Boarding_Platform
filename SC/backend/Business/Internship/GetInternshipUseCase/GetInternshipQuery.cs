using backend.Service.Contracts.Internship;
using MediatR;

namespace backend.Business.Internship.GetInternshipUseCase;

public record GetInternshipQuery(): IRequest<List<InternshipDto>>;