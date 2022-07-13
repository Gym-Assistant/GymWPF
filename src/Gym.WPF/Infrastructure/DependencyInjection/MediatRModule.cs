using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Gym.UseCases.User.LoginUser;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Gym.WPF.Infrastructure.DependencyInjection;

/// <summary>
/// Register Mediator as dependency.
/// </summary>
internal static class MediatRModule
{
    /// <summary>
    /// Register dependencies.
    /// </summary>
    /// <param name="services">Services.</param>
    public static void Register(IServiceCollection services)
    {
        services.AddMediatR(
            mediatrConfig =>
            {
                mediatrConfig.AsScoped();
                mediatrConfig.Using<MediatorScoped>();
            },
            typeof(LoginUserCommand));

        // Needed for MediatorScoped
        services.AddScoped<Mediator>();
    }

    /// <summary>
    /// Mediator implementation that creates a scope for every new request / message sent.
    /// </summary>
    internal class MediatorScoped : IMediator
    {
        private readonly IServiceScopeFactory serviceScopeFactory;

        /// <summary>
        /// Constructor.
        /// </summary>
        public MediatorScoped(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }

        /// <inheritdoc/>
        public async Task Publish(object notification, CancellationToken cancellationToken = default)
        {
            using var serviceScope = serviceScopeFactory.CreateScope();
            var mediator = serviceScope.ServiceProvider.GetRequiredService<Mediator>();
            await mediator.Publish(notification, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification
        {
            using var serviceScope = serviceScopeFactory.CreateScope();
            var mediator = serviceScope.ServiceProvider.GetRequiredService<Mediator>();
            await mediator.Publish(notification, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            using var serviceScope = serviceScopeFactory.CreateScope();
            var mediator = serviceScope.ServiceProvider.GetRequiredService<Mediator>();
            return await mediator.Send(request, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<object> Send(object request, CancellationToken cancellationToken = default)
        {
            using var serviceScope = serviceScopeFactory.CreateScope();
            var mediator = serviceScope.ServiceProvider.GetRequiredService<Mediator>();
            return await mediator.Send(request, cancellationToken);
        }

        public IAsyncEnumerable<TResponse> CreateStream<TResponse>(IStreamRequest<TResponse> request,
            CancellationToken cancellationToken = new CancellationToken()) =>
            throw new System.NotImplementedException();

        public IAsyncEnumerable<object?> CreateStream(object request, CancellationToken cancellationToken = new CancellationToken())
            => throw new System.NotImplementedException();
    }
}
