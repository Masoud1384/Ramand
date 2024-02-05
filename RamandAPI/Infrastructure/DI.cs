using Application.UserOperations.IRepositoryApplication;
using Application.UserOperations.RepositoryApplication;
using Domain.IRepositories;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using RabbitDI.RabbitMqOperation;


namespace Infrastructure
{
    public class DI
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserRepositoryApplication, UserRepositoryApplication>();
            services.AddScoped<ITokenRepository, TokenRepository>();
            services.AddScoped<IRabbitmqRepository, RabbitMqRepository>();
        }
    }
}
