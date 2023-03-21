using CardGames.Core.French.Cards;
using CardGames.War.Configuration;

namespace CardGames.War
{
  public class WarHand : IWarHand
  {
    private List<Card> _cardsMainPile = new();
    private List<Card> _cardsSecondaryPile = new();
    private List<Card> _warCardsOnTheTable = new();

    public WarHand(IGameConfigurationWar configuration, int userId)
    {
      config = configuration;
      UserId = userId;
      GameAtributes = new GameAtributesUserSpecific();
    }
    public GameAtributesUserSpecific GameAtributes { get; set; }
    public IGameConfigurationWar config { get; set; }
    public int UserId { get; }
    public IList<Card> CardsMainPile
    {
      get => _cardsMainPile;
      private set => _cardsMainPile = value.ToList();
    }
    public IList<Card> CardsSecondaryPile
    {
      get => _cardsSecondaryPile;
      private set => _cardsSecondaryPile = value.ToList();
    }
    public Card? CardOnTheTable { get; private set; }
    public IList<Card> WarCardsOnTheTable
    {
      get => _warCardsOnTheTable;
      private set => _warCardsOnTheTable = value.ToList();
    }
    public IList<Card> GetAllCardsOnTheTable()
    {
      List<Card> result = new();
      if (CardOnTheTable is not null)
      {
        result.Add(CardOnTheTable);
      }
      result.AddRange(WarCardsOnTheTable);
      return result;
    }
    public void ReceiveCards(IList<Card> cards)
    {
      _cardsMainPile.AddRange(cards);
    }
    public void TransferCardsToHand(IList<Card> cards)
    {
      _cardsSecondaryPile.AddRange(cards);
    }
    public IList<Card> GetAllCardsOnHand
    {
      get
      {
        List<Card> allCardsOnHand = new();
        allCardsOnHand.AddRange(CardsMainPile);
        allCardsOnHand.AddRange(CardsSecondaryPile);
        return allCardsOnHand;
      }
    }
    public void TransferCardToTable()
    {
      if (_cardsMainPile.Count == 0 || CardOnTheTable is not null)
      {
        return;
      }
      int cardOnTop = _cardsMainPile.Count - 1;
      Card cardToBeTransferred = _cardsMainPile[cardOnTop];

      CardOnTheTable = cardToBeTransferred;
      _cardsMainPile.RemoveAt(cardOnTop);
    }
    private void TransferOneCardToWarTable(int maxNumberOfWarCardsOnTheTable)
    {
      if (_cardsMainPile.Count == 0 || WarCardsOnTheTable.Count >= maxNumberOfWarCardsOnTheTable)
      {
        return;
      }
      int cardOnTop = _cardsMainPile.Count - 1;
      WarCardsOnTheTable.Add(_cardsMainPile[cardOnTop]);
      _cardsMainPile.RemoveAt(cardOnTop);
    }
    public void TransferCardsToWarTable(int maxNumberOfWarCardsOnTheTable)
    {
      int iteratorStart = WarCardsOnTheTable.Count() - 1;
      for (int i = iteratorStart; i < maxNumberOfWarCardsOnTheTable; i++)
      {
        TransferOneCardToWarTable(maxNumberOfWarCardsOnTheTable);
      }
    }
    public Card SelectCardFromWarTable(int cardNumber)
    {
      return cardNumber >= 0 && cardNumber < WarCardsOnTheTable.Count()
        ? WarCardsOnTheTable[cardNumber]
        : throw new ArgumentException($"Value must be less than {WarCardsOnTheTable.Count()}. AND it cannot be negative");
    }
    public void TransferCardsFromSecondaryToMainPile()
    {
      List<Card> secondaryPileWhereValuesAreSortedRandomly = _cardsSecondaryPile.OrderBy(x => Guid.NewGuid().ToString()).ToList();
      _cardsMainPile.AddRange(secondaryPileWhereValuesAreSortedRandomly);
      _cardsSecondaryPile = new List<Card>();
    }
    public void removeAllCardsOnTheTable()
    {
      CardOnTheTable = null;
      WarCardsOnTheTable = new List<Card>();
    }
  }
}
