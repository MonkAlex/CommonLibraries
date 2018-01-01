using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ReactiveUI;
using System.Reactive.Subjects;

namespace Dialogs.Avalonia.Dialogs
{
  public class DialogWindow : Window
  {
    public IButton ResultButton { get; private set; }

    public DialogWindow()
    {
      this.InitializeComponent();
    }

    public DialogWindow(IDialog dialog) : this()
    {
      DataContext = dialog;
      Closed += (sender, args) =>
      {
        if (ResultButton == null)
          ResultButton = dialog.Buttons.CancelButton;
      };
      dialog.Buttons.Clicked.Subscribe(args => ButtonsOnClicked(this, args));
    }

    private void ButtonsOnClicked(object sender, ButtonArgs buttonArgs)
    {
      if (buttonArgs.CloseAfterClick)
      {
        ResultButton = buttonArgs.Button;
        Close();
      }
    }

    private void InitializeComponent()
    {
      AvaloniaXamlLoader.Load(this);
    }
  }
}
