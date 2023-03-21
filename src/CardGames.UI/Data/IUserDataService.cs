using System.Collections.Generic;
using CardGames.Model;

namespace CardGames.UI.Data
{
  public interface IUserDataService
  {
    IEnumerable<User> GetAll();
  }
}