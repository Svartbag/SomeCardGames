using CardGames.Core.French.Cards;
using CardGames.War.Configuration;

namespace CardGames.War
{
  public interface IWarHand
  {
    public GameAtributesUserSpecific GameAtributes { get; set; }
    public IGameConfigurationWar config { get; set; }
    public int UserId { get; }
    public IList<Card> CardsMainPile { get; }
    public IList<Card> CardsSecondaryPile { get; }
    public Card? CardOnTheTable { get; }
    public IList<Card> WarCardsOnTheTable { get; }
    public IList<Card> GetAllCardsOnTheTable();
    public void ReceiveCards(IList<Card> cards);
    public void TransferCardsToHand(IList<Card> cards);
    public IList<Card> GetAllCardsOnHand { get; }
    public void TransferCardToTable();
    public void TransferCardsToWarTable(int maxNumberOfWarCardsOnTheTable);
    public Card SelectCardFromWarTable(int cardNumber);
    public void TransferCardsFromSecondaryToMainPile();
    public void removeAllCardsOnTheTable();
  }
}