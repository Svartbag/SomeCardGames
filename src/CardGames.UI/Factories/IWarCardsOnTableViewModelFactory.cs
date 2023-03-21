using CardGames.UI.ViewModels.War;

namespace CardGames.UI.Factories
{
  public interface IWarCardsOnTableViewModelFactory
  {
    public IWarCardsOnTableViewModel Create(int userIndex);
  }
}