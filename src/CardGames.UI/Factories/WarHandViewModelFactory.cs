using CardGames.War;
using Prism.Events;
using System.Collections.Generic;
using CardGames.Model;
using CardGames.UI.ViewModels.War;
namespace CardGames.UI.Factories
{
  public class WarHandViewModelFactory : IWarHandViewModelFactory
  {
    private readonly ICardGameWar _game;
    private readonly IEventAggregator _eventAggregator;

    public WarHandViewModelFactory(ICardGameWar game, IEventAggregator eventAggregator)
    {
      _game = game;
      _eventAggregator = eventAggregator;
    }
    public IWarHandViewModel Create(
      IList<User> users,
      int userIndex,
        string playerNameHeadline
      )
    {
      return new WarHandViewModel(_game, users, userIndex, playerNameHeadline, _eventAggregator);
    }
  }
}
