using backend.Service.Contracts.Match;
using backend.Shared.Enums;
using MediatR;

namespace backend.Business.Match.GetMatchesUseCase;

public record GetMatchesQuery(int ProfileId, ProfileType ProfileType) : IRequest<List<MatchDto>>;