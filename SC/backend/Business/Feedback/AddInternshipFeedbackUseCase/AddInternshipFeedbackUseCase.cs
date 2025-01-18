using backend.Data;
using backend.Data.Entities;
using backend.Shared.Enums;
using MediatR;

namespace backend.Business.Feedback.AddInternshipFeedbackUseCase;

public class AddInternshipFeedbackUseCase : IRequestHandler<AddInternshipFeedbackCommand, Unit>
{
   private readonly AppDbContext _dbContext;
   
   public AddInternshipFeedbackUseCase(AppDbContext dbContext)
   {
       _dbContext = dbContext;
   }

   public async Task<Unit> Handle(AddInternshipFeedbackCommand request, CancellationToken cancellationToken)
   {
       var dto = request.Dto;
       var applicationId = dto.ApplicationId;
       var application = _dbContext.Applications.FirstOrDefault(a => a.Id == applicationId);
       
       if (application == null)
       {
           throw new InvalidOperationException("Application not found.");
       }
       
       // Validate that the company id or the student id are involved in that application
       if(dto.Actor == ProfileType.Company)
       {
           var companyId = _dbContext.Internships
               .Where(i => i.Id == application.InternshipId)
               .Select(i => i.CompanyId)
               .FirstOrDefault();
           
              if(companyId != dto.ProfileId)
              {
                  throw new InvalidOperationException("Company is not involved in this application.");
              }
       }
       else
       {
           if (dto.ProfileId != application.StudentId)
           {
               throw new InvalidOperationException("Student is not involved in this application.");
           }
       }
       
       var feedback = new InternshipFeedback
       {
           Text = dto.Text,
           Rating = dto.Rating,
           Actor = dto.Actor,
           ApplicationId = applicationId,
           CreatedAt = DateTime.UtcNow,
           UpdatedAt = DateTime.UtcNow
       };
       
       _dbContext.InternshipFeedbacks.Add(feedback);
       await _dbContext.SaveChangesAsync(cancellationToken);
       
       return Unit.Value;
   }
}