using AutoMapper;
using backend.Data;
using backend.Service.Contracts.Internship;
using backend.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backend.Business.Internship.UpdateStatusApplicationUseCase;

public class UpdateStatusApplicationUseCase : IRequestHandler<UpdateStatusApplicationCommand, ApplicationDto>
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    
    public UpdateStatusApplicationUseCase(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    
    public async Task<ApplicationDto> Handle(UpdateStatusApplicationCommand request, CancellationToken cancellationToken)
    {
        var applicationId = request.ApplicationId;
        var updateStatusDto = request.Dto;
        
        var application = await _dbContext.Applications
            .Include(app => app.Internship)
            .FirstOrDefaultAsync(app => app.Id == applicationId, cancellationToken)
             ?? throw new KeyNotFoundException($"Application with ID {applicationId} not found.");

        if (application.ApplicationStatus != ApplicationStatus.Screening
            && application.ApplicationStatus != ApplicationStatus.LastEvaluation)
        {
            throw new ArgumentException("The application status is not valid for updating.");
        }
        
        application.ApplicationStatus = updateStatusDto.Status;

        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<ApplicationDto>(application);
    }
}