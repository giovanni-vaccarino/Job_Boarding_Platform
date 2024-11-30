using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using backend.Data;
using Microsoft.EntityFrameworkCore;

namespace backend.Service.Middlewares.Policies.StudentOrCompany;


/// <summary>
/// Handles authorization for endpoints that require access to a specific student's data
/// or authorization for a company to access that data.
/// </summary>
/// <remarks>
/// This handler ensures that the user making the request has the appropriate access to
/// the specified student. The logic is as follows:
/// - If only a `studentId` is provided, the user must be the owner of that student's data.
/// - If a `companyId` is provided as a query parameter, it checks whether the company
///   is authorized to access the student's data based on business rules (e.g., the
///   student applied for a job at that company).
/// </remarks>
public class StudentOrCompanyAccessHandler : AuthorizationHandler<StudentOrCompanyAccessRequirement>
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<StudentOrCompanyAccessHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="StudentOrCompanyAccessHandler"/> class.
    /// </summary>
    /// <param name="dbContext">The database context used to validate access.</param>
    /// <param name="logger">The logger used for critical logging and debugging.</param>
    public StudentOrCompanyAccessHandler(AppDbContext dbContext, ILogger<StudentOrCompanyAccessHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// Handles the authorization requirement for student or company-specific access.
    /// </summary>
    /// <param name="context">The authorization context containing the user and resource information.</param>
    /// <param name="requirement">The requirement that must be fulfilled for authorization to succeed.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <remarks>
    /// This method extracts the `studentId` from the route values of the current HTTP request
    /// and optionally the `companyId` from the query string. The access logic is as follows:
    /// - If `companyId` is absent:
    ///   - It verifies if the user (from the JWT token) is the owner of the student data.
    /// - If `companyId` is present:
    ///   - It verifies if the company is authorized to access the student’s data.
    /// 
    /// If the conditions are met, the requirement is succeeded; otherwise, it fails.
    /// </remarks>
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        StudentOrCompanyAccessRequirement requirement)
    {
        var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                     ?? throw new InvalidOperationException("User ID not found.");

        if (context.Resource is HttpContext httpContext)
        {
            var studentId = httpContext.Request.RouteValues["studentId"]?.ToString();
            var companyId = httpContext.Request.Query["companyId"].FirstOrDefault();

            if (string.IsNullOrEmpty(studentId) || !int.TryParse(studentId, out var studentIdInt))
            {
                _logger.LogWarning("Student ID is missing or invalid in the route.");
                context.Fail();
                return;
            }
            
            if (string.IsNullOrEmpty(companyId))
            {
                var isStudentOwner = await _dbContext.Students
                    .AnyAsync(s => s.Id == studentIdInt && s.UserId.ToString() == userId);

                if (isStudentOwner)
                {
                    context.Succeed(requirement);
                    return;
                }
            }
            else
            {
                if (!int.TryParse(companyId, out var companyIdInt))
                {
                    _logger.LogWarning("Company ID query param not valid.");
                    context.Fail();
                    return;
                }
                _logger.LogCritical($"Company ID: {companyIdInt}.");
                // var isAuthorizedCompany = await _dbContext.Applications
                //     .Include(a => a.Job)
                //     .AnyAsync(a => a.StudentId == studentGuid && a.Job.CompanyId == companyGuid);
                
                var isAuthorizedCompany = true; // Temporary solution, until the database is set up for applications
                
                if (isAuthorizedCompany)
                {
                    context.Succeed(requirement);
                    return;
                }
            }
        }

        _logger.LogWarning("Authorization failed for userId: {userId}", userId);
        context.Fail();
    }
}
