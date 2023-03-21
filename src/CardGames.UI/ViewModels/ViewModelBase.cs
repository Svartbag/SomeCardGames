using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CardGames.UI.ViewModels
{
  public class ViewModelBase : IViewModelBaseWrapper
  {
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
