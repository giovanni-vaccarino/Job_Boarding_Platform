using backend.Data;
using backend.Shared;
using Microsoft.EntityFrameworkCore;

namespace backend.Service;

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
            // var connectionString = "server=localhost;user=root;password=1234;database=ef";
            var serverVersion = new MySqlServerVersion(new Version(connectionString.DatabaseVersion));
            opt.UseMySql(connectionString.DefaultConnection, serverVersion);
        });
    }
}