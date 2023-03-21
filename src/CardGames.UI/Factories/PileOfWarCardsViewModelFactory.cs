using CardGames.War;
using CardGames.UI.ViewModels.War;

namespace CardGames.UI.Factories
{
  public class WarPileOfWarCardsViewModelFactory : IWarPileOfWarCardsViewModelFactory
  {
    private readonly ICardGameWar _game;

    public WarPileOfWarCardsViewModelFactory(ICardGameWar game)
    {
      _game = game;
    }

    public IWarPileOfWarCardsViewModel Create()
    {
      return new WarPileOfWarCardsViewModel(_game);
    }
  }
}

