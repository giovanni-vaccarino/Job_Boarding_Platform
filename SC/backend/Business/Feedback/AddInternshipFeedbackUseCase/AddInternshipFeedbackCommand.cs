using backend.Service.Contracts.Feedback;
using MediatR;

namespace backend.Business.Feedback.AddInternshipFeedbackUseCase;

public record AddInternshipFeedbackCommand(AddInternshipFeedbackDto Dto) : IRequest<Unit>;