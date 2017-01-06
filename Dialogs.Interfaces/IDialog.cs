using System.Collections.Generic;
using System.ComponentModel;

namespace Dialogs
{
  public interface IDialog : INotifyPropertyChanged
  {
    string Title { get; set; }

    string Description { get; set; }

    IButtonCollection Buttons { get; }

    ICollection<IDialogControl> Controls { get; }

    IButton Show();
  }
}
