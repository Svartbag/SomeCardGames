namespace CardGames.War.Configuration
{
  public class GameConfigurationWar : IGameConfigurationWar
  {
    public GameConfigurationWar()
    {
      CardsFromDealerPrRound = 2;
      MaxDiffBetweenCardsToHaveWar = 1;
      //NumberOfCardsToAchieveInOrderToWin = 22; // 52;// 4;//22; // 52 normaly
      NumberOfCardsToAchieveInOrderToWin = 52;// 4;//22; // 52 normaly
      MaxNumberOfWarCardsOnTheTable = 4; // Max 5. Otherwise UI will break
      ShuffleCardsAfterMainPileIsEmpty = false;
      ImagePath = "/Resources/Images/PlayingCards/PNGcards/";
    }
    public int CardsFromDealerPrRound { get; set; }
    public int MaxDiffBetweenCardsToHaveWar { get; set; }
    public int NumberOfCardsToAchieveInOrderToWin { get; set; }
    public int MaxNumberOfWarCardsOnTheTable { get; set; }
    public bool ShuffleCardsAfterMainPileIsEmpty { get; set; }
    public string ImagePath { get; set; }
  }
}