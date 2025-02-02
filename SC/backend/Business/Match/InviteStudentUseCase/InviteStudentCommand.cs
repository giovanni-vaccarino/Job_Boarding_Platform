using MediatR;

namespace backend.Business.Match.InviteStudentUseCase;

public record InviteStudentCommand(int MatchId) : IRequest<Unit>;