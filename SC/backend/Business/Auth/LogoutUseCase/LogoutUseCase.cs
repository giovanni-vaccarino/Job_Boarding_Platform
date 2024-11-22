using MediatR;

namespace backend.Business.Auth.LogoutUseCase;

public class LogoutUseCase: IRequestHandler<LogoutCommand, Unit>
{
    public Task<Unit> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }
}