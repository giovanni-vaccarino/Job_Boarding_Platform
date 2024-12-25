using MediatR;

namespace backend.Business.Auth.VerifyMailUseCase;

public record VerifyMailCommand(string VerificationToken) : IRequest<Unit>;