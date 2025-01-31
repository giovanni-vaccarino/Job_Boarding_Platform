using backend.Data;
using backend.Shared.EmailService;
using backend.Shared.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backend.Business.Auth.SendVerificationMailUseCase;

public class SendVerificationMailUseCase : IRequestHandler<SendVerificationMailCommand, Unit>
{
    private readonly AppDbContext _dbContext;
    private readonly SecurityContext _securityContext;
    private readonly IEmailService _emailService;
    private readonly ILogger<SendVerificationMailUseCase> _logger;

    public SendVerificationMailUseCase(AppDbContext dbContext, SecurityContext securityContext, IEmailService emailService, ILogger<SendVerificationMailUseCase> logger)
    {
        _dbContext = dbContext;
        _securityContext = securityContext;
        _emailService = emailService;
        _logger = logger;
    }
    
    public async Task<Unit> Handle(SendVerificationMailCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken)
            ?? throw new KeyNotFoundException("User not found");

        var email = request.Email;

        var verificationToken = _securityContext.CreateVerificationToken(user.Id.ToString());
        var verificationLink = $"http://localhost:5173/verify-email?token={verificationToken}";
        
        await _emailService.SendEmailAsync(email, "SC Email Verification", 
            $"Please verify your email by clicking this link: {verificationLink}");
        
        _logger.LogInformation("Verification email sent to {email}", email);
        
        return Unit.Value;
    }
}