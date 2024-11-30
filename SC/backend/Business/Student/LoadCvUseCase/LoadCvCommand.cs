using backend.Service.Contracts.Student;
using MediatR;

namespace backend.Business.Student.LoadCvUseCase;

public record LoadCvCommand(LoadCvFileDto Dto, int StudentId) : IRequest<string>;