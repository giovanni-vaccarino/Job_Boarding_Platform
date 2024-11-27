using backend.Data;
using AutoMapper;
using backend.Service.Contracts.Company;
using backend.Data.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using backend.Business;

namespace backend.Business.Company.GetCompanyDetail;

public class GetCompanyDetailUseCase : IRequestHandler<GetCompanyDetailQuery, UpdateCompanyProfileDto>
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetCompanyDetailUseCase(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<UpdateCompanyProfileDto> Handle(GetCompanyDetailQuery request, CancellationToken cancellationToken)
    {
        //TODO move the parse inside the Controller or create a dto ah hoc?
        var company = await _dbContext.Companies
            .Where(c => c.Id == int.Parse(request.Id))
            .FirstOrDefaultAsync(cancellationToken);
        

        if (company == null)
        {
            throw new KeyNotFoundException("Company not found.");
        }

        var companyDto = _mapper.Map<UpdateCompanyProfileDto>(company);
        return companyDto;
    }
}