using MediatR;

namespace backend.Business.Auth.LogoutUseCase;

public record LogoutCommand(): IRequest<Unit>;