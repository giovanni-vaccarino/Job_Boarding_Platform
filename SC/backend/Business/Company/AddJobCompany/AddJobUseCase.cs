using backend.Data;
using backend.Service.Contracts.Company;
using MediatR;

namespace backend.Business.Company.AddJobCompany;

public class AddJobUseCase
{
    private readonly IMediator _mediator;
    private readonly AppDbContext _dbContext;

    public AddJobUseCase(AppDbContext dbContext, IMediator mediator)
    {
        _mediator = mediator;
        _dbContext = dbContext;
    }
    
    public async Task<string> Handle(AddJobCommand request, CancellationToken cancellationToken)
    {
        var newJob = request.Dto;
        
        //TODO create a job entity  and update the db
        
        return "Job added successfully.";
    }
}