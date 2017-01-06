namespace Dialogs.Buttons
{
  public static class DefaultButtons
  {
    public static Button OkButton { get; } = new OkButtonImpl();

    public static Button CancelButton { get; } = new CancelButtonImpl();

    private class OkButtonImpl : Button
    {
      public OkButtonImpl()
      {
        Name = "Ok";
        IsDefault = true;
      }
    }

    private class CancelButtonImpl : Button
    {
      public CancelButtonImpl()
      {
        Name = "Cancel";
        IsCancel = true;
      }
    }
  }
}