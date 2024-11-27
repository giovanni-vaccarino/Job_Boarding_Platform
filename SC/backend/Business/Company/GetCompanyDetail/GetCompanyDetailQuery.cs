using backend.Service.Contracts.Company;
using MediatR;

namespace backend.Business.Company.GetCompanyDetail;


public record GetCompanyDetailQuery(string Id) : IRequest<UpdateCompanyProfileDto>;