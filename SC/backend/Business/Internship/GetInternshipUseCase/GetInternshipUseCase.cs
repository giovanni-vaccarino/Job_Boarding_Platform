using backend.Data;
using MediatR;

namespace backend.Business.Internship.GetInternshipUseCase;

public class GetInternshipUseCase: IRequestHandler<GetInternshipQuery, string>
{
    private readonly AppDbContext _dbContext;

    public GetInternshipUseCase(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<string> Handle(GetInternshipQuery request, CancellationToken cancellationToken)
    {
        if (await _dbContext.Database.CanConnectAsync(cancellationToken))
        {
            return await Task.FromResult<string>("Nice");
        }
        else
        {
            return await Task.FromResult<string>("Not nice");
        }
    }
}