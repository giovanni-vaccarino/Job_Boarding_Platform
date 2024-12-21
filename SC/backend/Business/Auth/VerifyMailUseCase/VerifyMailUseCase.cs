using MediatR;

namespace backend.Business.Auth.VerifyMailUseCase;

public class VerifyMailUseCase : IRequestHandler<VerifyMailCommand, Unit>
{
    public async Task<Unit> Handle(VerifyMailCommand request, CancellationToken cancellationToken)
    {
        return Unit
    }
}