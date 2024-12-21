using MediatR;

namespace backend.Business.Auth.VerifyMailUseCase;

public record VerifyMailCommand() : IRequest<Unit>;