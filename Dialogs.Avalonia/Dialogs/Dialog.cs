using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using Dialogs.Avalonia.Dialogs;
using Dialogs.Buttons;
using ReactiveUI;

namespace Dialogs.Avalonia
{
  public class Dialog : ReactiveObject, IDialog
  {
    private string title;
    private string description;

    public string Title
    {
      get => title;
      set => this.RaiseAndSetIfChanged(ref title, value);
    }

    public string Description
    {
      get => description;
      set => this.RaiseAndSetIfChanged(ref description, value);
    }

    public IButtonCollection Buttons { get; }
    public ICollection<IDialogControl> Controls { get; }

    public virtual IButton Show()
    {
      CancellationTokenSource source = new CancellationTokenSource();
      
      var dialog = new DialogWindow(this);
      dialog.Closed += (sender, args) => source.Cancel();
      dialog.Show();

      Dispatcher.UIThread.MainLoop(source.Token);
      return dialog.ResultButton;
    }

    public virtual async Task<IButton> ShowAsync()
    {
      var dialog = new DialogWindow(this);
      await dialog.ShowDialog();
      return dialog.ResultButton;
    }

    public Dialog()
    {
      Buttons = new ButtonCollection();
      Controls = new List<IDialogControl>();
    }
  }
}
