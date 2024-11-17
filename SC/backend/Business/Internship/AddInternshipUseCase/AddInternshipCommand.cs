using MediatR;

namespace backend.Services.Internship.UseCases.AddInternshipUseCase;

public record AddInternshipCommand() : IRequest<int>;