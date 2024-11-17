using MediatR;

namespace backend.Services.Internship.UseCases.AddInternshipUseCase;

public class AddInternshipUseCase : IRequestHandler<AddInternshipCommand, int>
{
    public Task<int> Handle(AddInternshipCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}