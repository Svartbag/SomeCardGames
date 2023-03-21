using CardGames.War;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using CardGames.Model;
using CardGames.UI.Events;

namespace CardGames.UI.ViewModels.War
{
  public class WarHandViewModel : ViewModelBase, IWarHandViewModel
  {
    private string _mainPileImage = string.Empty;
    private string _secondaryPileImage = string.Empty;
    private string _userSpecificGameText = string.Empty;
    private readonly ICardGameWar _game;
    private readonly IEventAggregator _eventAggregator;
    private int _userIndex = 0;

    public WarHandViewModel(
      ICardGameWar game,
      IList<User> usersInCurentGame,
      int userIndex,
      string playerNameHeadline,
      IEventAggregator eventAggregator
      )
    {
      _game = game;
      UsersInCurentGame = usersInCurentGame;
      UserIndex = userIndex;
      PlayerNameHeadline = playerNameHeadline;
      _eventAggregator = eventAggregator;

      Collect = new DelegateCommand<int?>(OnCollectExecute, OnCollectkCanExecute);
      PlaceCardFromDeckOnTable = new DelegateCommand<int?>(OnPlaceCardFromDeckOnTableExecute, OnPlaceCardFromDeckOnTableCanExecute);
      MoveCardsFromSecondaryPileToMainPile = new DelegateCommand<int?>(OnMoveCardsFromSecondaryPileToMainPileExecute, OnMoveCardsFromSecondaryPileToMainPileCanExecute);
    }

    public string MainPileImage
    {
      get => _mainPileImage;
      set
      {
        _mainPileImage = value;
        OnPropertyChanged();
      }
    }

    public string SecondaryPileImage
    {
      get => _secondaryPileImage;
      set
      {
        _secondaryPileImage = value;
        OnPropertyChanged();
      }
    }

    public IList<User> UsersInCurentGame { get; private set; } = new List<User>();

    public int UserIndex
    {
      get => _userIndex;
      private set
      {
        _userIndex = value;
        OnPropertyChanged();
      }
    }

    public string PlayerNameHeadline { get; private set; } = string.Empty;

    public string UserSpecificGameText
    {
      get => _userSpecificGameText;
      private set
      {
        _userSpecificGameText = value;
        OnPropertyChanged();
      }
    }

    public ICommand Collect { get; }

    public ICommand PlaceCardFromDeckOnTable { get; }

    public ICommand MoveCardsFromSecondaryPileToMainPile { get; }

    public void UpdateUserSpecificGameText(string info)
    {
      UserSpecificGameText = info;
    }

    private void UpdateMainPileImages()
    {
      MainPileImage = updateMainPileImage();
    }

    private void UpdateSecondaryPileImages()
    {
      SecondaryPileImage = updateSecondaryPileImage();
    }

    private string updateMainPileImage()
    {
      if (_game.Players[UserIndex].CardsMainPile.Count() == 0)
      {
        return string.Empty;
      }

      if (_game.Players[UserIndex].CardsMainPile.Count() == 2)
      {
        return $"{_game.Players[UserIndex].config.ImagePath}BackSidePileMedium.png";
      }
      if (_game.Players[UserIndex].CardsMainPile.Count() > 2)
      {
        return $"{_game.Players[UserIndex].config.ImagePath}BackSidePileALot.png";
      }

      string PlayerHandImage = $"{_game.Players[UserIndex].config.ImagePath}BackSide.png";
      if (_game.Players[UserIndex].CardsMainPile.Count() < 2)
      {
        Random rnd = new();
        int randomNumberBetweenZeroAndTwo = rnd.Next(2);
        if (randomNumberBetweenZeroAndTwo == 1)
        {
          PlayerHandImage = $"{_game.Players[UserIndex].config.ImagePath}BackSide2.png";
        }
        if (randomNumberBetweenZeroAndTwo == 2)
        {
          PlayerHandImage = $"{_game.Players[UserIndex].config.ImagePath}BackSide3.png";
        }
      }
      return PlayerHandImage;
    }

