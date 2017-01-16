using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Diagnostics;
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

      var custom = new Buttons.Button() { IsDefault = true, Name = "TRA TA TA" };
      custom.OnClick += (o, a) =>
      {
        a.CloseAfterClick = false;
        control.Value += "tu~";
        custom.IsEnabled = false;
        dialog.Buttons.DefaultButton = dialog.Buttons.SingleOrDefault(b => b.Name == "Ok");
      };
      dialog.Buttons.AddButton(custom);
      var dialogResult = await dialog.Show();
      var ttt = dialogResult.Name;
    }

    public static void AttachDevTools(Window window)
    {
#if DEBUG
            DevTools.Attach(window);
#endif
    }

    private static void InitializeLogging()
    {
#if DEBUG
            SerilogLogger.Initialize(new LoggerConfiguration()
                .MinimumLevel.Warning()
                .WriteTo.Trace(outputTemplate: "{Area}: {Message}")
                .CreateLogger());
#endif
    }
  }
}
