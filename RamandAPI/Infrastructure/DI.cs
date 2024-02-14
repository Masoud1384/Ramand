using Application.EmployeesOperation.EmployeeRepositories;
using Application.EmployeesOperation.IEmployeeRepositories;
using Application.UserOperations.IRepositoryApplication;
using Application.UserOperations.RepositoryApplication;
using Domain.IRepositories;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using RabbitDI.MessagesOperations;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;


namespace Infrastructure
{
    public class DI
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserRepositoryApplication, UserRepositoryApplication>();
            services.AddScoped<ITokenRepository, TokenRepository>();
            services.AddScoped<IMessagesRepository, MessagesRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IEmployeeRepositoriesApplication, EmployeeRepositoriesApplication>();
        }
    }
}
