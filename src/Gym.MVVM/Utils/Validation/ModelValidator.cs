using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Gym.MVVM.Utils.Validation;

/// <summary>
/// Validates models.
/// </summary>
public class ModelValidator : INotifyDataErrorInfo
{
    private static IServiceProvider serviceProvider;

    /// <summary>
    /// Register a service provider to be used for all validations.
    /// </summary>
    /// <param name="serviceProvider">Service provider instance.</param>
    public static void SetServiceProvider(IServiceProvider serviceProvider)
    {
        ModelValidator.serviceProvider = serviceProvider;
    }

    private readonly INotifyPropertyChanged target;
    private ValidationContext validationContext;
    private readonly List<ValidationResult> validationResults;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="target">A validatable view model.</param>
    public ModelValidator(INotifyPropertyChanged target)
    {
        this.target = target;

        validationContext = new ValidationContext(target, serviceProvider, null);
        validationResults = new List<ValidationResult>();
        target.PropertyChanged += Validate;
    }

    /// <summary>
    /// Clear validation results.
    /// </summary>
    public void ClearValidation()
    {
        validationResults.Clear();
    }

    private void Validate(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(EditableModel.IsValid))
        {
            return;
        }
        Validate();
    }

    /// <summary>
    /// Force validation on the associated model.
    /// </summary>
    public void Validate()
    {
        var previousValidationErrors = validationResults.SelectMany(v => v.MemberNames)
            .Distinct()
            .ToHashSet();
        validationResults.Clear();
        Validator.TryValidateObject(target, validationContext, validationResults, validateAllProperties: true);
        if (target is IValidatableObject validatableObject)
        {
            ValidateIValidatableObject(validatableObject, validationResults, validationContext);
        }
        var hashSet = new HashSet<string>(validationResults.SelectMany(x => x.MemberNames));
        // Notify other fields if they are no longer invalid
        hashSet.UnionWith(previousValidationErrors);
        foreach (var error in hashSet)
        {
            RaiseErrorsChanged(error);
        }
    }

    /// <inheritdoc/>
    public IEnumerable GetErrors(string propertyName)
    {
        return validationResults
            .Where(x => x.MemberNames.Contains(propertyName))
            .Select(x => x.ErrorMessage);
    }

    /// <inheritdoc/>
    public bool HasErrors => validationResults.Count > 0;

    /// <inheritdoc/>
    public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

    /// <summary>
    /// Set items to be used in validation context.
    /// </summary>
    /// <param name="items">List of items that can be used by validation logic.</param>
    public void SetValidationContextItems(IDictionary<object, object> items)
    {
        ClearValidation();
        validationContext = new ValidationContext(target, serviceProvider, items);
    }

    private void RaiseErrorsChanged(string propertyName)
    {
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
    }

    private static void ValidateIValidatableObject(IValidatableObject validatableObject, IList<ValidationResult> errors, ValidationContext validationContext)
    {
        var validations = validatableObject.Validate(validationContext)
            .Where(vr => vr != ValidationResult.Success)
            .ToList();

        validations.Where(vr => vr.MemberNames == null)
            .ToList()
            .ForEach(vr => errors.Add(new ValidationResult(vr.ErrorMessage)));

        validations.Where(vr => vr.MemberNames != null)
            .SelectMany(vr => vr.MemberNames.Select(mn => new { MemeberName = mn, vr.ErrorMessage }))
            .ToList()
            .ForEach(vr => errors.Add(new ValidationResult(vr.ErrorMessage, new string[] { vr.MemeberName })));
    }
}