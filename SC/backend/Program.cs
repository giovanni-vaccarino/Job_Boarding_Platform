using System.Reflection;
using backend.Business;
using backend.Business.Auth.LoginUseCase;
using backend.Service;
using backend.Service.Contracts.Auth;
using backend.Shared;
using backend.Shared.Security;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// Add logging configuration
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins("http://localhost:5173") 
            .AllowAnyHeader()                    
            .AllowAnyMethod();                  
    });
});

var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Mediator
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

// Add DbContext
ConnectionStrings? connectionString = configuration.GetSection("ConnectionStrings").Get<ConnectionStrings>();
if (connectionString is null || string.IsNullOrEmpty(connectionString.DefaultConnection) || string.IsNullOrEmpty(connectionString.DatabaseVersion))
{
    throw new InvalidOperationException("Invalid database connection configuration.");
}
builder.Services.AddMappers(); 
builder.Services.AddDbContexts(connectionString); 

// JWT
JwtConfig? jwtConfig = configuration.GetSection("Jwt").Get<JwtConfig>();
if (jwtConfig is null || string.IsNullOrEmpty(jwtConfig.Key))
{
    throw new InvalidOperationException("Invalid JWT configuration.");
}
builder.Services.AddAppAuthentication(jwtConfig);

var app = builder.Build();
// Enable CORS middleware
app.UseCors("AllowSpecificOrigins");

app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Commented just for development purposes
// app.UseHttpsRedirection();

// Map controllers to route requests to them
app.MapControllers();

app.Run();