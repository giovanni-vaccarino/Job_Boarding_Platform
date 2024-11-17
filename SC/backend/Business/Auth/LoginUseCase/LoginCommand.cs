using backend.Dtos.Auth;
using MediatR;

namespace backend.Service.Auth.LoginUseCase;

public record LoginCommand(UserLoginDto Dto) : IRequest<TokenResponse>;