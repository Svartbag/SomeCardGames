namespace CardGames.War
{
  public class GameAtributes
  {
    public GameAtributes()
    {
      CardsCollectedFromTable = false;
      WarIsOn = false;
      WarCardsCanBeMovedToWinningCardPile = false;
      SufficientWarCardsFromUsersOnTheTable = false;
    }
    public bool CardsCollectedFromTable { get; set; }
    public bool WarIsOn { get; set; }
    public bool WarCardsCanBeMovedToWinningCardPile { get; set; }
    public bool SufficientWarCardsFromUsersOnTheTable { get; set; }

  }
}
