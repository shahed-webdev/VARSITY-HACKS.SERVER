using Microsoft.Extensions.DependencyInjection;
using VARSITY_HACKS.Repository;
using VARSITY_HACKS.BusinessLogic.Registration;

namespace VARSITY_HACKS.BusinessLogic
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
        {
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IRegistrationCore, RegistrationCore>();
            services.AddTransient<IEventCore, EventCore>();


            return services;
        }
    }
}