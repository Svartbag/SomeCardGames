using CardGames.War;
using System.Collections.ObjectModel;
using System.Linq;
using CardGames.UI.helpers;
using CardGames.Core;

namespace CardGames.UI.ViewModels.War
{
  public class WarCardsOnTableViewModel : ViewModelBase, IWarCardsOnTableViewModel
  {
    private ObservableCollection<string> _warCardImages =
        new(){ string.Empty,
                                            string.Empty,
                                            string.Empty,
                                            string.Empty,
                                            string.Empty
                                          };
    private string _cardOnTableImage = string.Empty;
    private readonly ICardGameWar _game;
    private int _userIndex = 0;

    public WarCardsOnTableViewModel(ICardGameWar game, int userIndex)
    {
      _game = game;
      UserIndex = userIndex;
    }

    public int UserIndex
    {
      get => _userIndex;
      private set
      {
        _userIndex = value;
        OnPropertyChanged();
      }
    }

    public string CardOnTableImage
    {
      get => _cardOnTableImage;
      set
      {
        _cardOnTableImage = value;
        OnPropertyChanged();
      }
    }

    public ObservableCollection<string> WarCardImages
    {
      get => _warCardImages;
      private set
      {
        _warCardImages = value;
        OnPropertyChanged();
      }
    }

    public void UpdateImagesAndButtons()
    {
      UpdateCardOnTableImage();
      UpdateWarCardImages();
    }

    private void UpdateCardOnTableImage()
    {
      var cardOnTheTable = _game.Players[UserIndex].CardOnTheTable;
      string cardOnTheTableImage = string.Empty;
      if (cardOnTheTable is not null)
      {
        cardOnTheTableImage = $"{_game.Players[UserIndex].config.ImagePath}{CardToImageName.Convert(cardOnTheTable)}.png";
      }
      CardOnTableImage = cardOnTheTableImage;
    }

    private void UpdateWarCardImages()
    {
      ObservableCollection<string> warCards = new();
      int amountOfWarCards = _game.Players[UserIndex].WarCardsOnTheTable.Count();
      for (int i = 0; i < amountOfWarCards; i++)
      {
        CardGames.Core.French.Cards.Card warCard = _game.Players[UserIndex].WarCardsOnTheTable[i];
        warCards.Add($"{_game.Players[UserIndex].config.ImagePath}{CardToImageName.Convert(warCard)}.png");
      }
      while (warCards.Count() < 5)
      {
        warCards.Add(string.Empty);
      }
      WarCardImages = warCards;
    }
  }
}
