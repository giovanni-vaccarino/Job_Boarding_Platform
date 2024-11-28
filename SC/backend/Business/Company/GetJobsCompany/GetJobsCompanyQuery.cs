using backend.Service.Contracts.Company;
using MediatR;

namespace backend.Business.Company.GetJobsCompany;

public record GetJobsCompanyQuery(string Id) : IRequest<CompanyJobsDto>;