using MediatR;

namespace backend.Business.Match.AddMatchesUseCase;

public record AddMatchesCommand(List<int> InternshipIds, List<int> StudentIds) : IRequest<Unit>;