using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using backend.Data;
using Microsoft.EntityFrameworkCore;

namespace backend.Service.Middlewares.Policies.StudentPolicy;

/// <summary>
/// Handles authorization for endpoints that require access to specific students.
/// </summary>
/// <remarks>
/// This handler ensures that the user making the request has access to the specified student.
/// It verifies that the student ID in the route matches the user ID in the token.
/// </remarks>
public class StudentAccessHandler : AuthorizationHandler<StudentAccessRequirement>
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<StudentAccessHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="StudentAccessHandler"/> class.
    /// </summary>
    /// <param name="dbContext">The database context used to validate student ownership.</param>
    /// <param name="logger">The logger used for critical logging.</param>
    public StudentAccessHandler(AppDbContext dbContext, ILogger<StudentAccessHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// Handles the authorization requirement for student-specific access.
    /// </summary>
    /// <param name="context">The authorization context containing the user and resource information.</param>
    /// <param name="requirement">The requirement that must be fulfilled for authorization to succeed.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <remarks>
    /// This method extracts the student ID from the route values of the current HTTP request
    /// and checks if the user ID from the JWT token matches the owner of the specified student in the database.
    /// If the conditions are met, the requirement is succeeded; otherwise, it fails.
    /// </remarks>
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        StudentAccessRequirement requirement)
    {
        var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new InvalidOperationException("User ID not found.");

        if (context.Resource is HttpContext httpContext)
        {
            var studentId = httpContext.Request.RouteValues["studentId"]?.ToString();
            if (string.IsNullOrEmpty(studentId))
            {
                studentId = httpContext.Request.Query["studentId"].FirstOrDefault();
            }
            _logger.LogCritical(userId);
            _logger.LogCritical(studentId);
            if (!string.IsNullOrEmpty(studentId) &&
                await _dbContext.Students.AnyAsync(s => s.Id.ToString() == studentId && s.UserId.ToString() == userId))
            {
                context.Succeed(requirement);
                return;
            }
        }

        context.Fail();
    }
}
