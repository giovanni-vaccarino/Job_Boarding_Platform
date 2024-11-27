using backend.Service.Contracts.Company;
using MediatR;

namespace backend.Business.Company.GetJobsCompany;

public record GetJobsCompanyQuery() : IRequest<List<ReturnJobDto>>;