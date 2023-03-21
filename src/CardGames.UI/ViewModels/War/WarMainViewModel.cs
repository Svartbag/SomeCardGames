using CardGames.War;
using CardGames.War.Configuration;
using Prism.Events;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CardGames.Model;
using CardGames.UI.Events;
using CardGames.UI.Factories;

namespace CardGames.UI.ViewModels.War
{

  public partial class WarMainViewModel : ViewModelBase, IWarMainViewModel
  {
    private readonly ICardGameWar _game;
    private readonly IGameConfigurationWar _gameConfig;
    private readonly IWarHandViewModelFactory _warHandViewModelFactory;
    private readonly IWarCardsOnTableViewModelFactory _warCardsOnTableViewModelFactory;
    private readonly IWarPileOfWarCardsViewModelFactory _pileOfWarCardsViewModelFactory;
    private IWarPileOfWarCardsViewModel _pileOfWarCardsViewModel;

    private ObservableCollection<IWarHandViewModel> _handViewModel = new();
    private ObservableCollection<IWarCardsOnTableViewModel> _warCardsOnTableViewModel = new();
    private readonly IEventAggregator _eventAggregator;

    public WarMainViewModel(ICardGameWar game, IGameConfigurationWar gameConfig,
      IWarHandViewModelFactory warHandViewModelFactory,
      IWarCardsOnTableViewModelFactory warCardsOnTableViewModelFactory,
      IWarPileOfWarCardsViewModelFactory pileOfWarCardsViewModelFactory, IEventAggregator eventAggregator)
    {
      _game = game;
      _gameConfig = gameConfig;
      _warHandViewModelFactory = warHandViewModelFactory;
      _warCardsOnTableViewModelFactory = warCardsOnTableViewModelFactory;
      _pileOfWarCardsViewModelFactory = pileOfWarCardsViewModelFactory;
      _eventAggregator = eventAggregator;
      _ = _eventAggregator.GetEvent<UpdateAllEvent>().Subscribe(onUpdateAllImagesAndButtons);
      _ = _eventAggregator.GetEvent<UpdateHandAndCardsOnTableEvent>().Subscribe(onUpdateHandCardsAndTableCards);
    }

    public ObservableCollection<IWarHandViewModel> HandViewModel
    {
      get => _handViewModel;
      private set
      {
        _handViewModel = value;
        OnPropertyChanged();
      }
    }

    public ObservableCollection<IWarCardsOnTableViewModel> WarCardsOnTableViewModel
    {
      get => _warCardsOnTableViewModel;
      private set
      {
        _warCardsOnTableViewModel = value;
        OnPropertyChanged();
      }
    }

    public IWarPileOfWarCardsViewModel PileOfWarCardsViewModel
    {
      get => _pileOfWarCardsViewModel;
      private set
      {
        _pileOfWarCardsViewModel = value;
        OnPropertyChanged();
      }
    }

    public IList<User> UsersInCurentGame { get; private set; } = new List<User>();

    public void StartTheGame(
      ObservableCollection<User> usersInCurentGame
      )
    {
      int amountOfPlayers = 2;
      if (!usersAreValid(usersInCurentGame.ToList(), amountOfPlayers))
      {
        return;
      }

      UsersInCurentGame = usersInCurentGame;

      HandViewModel = new ObservableCollection<IWarHandViewModel>();
      WarCardsOnTableViewModel = new ObservableCollection<IWarCardsOnTableViewModel>();

      for (int i = 0; i < amountOfPlayers; i++)
      {
        HandViewModel.Add(_warHandViewModelFactory.Create(UsersInCurentGame, i, $"Player {i + 1}: {UsersInCurentGame.ElementAt(i).Name}"));
        WarCardsOnTableViewModel.Add(_warCardsOnTableViewModelFactory.Create(i));
      }
      PileOfWarCardsViewModel = _pileOfWarCardsViewModelFactory.Create();

      List<int> usersInCurentGameAsIds = new();
      UsersInCurentGame.ToList().ForEach(x => usersInCurentGameAsIds.Add(x.Id));
      _game.Start(_gameConfig, usersInCurentGameAsIds);

      onUpdateAllImagesAndButtons();
    }

    private bool usersAreValid(List<User> usersInCurentGame, int amountOfPlayers)
    {
      List<int> allUserIds = new();
      usersInCurentGame.ForEach(x => allUserIds.Add(x.Id));
      List<int> uniqueUserIds = allUserIds.Distinct().ToList();
      return usersInCurentGame.Count == amountOfPlayers && uniqueUserIds.Count() == amountOfPlayers;
    }

    private void onUpdateHandCardsAndTableCards()
    {
      updateUserspecificTextForAllUsers();
      HandViewModel.ToList().ForEach(x => x.UpdateImagesAndButtons());
      WarCardsOnTableViewModel.ToList().ForEach(x => x.UpdateImagesAndButtons());
    }

    private void onUpdateAllImagesAndButtons()
    {
      onUpdateHandCardsAndTableCards();
      PileOfWarCardsViewModel.UpdateWarCardPileImage();
    }

    private void onGameHasStopped()
    {
      if (weHaveAWinner())
      {
        foreach (IWarHandViewModel item in HandViewModel)
        {
          if (_game.Players[item.UserIndex].GameAtributes.HasWonTheGame)
          {
            item.UpdateUserSpecificGameText($"You are a winner\nAmount of cards: {_game.Players[item.UserIndex].GetAllCardsOnHand.Count}");
          }
          else
          {
            item.UpdateUserSpecificGameText($"You lost\nAmount of cards: {_game.Players[item.UserIndex].GetAllCardsOnHand.Count}");
          }
        }
      }
      _game.Stop();
    }

    private void updateUserspecificTextForAllUsers()
    {
      if (weHaveAWinner())
      {
        onGameHasStopped();
      }
      else
      {
        HandViewModel.ToList().ForEach(x => x.UpdateUserSpecificGameText($"Cards on hand: {_game.Players[x.UserIndex].GetAllCardsOnHand.Count}"));
      }
    }

    private bool weHaveAWinner()
    {
      return _game.Players.Any(x => x.GameAtributes.HasWonTheGame == true);
    }
  }
}
