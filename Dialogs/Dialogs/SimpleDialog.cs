using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dialogs.Buttons;
using Dialogs.WPF;

namespace Dialogs
{
  public class SimpleDialog : Dialog
  {
    public SimpleDialog()
    {
      Buttons.AddOkCancel();
    }
  }
}
