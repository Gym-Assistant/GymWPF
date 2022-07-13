using System.Collections;
using System.ComponentModel;
using System.Reflection;
using Gym.MVVM.Utils.Validation;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using PropertyChanged;

namespace Gym.MVVM.Utils;

/// <summary>
/// Base model for editable data with validation.
/// </summary>
public abstract class EditableModel : ObservableObject, INotifyDataErrorInfo, ITouchable, IValidatable
{
    private readonly IReadOnlyCollection<string> ignoreDirtyUpdates;
    private bool isDirty;

    /// <summary>
    /// Handles validation of the current model.
    /// </summary>
    public ModelValidator Validator { get; }

    /// <summary>
    /// True if object is changed after edit dialog open.
    /// </summary>
    [DoNotNotify]
    [DoesNotMakeDirty]
    public virtual bool IsDirty
    {
        get => isDirty;
        set
        {
            isDirty = value;

            if (isDirty == false)
            {
                Validator?.ClearValidation();
            }
        }
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    public EditableModel()
    {
        ignoreDirtyUpdates = GetIgnoreDirtyFields();
        Validator = new ModelValidator(this);
        Validator.ErrorsChanged += Validator_ErrorsChanged;
        PropertyChanged += EditableModel_PropertyChanged;
    }

    private void Validator_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
    {
        ErrorsChanged?.Invoke(sender, e);
        OnPropertyChanged(nameof(IsValid));
    }

    private void EditableModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (ignoreDirtyUpdates.Contains(e.PropertyName))
        {
            return;
        }
        IsDirty = true;
    }

    /// <summary>
    /// Is the model valid.
    /// </summary>
    [DoNotNotify]
    [DoesNotMakeDirty]
    public virtual bool IsValid => !Validator.HasErrors;

    #region INotifyDataErrorInfo

    /// <inheritdoc />
    public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

    /// <inheritdoc />
    public bool HasErrors => !IsValid;

    /// <inheritdoc />
    public IEnumerable GetErrors(string propertyName) => Validator.GetErrors(propertyName);

    #endregion

    private HashSet<string> GetIgnoreDirtyFields()
    {
        var propertyNames = GetAllProperties(GetType())
            .Where(prop => prop.IsDefined(typeof(DoesNotMakeDirtyAttribute), true))
            .Select(p => p.Name);

        var hashSet = new HashSet<string>(propertyNames);
        hashSet.Add(string.Empty);
        return hashSet;
    }

    private IEnumerable<PropertyInfo> GetAllProperties(Type type)
    {
        IEnumerable<PropertyInfo> properties = type.GetProperties();

        if (type.BaseType != null)
        {
            properties = properties.Concat(GetAllProperties(type.BaseType));
        }

        return properties;
    }

    /// <inheritdoc />
    public virtual void Touch(bool revalidate = true)
    {
        OnPropertyChanged(string.Empty);
        if (revalidate)
        {
            Validate();
        }
    }

    /// <inheritdoc />
    public virtual void Validate()
    {
        Validator.Validate();
    }

    /// <summary>
    /// Set dirty status of this model and all nested models.
    /// </summary>
    /// <param name="dirty">IsDirty status.</param>
    public virtual void SetGlobalDirty(bool dirty)
    {
        IsDirty = dirty;
    }
}