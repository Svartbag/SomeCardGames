using System.Collections.Generic;
using System.Windows.Input;
using CardGames.Model;

namespace CardGames.UI.ViewModels.War
{
  public interface IWarHandViewModel
  {
    public void UpdateImagesAndButtons();
    public void UpdateUserSpecificGameText(string info);
    public string MainPileImage { get; set; }
    public string SecondaryPileImage { get; set; }
    public IList<User> UsersInCurentGame { get; }
    public int UserIndex { get; }
    public string PlayerNameHeadline { get; }
    public string UserSpecificGameText { get; }
    public ICommand PlaceCardFromDeckOnTable { get; }
    public ICommand MoveCardsFromSecondaryPileToMainPile { get; }
    public ICommand Collect { get; }
  }
}