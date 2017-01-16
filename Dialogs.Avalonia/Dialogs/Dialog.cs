using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Dialogs.Avalonia.Dialogs;
using Dialogs.Buttons;

namespace Dialogs.Avalonia
{
  public abstract class Dialog : IDialog
  {
    private string title;
    private string description;
    public event PropertyChangedEventHandler PropertyChanged;

    public string Title
    {
      get { return title; }
      set
      {
        title = value;
        OnPropertyChanged();
      }
    }

    public string Description
    {
      get { return description; }
      set
      {
        description = value;
        OnPropertyChanged();
      }
    }

    public IButtonCollection Buttons { get; }
    public ICollection<IDialogControl> Controls { get; }

    public virtual async Task<IButton> Show()
    {
      var dialog = new DialogWindow(this);
      await dialog.ShowDialog();
      return dialog.ResultButton;
    }

    protected Dialog()
    {
      Buttons = new ButtonCollection();
      Controls = new List<IDialogControl>();
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
