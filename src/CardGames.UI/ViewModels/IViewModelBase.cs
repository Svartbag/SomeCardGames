namespace CardGames.UI.ViewModels
{
  public interface IViewModelBase
  {
    virtual void OnPropertyChanged(string propertyName) { }
  }
}