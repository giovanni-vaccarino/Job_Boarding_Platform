using backend.Service.Contracts.Company;
using MediatR;

namespace backend.Business.Internship.GetInternshipDetails;

public record InternshipDetailsCommand(int Id): IRequest<JobDetailsDto>;