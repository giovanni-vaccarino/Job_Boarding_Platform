using backend.Service.Contracts.Auth;
using backend.Service.Contracts.Company;
using MediatR;

namespace backend.Business.Company.UpdateCompanyProfile;

public record UpdateCompanyProfileCommand(UpdateCompanyProfileDto Dto) : IRequest<string>;