using backend.Service.Contracts.Company;
using MediatR;

namespace backend.Business.Company.GetMatchingApplicant;

public record GetMatchingApplicantCommand() : IRequest<ReturnPersonDetailsMatchingDto>;