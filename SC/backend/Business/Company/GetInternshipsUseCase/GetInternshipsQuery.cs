using backend.Service.Contracts.Internship;
using MediatR;

namespace backend.Business.Company.GetInternshipsUseCase;

public record GetInternshipsQuery(int Id, int? InternshipId = null) : IRequest<List<InternshipDto>>;