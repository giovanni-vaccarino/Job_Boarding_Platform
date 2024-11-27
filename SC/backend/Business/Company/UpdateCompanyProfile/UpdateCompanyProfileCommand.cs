using backend.Service.Contracts.Auth;
using MediatR;

namespace backend.Business.Company.UpdateCompanyProfile;

public record UpdateProfileCommand(UserRegisterDto Dto) : IRequest<TokenResponse>;