using backend.Data;
using backend.Shared.StorageService;
using MediatR;

namespace backend.Business.Internship.GetInternshipUseCase;

public class GetInternshipUseCase: IRequestHandler<GetInternshipQuery, string>
{
    private readonly AppDbContext _dbContext;
    private readonly S3Manager _s3Manager;

    public GetInternshipUseCase(AppDbContext dbContext, S3Manager s3Manager)
    {
        _dbContext = dbContext;
        _s3Manager = s3Manager;
    }
    
    public async Task<string> Handle(GetInternshipQuery request, CancellationToken cancellationToken)
    {
        // if (await _dbContext.Database.CanConnectAsync(cancellationToken))
        // {
        //     return await Task.FromResult<string>("Nice");
        // }
        // else
        // {
        //     return await Task.FromResult<string>("Not nice");
        // }
        
        var res = await _s3Manager.TestConnectionAsync();
        
        return "Nice";
    }
}