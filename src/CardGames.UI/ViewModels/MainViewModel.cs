using CardGames.War;
using CardGames.War.Configuration;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CardGames.Model;
using CardGames.UI.Commands;
using CardGames.UI.Data;
using CardGames.UI.Enums;
using CardGames.UI.Factories;
using CardGames.UI.ViewModels.War;

namespace CardGames.UI.ViewModels
{

  public class MainViewModel : ViewModelBase, IMainViewModel
  {
    private readonly IUserDataService _userDataService;
    private readonly ICardGameFactory _cardGameFactory;
    private readonly ICardGameWar _warGameEngine;
    private readonly IWarMainViewModel _warMainViewModel;
    private readonly IGameConfigurationWar _warGameConfiguration;
    private readonly IWarHandViewModelFactory _warHandViewModelFactory;
    private readonly IWarCardsOnTableViewModelFactory _warCardsOnTableViewModelFactory;
    private readonly IWarPileOfWarCardsViewModelFactory _warPileOfWarCardsViewModelFactory;
    private IViewModelBaseWrapper _selectedViewModel;
    public MainViewModel(
      IUserDataService userDataService,
      ICardGameFactory cardGameFactory,
      IWarMainViewModel warMainViewModel,
      ICardGameWar warGameEngine,
      IGameConfigurationWar warGameConfiguration,
      IWarHandViewModelFactory warHandViewModelFactory,
      IWarCardsOnTableViewModelFactory warCardsOnTableViewModelFactory,
      IWarPileOfWarCardsViewModelFactory warPileOfWarCardsViewModelFactory

      )
    {
      Users = new ObservableCollection<User>();
      _userDataService = userDataService;
      LoadUsers();
      _cardGameFactory = cardGameFactory;
      _warGameEngine = warGameEngine;
      _warMainViewModel = warMainViewModel;
      _warGameConfiguration = warGameConfiguration;
      _warHandViewModelFactory = warHandViewModelFactory;
      _warCardsOnTableViewModelFactory = warCardsOnTableViewModelFactory;
      _warPileOfWarCardsViewModelFactory = warPileOfWarCardsViewModelFactory;

      _selectedViewModel = _cardGameFactory.CreateMainPageViewModel(
        GameType.Undefined,
        _warMainViewModel
        );

      UpdateViewCommand = new UpdateViewCommand(
        this,
        _cardGameFactory,
        _warMainViewModel
        );
    }

    public IViewModelBaseWrapper SelectedViewModel
    {
      get => _selectedViewModel;
      set
      {
        _selectedViewModel = value;
        if (_selectedViewModel is IWarMainViewModel)
        {
          StartUpTheWarGame();
        }
        OnPropertyChanged();

      }
    }

    public ICommand UpdateViewCommand { get; set; }

    public void LoadUsers()
    {
      IEnumerable<User> users = _userDataService.GetAll();
      Users.Clear();
      foreach (User user in users)
      {
        Users.Add(user);
      }
    }

    public ObservableCollection<User> Users { get; set; }

    private void StartUpTheWarGame()
    {
      ObservableCollection<User> usersInTheGame = new()
          {
            Users.First(x => x.Name == "Pingvin"),
            Users.First(x => x.Name == "Pelikan")
          };
      IWarMainViewModel tmpSelectedModel = (IWarMainViewModel)_selectedViewModel;
      tmpSelectedModel.StartTheGame(usersInTheGame);
    }
  }
}
