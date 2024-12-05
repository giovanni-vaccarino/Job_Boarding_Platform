using backend.Service.Contracts.Company;
using MediatR;

namespace backend.Business.Company.GetCompanyDetailUseCase;


public record GetCompanyDetailQuery(int Id) : IRequest<CompanyDto>;