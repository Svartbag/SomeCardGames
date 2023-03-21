using System.Collections.ObjectModel;

namespace CardGames.UI.ViewModels.War
{
  public interface IWarCardsOnTableViewModel
  {
    string CardOnTableImage { get; set; }
    ObservableCollection<string> WarCardImages { get; }
    public void UpdateImagesAndButtons();
    public int UserIndex { get; }
  }
}