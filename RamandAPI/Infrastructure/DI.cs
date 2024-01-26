using Application.UserOperations.IRepositoryApplication;
using Application.UserOperations.RepositoryApplication;
using Domain.IRepositories;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public class DI
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserRepositoryApplication, UserRepositoryApplication>();
        }
    }
}
