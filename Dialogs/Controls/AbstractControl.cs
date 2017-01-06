using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Dialogs.Controls
{
  public abstract class AbstractControl<T> : IDialogControl<T>
  {
    private string name;
    private bool isVisible;
    private bool isEnabled;
    private bool isRequired;
    private T value;
    public event PropertyChangedEventHandler PropertyChanged;

    public string Name
    {
      get { return name; }
      set
      {
        name = value;
        OnPropertyChanged();
      }
    }

    public bool IsRequired
    {
      get { return isRequired; }
      set
      {
        isRequired = value;
        OnPropertyChanged();
      }
    }

    public bool IsVisible
    {
      get { return isVisible; }
      set
      {
        isVisible = value;
        OnPropertyChanged();
      }
    }

    public bool IsEnabled
    {
      get { return isEnabled; }
      set
      {
        isEnabled = value;
        OnPropertyChanged();
      }
    }

    public T Value
    {
      get { return value; }
      set
      {
        this.value = value;
        OnPropertyChanged();
      }
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected AbstractControl()
    {
      IsEnabled = true;
      IsVisible = true;
      IsRequired = false;
    }
  }
}