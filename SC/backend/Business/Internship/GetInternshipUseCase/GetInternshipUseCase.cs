using MediatR;

namespace backend.Service.Internship.GetInternshipUseCase;

public class GetInternshipUseCase: IRequestHandler<GetInternshipQuery, string>
{
    public Task<string> Handle(GetInternshipQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}