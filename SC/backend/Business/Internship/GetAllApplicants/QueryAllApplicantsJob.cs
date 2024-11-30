using backend.Service.Contracts.Company;
using MediatR;

namespace backend.Business.Internship.GetAllApplicants;

public record QueryAllApplicantsJob(int id) : IRequest<List<SingleApplicantToInternshipDto>>;