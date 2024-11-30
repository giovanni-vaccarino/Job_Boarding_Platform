using backend.Service.Contracts.Student;
using MediatR;

namespace backend.Business.Student.UpdateStudentUseCase;

public record UpdateStudentCommand(int StudentId, UpdateStudentDto Dto): IRequest<StudentDto>;