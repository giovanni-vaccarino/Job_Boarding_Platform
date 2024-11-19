using backend.Data;
using backend.Shared;
using Microsoft.EntityFrameworkCore;

namespace backend.Business;

public static class BusinessCollectionExtensions
{
    public static void AddMappers(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(Program));
    }

    public static void AddDbContexts(this IServiceCollection services,ConnectionStrings connectionString)
    {
        services.AddDbContext<AppDbContext>(opt =>
        {
            var serverVersion = new MySqlServerVersion(new Version(connectionString.DatabaseVersion));
            opt.UseMySql(connectionString.DefaultConnection, serverVersion);
        });
    }
}