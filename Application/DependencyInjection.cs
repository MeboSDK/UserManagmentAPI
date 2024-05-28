using Microsoft.Extensions.DependencyInjection;
using Application.Interfaces;
using Application.Services;
using FluentValidation;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = typeof(DependencyInjection).Assembly;

            services.AddScoped<IUserService,UserService>();

            services.AddValidatorsFromAssembly(assembly);

            return services;
        }
    }
}
