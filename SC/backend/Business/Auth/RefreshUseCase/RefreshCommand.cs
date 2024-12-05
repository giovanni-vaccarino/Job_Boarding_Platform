using backend.Service.Contracts.Auth;
using MediatR;

namespace backend.Business.Auth.RefreshUseCase;

public record RefreshCommand(RefreshTokenDto Dto): IRequest<TokenResponse>;