using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Dialogs.Avalonia.Dialogs
{
  public class DialogWindow : Window
  {
    public IButton ResultButton { get; private set; }

    public DialogWindow()
    {
      this.InitializeComponent();
      App.AttachDevTools(this);
    }

    public DialogWindow(IDialog dialog) : this()
    {
      DataContext = dialog;
      Closed += (sender, args) =>
      {
        if (ResultButton == null)
          ResultButton = dialog.Buttons.CancelButton;
      };
      dialog.Buttons.Clicked += ButtonsOnClicked;
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
