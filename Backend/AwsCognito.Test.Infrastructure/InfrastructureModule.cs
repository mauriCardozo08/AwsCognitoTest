using AwsCognito.Test.Application.Abstractions.Repositories;
using AwsCognito.Test.Infrastructure.Database.App;
using AwsCognito.Test.Infrastructure.Database.App.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AwsCognito.Test.Infrastructure
{
    public static class InfrastructureModule
    {
        public static IServiceCollection AddInfrastructureModule(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Default");

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString)
            );

            services.AddScoped<ISampleRepository, SampleRepository>();

            return services;
        }
    }
}
