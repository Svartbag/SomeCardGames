namespace CardGames.War.Configuration
{
  public interface IGameConfigurationWar
  {
    int CardsFromDealerPrRound { get; set; }
    int MaxDiffBetweenCardsToHaveWar { get; set; }
    int NumberOfCardsToAchieveInOrderToWin { get; set; }
    int MaxNumberOfWarCardsOnTheTable { get; set; }
    bool ShuffleCardsAfterMainPileIsEmpty { get; set; }
    string ImagePath { get; set; }
  }
}