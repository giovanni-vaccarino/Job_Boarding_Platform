using MediatR;

namespace backend.Business.Internship.AddInternshipUseCase;

public record AddInternshipCommand() : IRequest<int>;