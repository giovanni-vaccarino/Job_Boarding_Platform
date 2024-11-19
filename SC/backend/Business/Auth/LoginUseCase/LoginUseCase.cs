using backend.Service.Contracts.Auth;
using MediatR;

namespace backend.Business.Auth.LoginUseCase;

public class LoginUseCase: IRequestHandler<LoginCommand, TokenResponse>
{
    public Task<TokenResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}