using MediatR;

namespace backend.Service.Internship.GetInternshipUseCase;

public record GetInternshipQuery(): IRequest<string>;