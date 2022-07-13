namespace Gym.Infrastructure.Common.Configuration;

/// <summary>
/// Configuration for UI.
/// </summary>
public class UIConfiguration
{
    /// <summary>
    /// How often the UI should be updated if necessary to improve performance.
    /// </summary>
    public TimeSpan DesiredUiUpdateFrequency { get; init; } = TimeSpan.FromMilliseconds(50);

    /// <summary>
    /// Delay before starting the search when user manually enters text in an input.
    /// </summary>
    public int SearchDelayMilliseconds => 500;

    /// <inheritdoc cref="SearchDelayMilliseconds"/>
    public TimeSpan SearchDelay => TimeSpan.FromMilliseconds(SearchDelayMilliseconds);
}