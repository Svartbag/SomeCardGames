using CardGames.War;
using System.Linq;

namespace CardGames.UI.ViewModels.War
{
  public class WarPileOfWarCardsViewModel : ViewModelBase, IWarPileOfWarCardsViewModel
  {
    private readonly ICardGameWar _game;
    private string _pileOfWarCardsImage;

    public WarPileOfWarCardsViewModel(ICardGameWar game)
    {
      _game = game;
      _pileOfWarCardsImage = string.Empty;
      PileOfWarCardsImage = _pileOfWarCardsImage;
    }

    public string PileOfWarCardsImage
    {
      get => _pileOfWarCardsImage;
      set
      {
        _pileOfWarCardsImage = value;
        OnPropertyChanged();
      }
    }

    public void UpdateWarCardPileImage()
    {
      bool cardsOnTableCanBeMovedToWinningCardPile = _game.GameStatus.WarIsOn && _game.GameStatus.WarCardsCanBeMovedToWinningCardPile;
      if (cardsOnTableCanBeMovedToWinningCardPile)
      {
        PileOfWarCardsImage = $"{_game.Players.First().config.ImagePath}BackSidePileMedium.png";
      }
      if (!_game.GameStatus.WarIsOn)
      {
        PileOfWarCardsImage = string.Empty;
      }
      _game.GameStatus.WarCardsCanBeMovedToWinningCardPile = false;
    }

  }
}
