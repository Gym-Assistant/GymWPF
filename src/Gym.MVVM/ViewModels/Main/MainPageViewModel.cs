using Gym.MVVM.ServiceAbstractions.Navigation;
using MediatR;

namespace Gym.MVVM.ViewModels.Main;

/// <summary>
/// Application main page. Open after authentication.
/// </summary>
public class MainPageViewModel : BaseViewModel
{
    private readonly INavigationService navigationService;
    private readonly IMediator mediator;

    /// <summary>
    /// SomeString TODO dell it.
    /// </summary>
    public string HelloWorld { get; set; }= "Hello world";

    /// <summary>
    /// Constructor.
    /// </summary>
    public MainPageViewModel(
        INavigationService navigationService,
        IMediator mediator)
    {
        this.navigationService = navigationService;
        this.mediator = mediator;

        OnPropertyChanged("HelloWorld");
    }

    /// <inheritdoc/>
    public override async Task LoadAsync()
    {
        await base.LoadAsync();
    }
}
