using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Test2.Application.Interfaces;

using Test2.Application.Services;
using Test2.DataLayer.DependencyInjection;

namespace Test2.Application.DependencyInjection
{
    public static class StartupHelper
    {
        public static void AddApplicationLayerServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDotNetTrainingCoreContext(configuration);
            services.AddDataLayerRepositories();
            //services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPostService, PostService>();
        }
    }
}
