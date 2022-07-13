using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using Gym.MVVM.ServiceAbstractions;
using Gym.MVVM.ServiceAbstractions.Navigation;
using Microsoft.Extensions.Logging;
using Gym.MVVM.Extensions;
using Gym.MVVM.ViewModels.Users;
using Gym.WPF.Infrastructure.Startup;

namespace Gym.WPF;

/// <summary>
/// Compositional root.
/// </summary>
internal class CompositionRoot : IDisposable
{
    private ServiceProvider serviceProvider;

    /// <summary>
    /// Service provider.
    /// </summary>
    public IServiceProvider ServiceProvider => serviceProvider;

    /// <summary>
    /// Application configuration.
    /// </summary>
    public IConfiguration Configuration { get; private set; }

    private static CompositionRoot instance;
    private bool disposedValue;

    /// <summary>
    /// Get an instance of this class.
    /// </summary>
    /// <returns></returns>
    public static CompositionRoot GetInstance()
    {
        if (instance == null)
        {
            instance = new CompositionRoot();
            instance.Configure();
        }
        return instance;
    }

    /// <summary>
    /// Preparing DI.
    /// </summary>
    private void Configure()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        Configuration = builder.Build();

        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);

        serviceProvider = serviceCollection.BuildServiceProvider();
        ServiceProvider.PostRegisterMvvm();
    }

    /// <summary>
    /// Run the app and show the main page.
    /// </summary>
    public async Task RunAsync()
    {
        try
        {
            await InitializeDatabaseAsync();
            var uiContext = ServiceProvider.GetRequiredService<IUiContext>();
            await uiContext.SwitchToUi();

            var navigation = ServiceProvider.GetRequiredService<INavigationService>();
            navigation.Open<LoginViewModel>();
        }
        catch (Exception exception)
        {
            MessageBox.Show("Unable to start the application.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            var logger = ServiceProvider.GetRequiredService<ILogger<CompositionRoot>>();
            logger.LogCritical(exception, "Unexpected error occurred.");
        }
    }

    private void ConfigureServices(IServiceCollection services)
    {
        Infrastructure.DependencyInjection.DesktopModule.Register(services, Configuration);
        UseCases.Common.Infrastructure.CommonModule.Register(services);
    }

    private async Task InitializeDatabaseAsync()
    {
        var databaseInitializer = ServiceProvider.GetRequiredService<DatabaseInitializer>();
        await databaseInitializer.InitializeAsync();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                serviceProvider.Dispose();
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            disposedValue = true;
        }
    }

    // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    // ~CompositionRoot()
    // {
    //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
    //     Dispose(disposing: false);
    // }
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
