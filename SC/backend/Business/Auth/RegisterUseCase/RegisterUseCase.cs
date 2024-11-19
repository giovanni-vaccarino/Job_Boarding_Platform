using backend.Service.Contracts.Auth;
using MediatR;

namespace backend.Business.Auth.RegisterUseCase;

public class RegisterUseCase: IRequestHandler<RegisterCommand, TokenResponse>
{
    public Task<TokenResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}