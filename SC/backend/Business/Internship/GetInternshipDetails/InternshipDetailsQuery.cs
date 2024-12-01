using backend.Service.Contracts.Company;
using MediatR;

namespace backend.Business.Internship.GetInternshipDetails;

public record InternshipDetailsQuery(int Id): IRequest<JobDetailsDto>;