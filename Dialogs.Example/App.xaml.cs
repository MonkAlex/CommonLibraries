using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Dialogs.Buttons;
using Dialogs.Controls;

namespace Dialogs.Example
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    private void App_OnStartup(object sender, StartupEventArgs e)
    {
      var dialog = new Dialogs.SimpleDialog();
      dialog.Buttons.AddCancel();
      dialog.Title = "Example";
      dialog.Description = "Just do it!";
      var control = new StringControl() {Name = "Test", IsRequired = true};
      dialog.Controls.Add(control);

      var boolC = new BoolControl() {Name = "Galochka", Value = true};
      dialog.Controls.Add(boolC);

      var custom = new Button() {IsDefault = true, Name = "TRA TA TA"};
      custom.OnClick += (o, args) =>
      {
        args.CloseAfterClick = false;
        control.Value += "tu~";
        custom.IsEnabled = false;
        dialog.Buttons.DefaultButton = dialog.Buttons.SingleOrDefault(b => b.Name == "Ok");
      };
      dialog.Buttons.AddButton(custom);
      var dialogResult = dialog.Show();
      if (DefaultButtons.CancelButton != (Button)dialogResult)
      {
        MessageBox.Show(control.Value);
      }
      Current?.Shutdown();
    }
  }
}
