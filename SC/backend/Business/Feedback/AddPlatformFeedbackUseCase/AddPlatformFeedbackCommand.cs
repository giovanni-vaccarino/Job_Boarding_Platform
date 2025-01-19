using backend.Service.Contracts.Feedback;
using MediatR;

namespace backend.Business.Feedback.AddPlatformFeedbackUseCase;

public record AddPlatformFeedbackCommand(AddPlatformFeedbackDto Dto) : IRequest<Unit>;