    private string updateSecondaryPileImage()
    {
      return _game.Players[UserIndex].CardsSecondaryPile.Count() == 2
          ? $"{_game.Players[UserIndex].config.ImagePath}BackSidePileMedium.png"
          : _game.Players[UserIndex].CardsSecondaryPile.Count() > 2
        ? $"{_game.Players[UserIndex].config.ImagePath}BackSidePileALot.png"
        : string.Empty;
    }

    private void updateImages()
    {
      UpdateMainPileImages();
      UpdateSecondaryPileImages();
    }

    private void updateButtons()
    {
      ((DelegateCommand<int?>)Collect).RaiseCanExecuteChanged();
      ((DelegateCommand<int?>)PlaceCardFromDeckOnTable).RaiseCanExecuteChanged();
      ((DelegateCommand<int?>)MoveCardsFromSecondaryPileToMainPile).RaiseCanExecuteChanged();
    }

    public void UpdateImagesAndButtons()
    {
      updateImages();
      updateButtons();
    }

    #region Event handling
    private bool OnCollectkCanExecute(int? userIndex)
    {
      if (!_game.GameStarted || !userIndex.HasValue)
      {
        return false;
      }
      int index = userIndex ?? 0;
      return _game.CanCollectCardsFromTable(_game.Players[index].UserId)
      || (_game.GameStatus.WarIsOn && _game.GameStatus.WarCardsCanBeMovedToWinningCardPile);
    }

    private void OnCollectExecute(int? userIndex)
    {
      if (!_game.GameStarted || !userIndex.HasValue)
      {
        return;
      }
      int index = userIndex ?? 0;
      _game.CollectCardsFromTable(_game.Players[index].UserId);
      bool cardsHaveBeenCollected = _game.Players[index].GameAtributes.CanCollectCardsFromTable
                                 && _game.GameStatus.CardsCollectedFromTable;
      bool warCardsShouldBeMovedToWinningPile = _game.GameStatus.WarIsOn && _game.GameStatus.WarCardsCanBeMovedToWinningCardPile;
      if (cardsHaveBeenCollected)
      {
        if (!weHaveAWinner())
        {
          _game.ResetGameAtributesForAll();
        }
        _eventAggregator.GetEvent<UpdateAllEvent>().Publish();
      }
      else if (warCardsShouldBeMovedToWinningPile)
      {
        _eventAggregator.GetEvent<UpdateAllEvent>().Publish();
      }
    }

    private bool weHaveAWinner()
    {
      return _game.Players.Any(x => x.GameAtributes.HasWonTheGame == true);
    }

    private bool OnPlaceCardFromDeckOnTableCanExecute(int? userIndex)
    {
      if (!_game.GameStarted || !userIndex.HasValue)
      {
        return false;
      }
      int index = userIndex ?? 0;
      return _game.Players[index].CardsMainPile.Count > 0;
    }

    private void OnPlaceCardFromDeckOnTableExecute(int? userIndex)
    {
      if (!_game.GameStarted || !userIndex.HasValue)
      {
        return;
      }
      int index = userIndex ?? 0;
      _game.TryPlaceCardOnTable(_game.Players[index].UserId);
      _eventAggregator.GetEvent<UpdateHandAndCardsOnTableEvent>().Publish();
    }

    private bool OnMoveCardsFromSecondaryPileToMainPileCanExecute(int? userIndex)
    {
      if (!_game.GameStarted || !userIndex.HasValue)
      {
        return false;
      }
      int index = userIndex ?? 0;
      return _game.Players[index].CardsMainPile.Count == 0
        && _game.Players[index].CardsSecondaryPile.Count > 0;
    }

    private void OnMoveCardsFromSecondaryPileToMainPileExecute(int? userIndex)
    {
      if (!_game.GameStarted || !userIndex.HasValue)
      {
        return;
      }
      int index = userIndex ?? 0;
      _game.TryMoveCardsFromSecondaryToMainPile(_game.Players[index].UserId);
      _eventAggregator.GetEvent<UpdateHandAndCardsOnTableEvent>().Publish();
    }

    #endregion


  }
}
