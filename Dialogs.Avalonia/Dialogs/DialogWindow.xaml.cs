using System;
using System.Linq;
using System.Reactive.Linq;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ReactiveUI;
using System.Reactive.Subjects;
using Avalonia;

namespace Dialogs.Avalonia.Dialogs
{
  public class DialogWindow : Window
  {
    public IButton ResultButton { get; private set; }

    public DialogWindow()
    {
      this.InitializeComponent();
      if (Owner == null && WindowStartupLocation == WindowStartupLocation.CenterOwner)
        Owner = Application.Current.Windows.FirstOrDefault(w => w.IsActive);
    }

    public DialogWindow(IDialog dialog) : this()
    {
      DataContext = dialog;
      var sub = dialog.Buttons.Clicked
        .Where(b => b.CloseAfterClick)
        .Subscribe(args =>
        {
          ResultButton = args.Button;
          Close();
        });
      Closed += (sender, args) =>
      {
        sub?.Dispose();
        if (ResultButton == null)
          ResultButton = dialog.Buttons.CancelButton;
      };
    }

    private void InitializeComponent()
    {
      AvaloniaXamlLoader.Load(this);
    }
  }
}
