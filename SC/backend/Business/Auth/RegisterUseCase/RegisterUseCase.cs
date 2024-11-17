using backend.Dtos.Auth;
using MediatR;

namespace backend.Service.Auth.RegisterUseCase;

public class RegisterUseCase: IRequestHandler<RegisterCommand, TokenResponse>
{
    public Task<TokenResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}