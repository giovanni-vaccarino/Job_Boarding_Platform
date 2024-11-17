using System.Reflection;
using System.Text;
using backend.Data;
using backend.Service;
using backend.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Mediator
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

// Add DbContext
ConnectionStrings connectionString = configuration.GetSection("ConnectionStrings").Get<ConnectionStrings>();
builder.Services.AddMappers(); 
builder.Services.AddDbContexts(connectionString); 

// JWT
builder.Services.AddAppAuthentication(configuration.GetSection("Jwt").Get<JwtConfig>());

builder.Services.AddAuthorization();
builder.Services.AddAuthentication();

var app = builder.Build();

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