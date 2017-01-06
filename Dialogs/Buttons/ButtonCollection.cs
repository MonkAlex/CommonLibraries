using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Dialogs.Buttons
{
  public class ButtonCollection : IButtonCollection
  {
    private readonly ICollection<IButton> buttons = new List<IButton>();
    public event PropertyChangedEventHandler PropertyChanged;

    public IButton DefaultButton
    {
      get { return buttons.SingleOrDefault(b => b.IsDefault); }
      set
      {
        ChangeDefault(value);
        OnPropertyChanged();
      }
    }

    public IButton CancelButton
    {
      get { return buttons.SingleOrDefault(b => b.IsCancel); }
      set
      {
        ChangeCancel(value);
        OnPropertyChanged();
      }
    }

    public event EventHandler<ButtonArgs> Clicked;

    public void AddButton(IButton button)
    {
      if (buttons.Contains(button))
        return;

      button.Clicked += (sender, args) => OnClicked(args);

      if (button.IsDefault)
        ChangeDefault(button);

      if (button.IsCancel)
        ChangeCancel(button);

      buttons.Add(button);
    }

    public void AddOkCancel()
    {
      AddButton(DefaultButtons.OkButton);

      AddCancel();
    }

    public void AddCancel()
    {
      AddButton(DefaultButtons.CancelButton);
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public IEnumerator<IButton> GetEnumerator()
    {
      return buttons.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return ((IEnumerable) buttons).GetEnumerator();
    }

    public int Count
    {
      get { return buttons.Count; }
    }

    protected virtual void OnClicked(ButtonArgs e)
    {
      Clicked?.Invoke(this, e);
    }

    private void ChangeDefault(IButton newButton)
    {
      var old = buttons.SingleOrDefault(b => b.IsDefault);
      if (old != null)
        old.IsDefault = false;

      newButton.IsDefault = true;
    }

    private void ChangeCancel(IButton newButton)
    {
      var old = buttons.SingleOrDefault(b => b.IsCancel);
      if (old != null)
        old.IsCancel = false;

      newButton.IsCancel = true;
    }
  }
}