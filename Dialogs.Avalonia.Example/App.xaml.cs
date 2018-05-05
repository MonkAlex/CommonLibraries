using System;
using System.Linq;
using System.Reactive.Linq;
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

    public static AppBuilder BuildAvaloniaApp()
      => AppBuilder.Configure<App>()
        .UsePlatformDetect()
        .UseReactiveUI();

    public override void Initialize()
    {
      AvaloniaXamlLoader.Load(this);
      base.Initialize();
    }

    static void Main(string[] args)
    {
      BuildAvaloniaApp().SetupWithoutStarting();

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

      var rci = selector.Buttons.AddButton("Retry-Cont-Ignore");
      rci.OnClick.Subscribe(a =>
      {
        a.CloseAfterClick = false;
        RetryContinueIgnoreDialog();
      });

      var autoclose = selector.Buttons.AddButton("Autoclose");
      autoclose.OnClick.Subscribe(a =>
      {
        a.CloseAfterClick = false;
        AutoCloseDialog();
      });

      selector.Buttons.AddButton("Exit");

      selector.Show();
    }

    private static void AutoCloseDialog()
    {
      var dialog = new Dialog();
      var progress = new ProgressControl
      {
        MinValue = 0,
        Value = 0,
        MaxValue = 100
      };
      dialog.Controls.Add(progress);
      Task.Run(async () =>
      {
        while (progress.Value < progress.MaxValue)
        {
          await Task.Delay(50);
          progress.Value += 1;
          dialog.Description = $"Do {progress.Value} of {progress.MaxValue}";
        }
      });
      progress.Changed
        .Where(p => progress.Value >= progress.MaxValue)
        .Subscribe(async agrs => await dialog.CloseAsync());
      dialog.Show();
    }

    private static void RetryContinueIgnoreDialog()
    {
      var dialog = new Dialog();
      dialog.Title = "Error";
      dialog.Description = "  -  When in compatibility mode, and the JavaFX/SWT primary windows are closed, we want to make sure that the SystemTray is also \r\n    closed.  Additionally, when using the Swing tray type, Windows does not always remove the tray icon if the JVM is stopped, \r\n    and this makes sure that the tray is also removed from the notification area. \r\n    This property is available to disable this functionality in situations where you don\'t want this to happen.\r\n    This is an advanced feature, and it is recommended to leave as true.";
      var progress = new ProgressControl();
      progress.MaxValue = 100;
      progress.Changed
        .Where(p => p.PropertyName == nameof(progress.Value) && progress.Value >= progress.MaxValue)
        .Subscribe(a =>
      {
        foreach (var button in dialog.Buttons.Where(b => !b.IsCancel))
          button.IsEnabled = false;
      });
      dialog.Controls.Add(progress);

      var retry = dialog.Buttons.AddButton("Retry");
      retry.OnClick.Subscribe(a =>
      {
        a.CloseAfterClick = false;
      });

      var cont = dialog.Buttons.AddButton("Continue");
      cont.OnClick.Subscribe(a =>
      {
        a.CloseAfterClick = false;
        progress.Value += 5;
      });

      dialog.Buttons.AddButton("Ignore")
        .OnClick.Subscribe(a =>
      {
        a.CloseAfterClick = false;
        progress.Value += 20;
      });

      dialog.Buttons.AddCancel();

      dialog.Show();
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
      var control = new StringControl() { Name = "Test", IsRequired = true };
      dialog.Controls.Add(control);

      var boolC = new BoolControl() { Name = "Checkbox", Value = true };
      dialog.Controls.Add(boolC);

      var p = new ProgressControl() { Name = "Ppp", Value = 66, MinValue = 0, MaxValue = 666 };
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

      var custom = new Buttons.Button() { IsDefault = true, Name = "TRA TA TA" };
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
