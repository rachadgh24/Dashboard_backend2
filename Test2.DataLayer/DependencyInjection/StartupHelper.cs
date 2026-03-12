using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Test2.DataLayer.DbContexts;
using Test2.DataLayer.Interfaces;
using Test2.DataLayer.Repositories;
namespace Test2.DataLayer.DependencyInjection
{
    public static class StartupHelper
    {
        public static void AddDotNetTrainingCoreContext(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<DotNetTrainingCoreContext>(opt =>
                    opt.UseInMemoryDatabase("Task1Db"));
                return;
            }

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<DotNetTrainingCoreContext>(opt =>
            {
                opt.UseNpgsql(connectionString, options =>
                {
                    options.EnableRetryOnFailure(
                        maxRetryCount: 3,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorCodesToAdd: null);
                    options.CommandTimeout(30);
                    options.MaxBatchSize(100);
                    options.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                })
                .LogTo(Console.WriteLine, LogLevel.Information);
            });
        }

        public static void AddDataLayerRepositories(this IServiceCollection services)
        {
            //services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPostRepository, PostRepository>();
        }
    }
}