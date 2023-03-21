namespace CardGames.War
{
  public class GameAtributesUserSpecific
  {
    public GameAtributesUserSpecific()
    {
      HasWonTheGame = false;
      CanCollectCardsFromTable = false;
      SufficientAmountOfWarAreCardsAreOnTheTable = false;
    }
    public bool HasWonTheGame { get; set; }
    public bool CanCollectCardsFromTable { get; set; }
    public bool SufficientAmountOfWarAreCardsAreOnTheTable { get; set; }

  }
}
