using CardGames.War.Configuration;

namespace CardGames.War
{
  public interface ICardGameWar
  {
    public GameAtributes GameStatus { get; }
    public IList<IWarHand> Players { get; }
    public bool GameStarted { get; }
    public IList<int> PlayersAsUserIds { get; }
    public void Start(IGameConfigurationWar config, IList<int> playersUsersAsUserIds);
    public void Stop();
    public void Reset();
    public void CollectCardsFromTable(int userId);
    public void ResetGameAtributesForAll();
    public bool CanCollectCardsFromTable(int userId);
    public void TryPlaceCardOnTable(int userId);
    public void TryMoveCardsFromSecondaryToMainPile(int userId);

  }
}