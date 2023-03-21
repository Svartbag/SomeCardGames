using System.Collections.Generic;
using CardGames.Model;

namespace CardGames.UI.Data
{
  internal class UserDataService : IUserDataService
  {
    public IEnumerable<User> GetAll()
    {
      // To do: Get user info from database
      yield return new User { Id = 1, Name = "Pingvin", Level = 1, GamesPlayed = 0, GamesWon = 0 };
      yield return new User { Id = 2, Name = "Chokofant", Level = 3, GamesPlayed = 20, GamesWon = 3 };
      yield return new User { Id = 3, Name = "Jeppe", Level = 30, GamesPlayed = 100, GamesWon = 99 };
      yield return new User { Id = 4, Name = "Vaskebjørn", Level = 20, GamesPlayed = 80, GamesWon = 33 };
      yield return new User { Id = 5, Name = "Pelikan", Level = 20, GamesPlayed = 80, GamesWon = 33 };
    }
  }
}

