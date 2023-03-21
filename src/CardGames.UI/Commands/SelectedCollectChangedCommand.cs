using System;
using System.Windows.Input;

namespace CardGames.UI.Commands
{
  public class SelectedCollectChangedCommand : ICommand
  {
    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter)
    {
      return true;
    }

    public void Execute(object parameter)
    {

    }
  }
}
