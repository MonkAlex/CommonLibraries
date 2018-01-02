using System;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Diagnostics;
using Avalonia.Logging;
using Avalonia.Logging.Serilog;
using Avalonia.Themes.Default;
using Avalonia.Markup.Xaml;
using Dialogs.Controls;
using Serilog;

namespace Dialogs.Avalonia.Example
{
  class App : Application
  {

    public override void Initialize()
    {
      AvaloniaXamlLoader.Load(this);
      base.Initialize();
    }

    static void Main(string[] args)
    {
      try
      {
        AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .SetupWithoutStarting();

        var selector = new Dialog();
        selector.Title = "Select dialog";
        selector.Description = "Some different dialogs:";
        var all = selector.Buttons.AddButton("All controls");
        all.OnClick.Subscribe(a =>
        {
          a.CloseAfterClick = false;
          SimpleDialog();
        });

        var strings = selector.Buttons.AddButton("Strings");
        strings.OnClick.Subscribe(a =>
        {
          a.CloseAfterClick = false;
          StringDialog();
        });

        selector.Buttons.AddButton("Exit");

        selector.Show();
        throw new NotImplementedException();
      }
      catch (Exception ex)
      {
        Logger.Error("MainLoop", ex, "Binding produced invalid value for {$ex}", ex);
      }
    }

    private static void StringDialog()
    {
      var dialog = new SimpleDialog();
      dialog.Controls.Add(new StringControl() { Name = "11111111111111111" });
      dialog.Controls.Add(new StringControl() { Name = "1" });
      dialog.Controls.Add(new StringControl() { Name = "12345" });
      dialog.Controls.Add(new StringControl() { Name = "1234567890" });
      dialog.Show();
    }

    private static void SimpleDialog()
    {
      var dialog = new SimpleDialog();
      dialog.Buttons.AddCancel();
      dialog.Title = "Example";
      dialog.Description = "Just do it!";
      var control = new StringControl() {Name = "Test", IsRequired = true};
      dialog.Controls.Add(control);

      var boolC = new BoolControl() {Name = "Checkbox", Value = true};
      dialog.Controls.Add(boolC);

      var p = new ProgressControl() {Name = "Ppp", Value = 66, MinValue = 0, MaxValue = 666};
      dialog.Controls.Add(p);

      Task.Run(() =>
      {
        while (true)
        {
          Task.Delay(TimeSpan.FromSeconds(1)).Wait();
          p.Value = p.Value + 10;
          p.MaxValue = p.MaxValue - 10;
        }
      });

      var custom = new Buttons.Button() {IsDefault = true, Name = "TRA TA TA"};
      custom.OnClick.Subscribe(a =>
      {
        a.CloseAfterClick = false;
        control.Value += "tu~";
        custom.IsEnabled = false;
        dialog.Buttons.DefaultButton = dialog.Buttons.SingleOrDefault(b => b.Name == "Ok");
      });
      dialog.Buttons.AddButton(custom);
      var dialogResult = dialog.Show();
      control.Value = dialogResult.Name;
      dialog.Show();
    }
  }
}
