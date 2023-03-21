using CardGames.UI.Enums;
using CardGames.UI.ViewModels;
using CardGames.UI.ViewModels.Poker;
using CardGames.UI.ViewModels.War;

namespace CardGames.UI.Factories
{
  public class CardGameFactory : ICardGameFactory
  {
    public IViewModelBaseWrapper CreateMainPageViewModel(
        GameType gameType,
        IWarMainViewModel warMainViewModel
      )
    {
      return gameType switch
      {
        GameType.War => (IViewModelBaseWrapper)warMainViewModel,
        GameType.Poker => new CardGamePokerViewModel(),
        _ => new StartPageViewModel(),
      };
    }
  }
}

