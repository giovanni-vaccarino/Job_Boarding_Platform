using backend.Service.Contracts.Company;
using MediatR;

namespace backend.Business.Company.AddJobCompany;

public record AddJobCommand(AddJobCompanyDto Dto) : IRequest<string>;