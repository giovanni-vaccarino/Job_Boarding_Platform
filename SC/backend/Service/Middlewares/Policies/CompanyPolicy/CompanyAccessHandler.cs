using System.Security.Claims;
using backend.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace backend.Service.Middlewares;

/// <summary>
/// Handles authorization for endpoints that require access to specific companies.
/// </summary>
/// <remarks>
/// This handler ensures that the user making the request has access to the specified company.
/// It verifies that the company ID in the route matches the user ID in the token.
/// </remarks>
public class CompanyAccessHandler : AuthorizationHandler<CompanyAccessRequirement>
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<CompanyAccessHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="CompanyAccessHandler"/> class.
    /// </summary>
    /// <param name="dbContext">The database context used to validate company ownership.</param>
    /// <param name="logger">The logger used for critical logging.</param>
    public CompanyAccessHandler(AppDbContext dbContext, ILogger<CompanyAccessHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// Handles the authorization requirement for company-specific access.
    /// </summary>
    /// <param name="context">The authorization context containing the user and resource information.</param>
    /// <param name="requirement">The requirement that must be fulfilled for authorization to succeed.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <remarks>
    /// This method extracts the company ID from the route values of the current HTTP request
    /// and checks if the user ID from the JWT token matches the owner of the specified company in the database.
    /// If the conditions are met, the requirement is succeeded; otherwise, it fails.
    /// </remarks>
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        CompanyAccessRequirement requirement)
    {
        var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new InvalidOperationException("User ID not found.");

        if (context.Resource is HttpContext httpContext)
        {
            var companyId = httpContext.Request.RouteValues["id"]?.ToString();
            _logger.LogCritical($"UserId: {userId}");
            _logger.LogCritical($"CompanyId: {companyId}");

            if (!string.IsNullOrEmpty(companyId) &&
                await _dbContext.Companies.AnyAsync(c => c.Id.ToString() == companyId && c.UserId.ToString() == userId))
            {
                context.Succeed(requirement);
                return;
            }
        }

        context.Fail();
    }
}