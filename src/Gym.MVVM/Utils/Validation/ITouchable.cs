namespace Gym.MVVM.Utils.Validation;

/// <summary>
/// Allows mark an object as "touched" (as if user attempted to make any changes in the control.)
/// </summary>
public interface ITouchable
{
    /// <summary>
    /// Emulate a touch on the object as if user attempted to modify it.
    /// </summary>
    /// <param name="revalidate">Indicates if touching the model should also trigger revalidation.</param>
    void Touch(bool revalidate);
}