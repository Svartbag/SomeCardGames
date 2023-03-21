using System.Collections.Generic;
using CardGames.Model;
using CardGames.UI.ViewModels.War;

namespace CardGames.UI.Factories
{
  public interface IWarHandViewModelFactory
  {
    public IWarHandViewModel Create(
              IList<User> users,
              int userIndex,
              string playerNameHeadline);
  }
}