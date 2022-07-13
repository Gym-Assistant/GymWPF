using Gym.UseCases.Common.Behaviors;
using Gym.UseCases.Common.DependencyInjection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Gym.UseCases.Common.Infrastructure
{
    public static class CommonModule
    {
        /// <summary>
        /// Register dependencies.
        /// </summary>
        /// <param name="services">Services.</param>
        public static void Register(IServiceCollection services)
        {
            services.AddTransient(typeof(Lazy<>), typeof(LazyProvider<>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CancellationHandlingBehavior<,>));
        }
    }
}
