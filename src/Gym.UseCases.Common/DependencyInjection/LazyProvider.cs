using Microsoft.Extensions.DependencyInjection;

namespace Gym.UseCases.Common.DependencyInjection
{
    /// <summary>
    /// Implementation of <see cref="Lazy{T}"/> to be used in DI.
    /// </summary>
    /// <typeparam name="T">Type of service.</typeparam>
    internal class LazyProvider<T> : Lazy<T> where T : class
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public LazyProvider(IServiceProvider provider)
            : base(() => provider.GetRequiredService<T>())
        {
        }
    }
}
