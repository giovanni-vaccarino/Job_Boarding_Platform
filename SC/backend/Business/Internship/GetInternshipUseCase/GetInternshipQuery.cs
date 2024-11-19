using MediatR;

namespace backend.Business.Internship.GetInternshipUseCase;

public record GetInternshipQuery(): IRequest<string>;