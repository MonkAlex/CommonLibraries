using System.Windows;

namespace Dialogs.WPF
{
  /// <summary>
  /// Interaction logic for DialogWindow.xaml
  /// </summary>
  public partial class DialogWindow : Window
  {
    public IButton ResultButton { get; private set; }

    private DialogWindow()
    {
      InitializeComponent();
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
  }
}
