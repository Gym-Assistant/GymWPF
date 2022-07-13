using System.Reactive;
using System.Reactive.Subjects;
using Gym.MVVM.ServiceAbstractions.Navigation;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace Gym.MVVM.ViewModels;

/// <summary>
/// Base class for view models.
/// </summary>
public abstract class BaseViewModel : ObservableObject, IDisposable
{
    /// <summary>
    /// Is a view model busy.
    /// </summary>
    public bool IsBusy { get; set; }

    /// <summary>
    /// Loading data.
    /// </summary>
    public virtual Task LoadAsync()
    {
        return Task.CompletedTask;
    }

#pragma warning disable CA2213 // Disposable fields should be disposed, will be unusable if we dispose it on dispose
    private readonly ReplaySubject<Unit> unloadSubject = new(1);
#pragma warning restore CA2213 // Disposable fields should be disposed
    private bool disposedValue;

    /// <summary>
    /// Tells when view model is unloaded.
    /// </summary>
    public IObservable<Unit> Unload => unloadSubject;

    /// <summary>
    /// Returns a value indicating whether the page can go back or not.
    /// TODO: Integrate the logic to `EditableViewModel`.
    /// </summary>
    /// <remarks>
    /// Do not call methods from <see cref="IDialogService"/> or <see cref="INavigationService"/> in this method.
    /// </remarks>
    public virtual bool CanGoBack() => true;

    /// <inheritdoc />
    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                unloadSubject.OnNext(default);
                unloadSubject.OnCompleted();
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            disposedValue = true;
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}