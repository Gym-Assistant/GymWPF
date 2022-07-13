using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace Gym.Infrastructure.Common.Reactive;

/// <summary>
/// Contains extension methods for Observable operators.
/// </summary>
public static class ReactiveExtensions
{
    /// <summary>
    /// Limits the emissions from source Observable, making it to emit not more often than <paramref name="emitPeriod"/>.
    /// http://reactivex.io/documentation/operators/debounce.html .
    /// </summary>
    /// <typeparam name="T">Type of the data emitted.</typeparam>
    /// <param name="source">Source observable.</param>
    /// <param name="emitPeriod">How frequently should the data be emitted.</param>
    /// <returns>Resulting observable.</returns>
    public static IObservable<T> Debounce<T>(this IObservable<T> source, TimeSpan emitPeriod)
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }
        if (emitPeriod < TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(emitPeriod));
        }

        return Observable.Create<T>(observer =>
        {
            bool hasLastValue = false;
            T lastValue = default;

            var disposable = new CompositeDisposable(2);
            disposable.Add(Scheduler.Default.SchedulePeriodic(emitPeriod, () =>
            {
                if (hasLastValue)
                {
                    observer.OnNext(lastValue);
                    hasLastValue = false;
                }
            }));

            disposable.Add(source.Subscribe(value =>
            {
                hasLastValue = true;
                lastValue = value;
            }, observer.OnError, observer.OnCompleted));

            return disposable;
        });
    }

    /// <summary>
    /// Limits the emissions from source Observable, making it to emit not more often than <paramref name="emitPeriod"/>.
    /// Unlike <see cref="Debounce{T}(IObservable{T}, TimeSpan)"/>, this method just ignores the emissions instead of remembering the emission that was available last.
    /// http://reactivex.io/documentation/operators/debounce.html
    /// </summary>
    /// <typeparam name="T">Type of the data emitted.</typeparam>
    /// <param name="source">Source observable.</param>
    /// <param name="sleepSelector">Function to calculate the time of sleep until the next emission is available.</param>
    /// <returns>Resulting observable.</returns>
    public static IObservable<T> LimitTime<T>(this IObservable<T> source, Func<T, TimeSpan> sleepSelector)
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }
        if (sleepSelector == null)
        {
            throw new ArgumentNullException(nameof(sleepSelector));
        }

        return Observable.Create<T>(observer =>
        {
            var nextEmitTime = default(DateTime?);

            return source.Subscribe(value =>
            {
                var currentTime = DateTime.UtcNow;
                if (nextEmitTime == null || currentTime >= nextEmitTime)
                {
                    var sleepTime = sleepSelector(value);
                    nextEmitTime = currentTime + sleepTime;
                    observer.OnNext(value);
                }
            }, observer.OnError, observer.OnCompleted);
        });
    }

    private static readonly TimeSpan uiThrottlePeriod = TimeSpan.FromMilliseconds(10);

    /// <summary>
    /// Used to optimize too frequent emissions from observable when receiving data from UI changes.
    /// </summary>
    public static IObservable<T> ThrottleUI<T>(this IObservable<T> source)
    {
        return source.Throttle(uiThrottlePeriod);
    }
}
