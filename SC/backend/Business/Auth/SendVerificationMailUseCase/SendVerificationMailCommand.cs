using MediatR;

namespace backend.Business.Auth.SendVerificationMailUseCase;

public record SendVerificationMailCommand(string Email) : IRequest<Unit>;