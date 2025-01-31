using backend.Data;
using backend.Shared.EmailService;
using backend.Shared.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backend.Business.Auth.SendForgotPasswordUseCase;

public class SendForgotPasswordUseCase : IRequestHandler<SendForgotPasswordCommand, Unit>
{
    private readonly AppDbContext _dbContext;
    private readonly IEmailService _emailService;
    private readonly SecurityContext _securityContext;
    private readonly ILogger<SendForgotPasswordUseCase> _logger;

    public SendForgotPasswordUseCase(AppDbContext dbContext, IEmailService emailService, SecurityContext securityContext, ILogger<SendForgotPasswordUseCase> logger)
    {
        _dbContext = dbContext;
        _emailService = emailService;
        _securityContext = securityContext;
        _logger = logger;
    }

    public async Task<Unit> Handle(SendForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken)
            ?? throw new KeyNotFoundException("User not found.");;

        var resetToken = _securityContext.CreateVerificationToken(user.Id.ToString());

        var resetLink = $"https://localhost:5173/reset-password?token={resetToken}";
        await _emailService.SendEmailAsync(user.Email, "Password Reset", $"Reset your password using this link: {resetLink}");

        _logger.LogInformation("Password reset email sent to {Email}", user.Email);

        return Unit.Value;
    }
}