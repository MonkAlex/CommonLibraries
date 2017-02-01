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
      InitializeLogging();
      try
      {
        AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .SetupWithoutStarting();

        var dialog = new SimpleDialog();
        dialog.Buttons.AddCancel();
        dialog.Title = "Example";
        dialog.Description = "Just do it!";
        var control = new StringControl() { Name = "Test", IsRequired = true };
        dialog.Controls.Add(control);

        var boolC = new BoolControl() { Name = "Galochka", Value = true };
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
        custom.OnClick += (o, a) =>
        {
          a.CloseAfterClick = false;
          control.Value += "tu~";
          custom.IsEnabled = false;
          dialog.Buttons.DefaultButton = dialog.Buttons.SingleOrDefault(b => b.Name == "Ok");
        };
        dialog.Buttons.AddButton(custom);
        var dialogResult = dialog.Show();
        control.Value = dialogResult.Name;
        dialog.Show();
        throw new NotImplementedException();
      }
      catch (Exception ex)
      {
        Logger.Error("MainLoop", ex, "Binding produced invalid value for {$ex}", ex);
      }
    }

    private static void InitializeLogging()
    {
#if DEBUG
      SerilogLogger.Initialize(new LoggerConfiguration()
          .MinimumLevel.Debug()
          .WriteTo.RollingFile(outputTemplate: "{Area}: {Message}\r\n", pathFormat: "Debug-{Date}.log")
          .CreateLogger());
#endif
    }
  }
}
