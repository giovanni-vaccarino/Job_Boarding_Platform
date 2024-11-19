using backend.Service.Contracts.Auth;
using MediatR;

namespace backend.Business.Auth.RegisterUseCase;

public record RegisterCommand(UserRegisterDto Dto) : IRequest<TokenResponse>;