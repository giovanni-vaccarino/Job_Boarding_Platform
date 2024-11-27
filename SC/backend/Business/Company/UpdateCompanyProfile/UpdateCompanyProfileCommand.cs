using backend.Service.Contracts.Auth;
using backend.Service.Contracts.Company;
using MediatR;

namespace backend.Business.Company.UpdateCompanyProfile;

public record UpdateProfileCommand(UpdateCompanyProfileDto Dto) : IRequest<string>;