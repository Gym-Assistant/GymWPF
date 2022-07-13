namespace Gym.MVVM.Utils.Validation;

/// <summary>
/// Mark a property to say that changes in this property do not make a model dirty.
/// </summary>
[AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
sealed class DoesNotMakeDirtyAttribute : Attribute
{
}