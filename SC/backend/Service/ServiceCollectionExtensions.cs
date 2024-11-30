using System.Text;
using backend.Service.Middlewares;
using backend.Shared;
using backend.Shared.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace backend.Service;

public static class ServiceCollectionExtensions
{
    public static void AddAppAuthentication(this IServiceCollection services, JwtConfig config)
    {
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(config.Key)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
            });
        
        services.AddAuthorization(options =>
        {
            options.AddPolicy("StudentAccessPolicy", policy =>
                policy.Requirements.Add(new StudentAccessRequirement()));
            options.AddPolicy("CompanyAccessPolicy", policy =>
                policy.Requirements.Add(new CompanyAccessRequirement()));
        });

        services.AddScoped<SecurityContext>();
    }
}