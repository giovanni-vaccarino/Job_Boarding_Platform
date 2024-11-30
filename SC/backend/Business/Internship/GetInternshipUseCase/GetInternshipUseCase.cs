using backend.Data;
using backend.Shared.StorageService;
using MediatR;

namespace backend.Business.Internship.GetInternshipUseCase;

public class GetInternshipUseCase: IRequestHandler<GetInternshipQuery, string>
{
    private readonly AppDbContext _dbContext;
    private readonly IS3Manager _s3Manager;

    public GetInternshipUseCase(AppDbContext dbContext, IS3Manager s3Manager)
    {
        _dbContext = dbContext;
        _s3Manager = s3Manager;
    }
    
    public async Task<string> Handle(GetInternshipQuery request, CancellationToken cancellationToken)
    { 
        var res = await _s3Manager.TestConnectionAsync();
        
        return "Nice";
    }
}