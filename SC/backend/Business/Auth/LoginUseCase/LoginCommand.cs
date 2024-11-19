using backend.Service.Contracts.Auth;
using MediatR;

namespace backend.Business.Auth.LoginUseCase;

public record LoginCommand(UserLoginDto Dto) : IRequest<TokenResponse>;