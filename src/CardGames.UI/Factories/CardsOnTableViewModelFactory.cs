using CardGames.War;
using CardGames.UI.ViewModels.War;

namespace CardGames.UI.Factories
{
  public class WarCardsOnTableViewModelFactory : IWarCardsOnTableViewModelFactory
  {
    private readonly ICardGameWar _game;

    public WarCardsOnTableViewModelFactory(ICardGameWar game)
    {
      _game = game;
    }
    public IWarCardsOnTableViewModel Create(int userIndex)
    {
      return new WarCardsOnTableViewModel(_game, userIndex);
    }
  }
}

