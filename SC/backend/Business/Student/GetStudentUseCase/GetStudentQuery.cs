using backend.Service.Contracts.Student;
using MediatR;

namespace backend.Business.Student.GetStudentUseCase;

public record GetStudentQuery(int Id): IRequest<StudentDto>;