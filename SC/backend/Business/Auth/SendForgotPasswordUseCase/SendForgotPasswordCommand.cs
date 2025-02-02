using MediatR;

namespace backend.Business.Auth.SendForgotPasswordUseCase;

public record SendForgotPasswordCommand(string Email) : IRequest<Unit>;