using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Dialogs.Buttons
{
  public class Button : IButton, ICommand
  {
    #region IButton

    private string name;
    private bool isVisible;
    private bool isEnabled;
    private bool isDefault;
    private bool isCancel;
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

    public bool IsDefault
    {
      get { return isDefault; }
      set
      {
        isDefault = value;
        OnPropertyChanged();
      }
    }

    public bool IsCancel
    {
      get { return isCancel; }
      set
      {
        isCancel = value;
        OnPropertyChanged();
      }
    }

    public event EventHandler<ButtonArgs> OnClick;

    public event EventHandler<ButtonArgs> Clicked;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

      if (propertyName == nameof(IsVisible) || propertyName == nameof(IsEnabled))
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    #endregion

    #region ICommand

    public bool CanExecute(object parameter)
    {
      return IsEnabled && IsVisible;
    }

    public void Execute(object parameter)
    {
      var buttonArgs = new ButtonArgs(this);
      OnClick?.Invoke(this, buttonArgs);
      Clicked?.Invoke(this, buttonArgs);
    }

    public event EventHandler CanExecuteChanged;

    #endregion

    #region Equals

    public override bool Equals(object obj)
    {
      var button = obj as Button;
      if (button != null)
        return button.GetType() == GetType() && Equals(button.Name, Name);
      return false;
    }

    public override int GetHashCode()
    {
      return GetType().GetHashCode() ^ Name.GetHashCode();
    }

    public static bool operator ==(Button button1, Button button2)
    {
      if (ReferenceEquals(button1, null))
      {
        return ReferenceEquals(button2, null);
      }

      return button1.Equals(button2);
    }

    public static bool operator !=(Button button1, Button button2)
    {
      return !(button1 == button2);
    }

    #endregion

    public Button()
    {
      IsEnabled = true;
      IsVisible = true;
    }

  }
}
