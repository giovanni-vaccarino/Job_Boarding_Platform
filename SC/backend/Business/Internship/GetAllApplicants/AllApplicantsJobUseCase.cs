using AutoMapper;
using backend.Service.Contracts.Company;
using MediatR;

namespace backend.Business.Internship.GetAllApplicants;

public class AllApplicantsJobUseCase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    
    public AllApplicantsJobUseCase(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }
    
    public async Task<List<SingleApplicantToInternshipDto>> Handle(QueryAllApplicantsJob request, CancellationToken cancellationToken)
    {
        var jobId = request.;
        
        var applicants = await _mediator.Send(new GetApplicantsQuery { JobId = jobId }, cancellationToken);
        
        var applicantsToEvaluate = applicants.Select(a => new ApplicantDetailsToEvaluateDto
        {
            applicantDetails = _mapper.Map<ApplicantDetailsDto>(a),
            questions = _mapper.Map<List<QuestionDto>>(a.Questions)
        }).ToList();
        
        return applicantsToEvaluate;
    }
}