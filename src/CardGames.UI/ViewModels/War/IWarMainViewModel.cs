using System.Collections.Generic;
using System.Collections.ObjectModel;
using CardGames.Model;

namespace CardGames.UI.ViewModels.War
{
  public interface IWarMainViewModel
  {
    public ObservableCollection<IWarHandViewModel> HandViewModel { get; }
    public ObservableCollection<IWarCardsOnTableViewModel> WarCardsOnTableViewModel { get; }
    public IWarPileOfWarCardsViewModel PileOfWarCardsViewModel { get; }
    public IList<User> UsersInCurentGame { get; }
    public void StartTheGame(ObservableCollection<User> usersInCurentGame);

  }
}