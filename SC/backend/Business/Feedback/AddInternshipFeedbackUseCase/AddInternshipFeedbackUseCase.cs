using backend.Data;
using backend.Data.Entities;
using backend.Shared.Enums;
using MediatR;

namespace backend.Business.Feedback.AddInternshipFeedbackUseCase;

/// <summary>
/// Use case for adding internship feedback.
/// </summary>
public class AddInternshipFeedbackUseCase : IRequestHandler<AddInternshipFeedbackCommand, Unit>
{
   private readonly AppDbContext _dbContext;
   
   
   /// <summary>
   /// Initializes a new instance of the <see cref="AddInternshipFeedbackUseCase"/> class.
   /// </summary>
   /// <param name="dbContext">The application database context.</param>
   public AddInternshipFeedbackUseCase(AppDbContext dbContext)
   {
       _dbContext = dbContext;
   }

   /// <summary>
   /// Handles the command to add feedback for an internship application.
   /// </summary>
   /// <param name="request">The feedback command containing the feedback details.</param>
   /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
   /// <returns>A unit task representing the completion of the command.</returns>
   /// <exception cref="InvalidOperationException">
   /// Thrown if the application is not found or if the actor is not involved in the application.
   /// </exception>
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