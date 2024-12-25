using MediatR;

namespace backend.Business.Auth.UpdatePasswordUseCase;

public record UpdatePasswordCommand(string Token, string Password) : IRequest<Unit>;