using MediatR;

namespace backend.Business.Match.AcceptMatchUseCase;

public record AcceptMatchCommand(int MatchId) : IRequest<Unit>;