using System.Collections.ObjectModel;
using System.Windows.Input;
using CardGames.Model;

namespace CardGames.UI.ViewModels
{
  public interface IMainViewModel
  {
    IViewModelBaseWrapper SelectedViewModel { get; set; }
    ICommand UpdateViewCommand { get; set; }
    void LoadUsers();
    ObservableCollection<User> Users { get; set; }
  }
}