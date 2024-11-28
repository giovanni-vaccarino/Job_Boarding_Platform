using backend.Service.Contracts.Student;
using MediatR;

namespace backend.Business.Student.UpdateStudentUseCase;

public record UpdateStudentCommand(StudentDto Dto): IRequest<StudentDto>;