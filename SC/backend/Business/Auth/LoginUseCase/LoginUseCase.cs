using backend.Dtos.Auth;
using MediatR;

namespace backend.Service.Auth.LoginUseCase;

public class LoginUseCase: IRequestHandler<LoginCommand, TokenResponse>
{
    public Task<TokenResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}