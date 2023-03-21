using CardGames.UI.Enums;
using CardGames.UI.ViewModels;
using CardGames.UI.ViewModels.War;

namespace CardGames.UI.Factories
{
  public interface ICardGameFactory
  {
    IViewModelBaseWrapper CreateMainPageViewModel(
        GameType gameType,
        IWarMainViewModel warMainViewModel
      );
  }
}