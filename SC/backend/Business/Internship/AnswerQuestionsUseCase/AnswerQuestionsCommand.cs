using backend.Service.Contracts.Internship;
using MediatR;

namespace backend.Business.Internship.AnswerQuestionsUseCase;

public record AnswerQuestionsCommand(int ApplicationId, AnswerQuestionsDto Dto) : IRequest<ApplicationDto>;