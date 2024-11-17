using backend.Dtos.Auth;
using MediatR;

namespace backend.Service.Auth.RegisterUseCase;

public record RegisterCommand(UserRegisterDto Dto) : IRequest<TokenResponse>;