using AwsCognito.Test.Application.Abstractions.Services;
using AwsCognito.Test.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AwsCognito.Test.Application
{
    public static class ApplicationModule
    {
        public static IServiceCollection AddApplicationModule(this IServiceCollection services)
        {
            services.AddScoped<ISampleService, SampleService>();
            return services;
        }
    }
}
