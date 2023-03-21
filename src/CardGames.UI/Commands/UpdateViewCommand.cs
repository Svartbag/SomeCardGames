using System;
using CardGames.UI.Enums;
using CardGames.UI.Factories;
using CardGames.UI.ViewModels;
using CardGames.UI.ViewModels.War;

namespace CardGames.UI.Commands
{
  public class UpdateViewCommand : IUpdateViewCommandWrapper
  {
    private readonly IMainViewModel _viewModel;
    private readonly ICardGameFactory _cardGameFactory;
    private readonly IWarMainViewModel _warMainViewModel;

    public UpdateViewCommand(
      IMainViewModel viewModel,
      ICardGameFactory cardGameFactory,
      IWarMainViewModel warMainViewModel
      )
    {
      _viewModel = viewModel;
      _cardGameFactory = cardGameFactory;
      _warMainViewModel = warMainViewModel;
    }

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter)
    {
      return true;
    }

    public void Execute(object? parameter)
    {
      GameType gameType = GameType.Undefined;
      if (parameter is not null && parameter.ToString() == "StartCardGameWar")
      {
        gameType = GameType.War;
      }

      _viewModel.SelectedViewModel = _cardGameFactory.CreateMainPageViewModel(
        gameType,
        _warMainViewModel
        );
    }
  }
}
