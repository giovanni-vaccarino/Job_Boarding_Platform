using backend.Service.Contracts.Auth;
using backend.Service.Contracts.Company;
using MediatR;

namespace backend.Business.Company.UpdateCompanyProfileUseCase;

public record UpdateCompanyProfileCommand(int Id, UpdateCompanyProfileDto Dto) : IRequest<CompanyDto>;