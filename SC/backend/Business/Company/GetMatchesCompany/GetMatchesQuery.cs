using backend.Service.Contracts.Company;
using MediatR;

namespace backend.Business.Company.GetMatchesCompany;

public record GetMatchesQuery(int Id): IRequest<List<SingleMatchesDetails>>;