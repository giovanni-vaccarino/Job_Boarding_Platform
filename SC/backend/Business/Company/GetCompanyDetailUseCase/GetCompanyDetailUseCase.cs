using backend.Data;
using AutoMapper;
using backend.Service.Contracts.Company;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backend.Business.Company.GetCompanyDetailUseCase;

public class GetCompanyDetailUseCase : IRequestHandler<GetCompanyDetailQuery, CompanyDto>
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetCompanyDetailUseCase(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<CompanyDto> Handle(GetCompanyDetailQuery request, CancellationToken cancellationToken)
    {
        var company = await _dbContext.Companies
            .Where(c => c.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new KeyNotFoundException("Company not found.");
        
        return _mapper.Map<CompanyDto>(company);
    }
}