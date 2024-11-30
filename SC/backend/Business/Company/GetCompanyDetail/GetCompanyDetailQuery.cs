using backend.Service.Contracts.Company;
using MediatR;

namespace backend.Business.Company.GetCompanyDetail;


public record GetCompanyDetailQuery(int Id) : IRequest<CompanyDto>;