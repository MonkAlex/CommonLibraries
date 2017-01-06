using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Dialogs
{
  public interface IButtonCollection : INotifyPropertyChanged, IReadOnlyCollection<IButton>
  {
    IButton DefaultButton { get; set; }

    IButton CancelButton { get; set; }

    event EventHandler<ButtonArgs> Clicked;

    void AddButton(IButton button);
    void AddOkCancel();
    void AddCancel();
  }
}