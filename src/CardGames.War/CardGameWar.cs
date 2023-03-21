using CardGames.Core.Deck;
using CardGames.Core.French.Cards;
using CardGames.Core.French.Dealers;
using CardGames.Core.French.Decks;
using CardGames.War.Configuration;
using System.Collections.ObjectModel;

namespace CardGames.War
{
  public class CardGameWar : ICardGameWar
  {
    private IGameConfigurationWar _config;
    private IDeck<Card> _startupDeck;
    private int _deckSize;
    private FrenchDeckDealer _dealer;
    private IList<Card> _cardsOnTableWhichCanBeCollected;

    public CardGameWar()
    {
      GameStarted = false;
      GameStatus = new GameAtributes();
      _config = new GameConfigurationWar();
      PlayersAsUserIds = new List<int>();
      _cardsOnTableWhichCanBeCollected = new List<Card>();
      _startupDeck = new FullFrenchDeck();
      _deckSize = _startupDeck.NumberOfCardsLeft();
      _dealer = new FrenchDeckDealer((FrenchDeck)_startupDeck);
      Players = new List<IWarHand>();
      Players2 = new List<IWarHand>();
    }

    public GameAtributes GameStatus { get; private set; }

    public IList<IWarHand> Players { get; private set; }

    public IList<IWarHand> Players2 { get; private set; }

    public bool GameStarted { get; private set; }

    public IList<int> PlayersAsUserIds { get; private set; }

    public void Start(IGameConfigurationWar config, IList<int> playersUsersAsUserIds)
    {
      int amountOfUsersAllowed = 2; // maybe more users allowed in future, but only 2 for now

      GameStarted = true;
      _config = config;
      PlayersAsUserIds = playersUsersAsUserIds;
      _cardsOnTableWhichCanBeCollected = new ObservableCollection<Card>();
      _startupDeck = new FullFrenchDeck();
      _deckSize = _startupDeck.NumberOfCardsLeft();
      _dealer = new FrenchDeckDealer((FrenchDeck)_startupDeck);
      _dealer.Shuffle();
      Players = new ObservableCollection<IWarHand>();
      GameStatus = new GameAtributes();

      _ = playersUsersAsUserIds.Distinct().ToList();

      if (playersUsersAsUserIds is null || playersUsersAsUserIds.Count() != amountOfUsersAllowed)
      {
        throw new ArgumentException($"Input user id's are not as expected.");
      }

      for (int i = 0; i < playersUsersAsUserIds.Count; i++)
      {
        Players.Add(new WarHand(_config, PlayersAsUserIds[i]));
      }

      int dealingRounds = (_deckSize / _config.CardsFromDealerPrRound / Players.Count()) + 1;
      for (int i = 0; i < dealingRounds; i++)
      {
        for (int iterator = 0; iterator < playersUsersAsUserIds.Count(); iterator++)
        {
          if (_startupDeck.NumberOfCardsLeft() > 0)
          {
            Players[iterator].ReceiveCards(_dealer.DealCards(_config.CardsFromDealerPrRound).ToList());
          }
        }
      }
    }

    public void Stop()
    {
      GameStarted = false;
    }

    public void Reset()
    {
      PlayersAsUserIds = new List<int>();
      Players = new ObservableCollection<IWarHand>();
      GameStatus = new GameAtributes();
    }

    public void CollectCardsFromTable(int userId)
    {
      int userId1 = Players.First(x => x.UserId == userId).UserId;
      int userId2 = Players.First(x => x.UserId != userId1).UserId;
      evaluatateGameScenario(userId1, userId2);
      bool userCanCollectCards = Players.First(x => x.UserId == userId1) is not null && Players.First(x => x.UserId == userId1).GameAtributes.CanCollectCardsFromTable;
      if (userCanCollectCards)
      {
        Players.First(x => x.UserId == userId1).TransferCardsToHand(_cardsOnTableWhichCanBeCollected);

        GameStatus.CardsCollectedFromTable = true;

        // Reset some more stuff.
        Players.ToList().ForEach(x => x.removeAllCardsOnTheTable());
        _cardsOnTableWhichCanBeCollected = new List<Card>();
        GameStatus.WarIsOn = false;

        if (PlayerHasWon(userId1))
        {
          OnPlayerHasWon(userId1);
        }
      }
      if (GameStatus.WarIsOn && GameStatus.WarCardsCanBeMovedToWinningCardPile)
      {
        Players.ToList().ForEach(x => x.removeAllCardsOnTheTable());
      }
      if (userCanCollectCards ||
        (GameStatus.WarIsOn && GameStatus.WarCardsCanBeMovedToWinningCardPile))
      {
        GameStatus.SufficientWarCardsFromUsersOnTheTable = false;
        Players.First(x => x.UserId == userId1).GameAtributes.SufficientAmountOfWarAreCardsAreOnTheTable = false;
        Players.First(x => x.UserId == userId2).GameAtributes.SufficientAmountOfWarAreCardsAreOnTheTable = false;
      }
    }

    public void ResetGameAtributesForAll()
    {
      Players.ToList().ForEach(x => x.GameAtributes = new GameAtributesUserSpecific());
      GameStatus = new GameAtributes();
    }

    public bool CanCollectCardsFromTable(int userId)
    {
      return Players.First(x => x.UserId == userId) is not null && Players.First(x => x.UserId == userId).GameAtributes.CanCollectCardsFromTable;
    }

    public void TryPlaceCardOnTable(int userId)
    {
      int userId1 = Players.First(x => x.UserId == userId).UserId;
      int userId2 = Players.First(x => x.UserId != userId1).UserId;
      evaluatateGameScenario(userId1, userId2);
      if (GameStatus.WarIsOn)
      {
        Players.First(x => x.UserId == userId1).TransferCardsToWarTable(amountOfWarCardsToPlaceOnTablePrUser());
      }
      else
      {
        Players.First(x => x.UserId == userId1).TransferCardToTable();
      }
      evaluatateGameScenario(userId1, userId2);
      evaluatateGameScenario(userId1, userId2);
    }

    public void TryMoveCardsFromSecondaryToMainPile(int userId)
    {
      if (Players.First(x => x.UserId == userId).CardsMainPile.Count == 0)
      {
        Players.First(x => x.UserId == userId).TransferCardsFromSecondaryToMainPile();
      }
    }

    private bool PlayerHasWon(int userId)
    {
      return Players.First(x => x.UserId == userId).GetAllCardsOnHand.Count >= _config.NumberOfCardsToAchieveInOrderToWin;
    }

    private void OnPlayerHasWon(int userId)
    {
      Players.First(x => x.UserId == userId).GameAtributes.HasWonTheGame = true;
      _ = Players.Where(x => x.UserId != userId).All(x => x.GameAtributes.HasWonTheGame = false);
    }

    private void evaluatateGameScenario(int userId1, int userId2)
    {
      if (GameStatus.WarIsOn)
      {
        if (GameStatus.SufficientWarCardsFromUsersOnTheTable)
        {
          evaluateWarCards(userId1, userId2);
          return;
        }
        _ = Players.Where(x => x.WarCardsOnTheTable.Count() == amountOfWarCardsToPlaceOnTablePrUser())
                .All(x => x.GameAtributes.SufficientAmountOfWarAreCardsAreOnTheTable = true);

        GameStatus.SufficientWarCardsFromUsersOnTheTable =
             Players.First(x => x.UserId == userId1).GameAtributes.SufficientAmountOfWarAreCardsAreOnTheTable
          && Players.First(x => x.UserId == userId2).GameAtributes.SufficientAmountOfWarAreCardsAreOnTheTable;
        return;
      }
      evaluateTableCards(userId1, userId2);
    }

    private void evaluateTableCards(int userId1, int userId2)
    {
      bool tableCardsAreEqual = cardsAreEqual(Players.First(x => x.UserId == userId1).CardOnTheTable, Players.First(x => x.UserId == userId2).CardOnTheTable);
      if (tableCardsAreEqual)
      {
        GameStatus.WarIsOn = true;
        return;
      }
      if (Players.First(x => x.UserId == userId1).CardOnTheTable is null || Players.First(x => x.UserId == userId2).CardOnTheTable is null)
      {
        return;
      }
      if (Players.First(x => x.UserId == userId1).CardOnTheTable.Value > Players.First(x => x.UserId == userId2).CardOnTheTable.Value)
      {
        Players.First(x => x.UserId == userId1).GameAtributes.CanCollectCardsFromTable = true;
      }
      else
      {
        Players.First(x => x.UserId == userId2).GameAtributes.CanCollectCardsFromTable = true;
      }
      moveCardsOnTableToWinnerCardPile();
    }

    private void evaluateWarCards(int userId1, int userId2)
    {
      if (GameStatus.WarCardsCanBeMovedToWinningCardPile)
      {
        return;
      }
      if (Players.First(x => x.UserId == userId1).WarCardsOnTheTable.Count() == 0) // this situation only occurs if one of the players do not have any cards on the hand
      {
        if (Players.First(x => x.UserId == userId1).GetAllCardsOnHand.Count() >= Players.First(x => x.UserId == userId1).GetAllCardsOnHand.Count())
        {
          Players.First(x => x.UserId == userId1).GameAtributes.CanCollectCardsFromTable = true;
        }
        else
        {
          Players.First(x => x.UserId == userId2).GameAtributes.CanCollectCardsFromTable = true;
        }
        GameStatus.WarIsOn = false;
        moveCardsOnTableToWinnerCardPile();
        return;
      }

      bool warCardsAreEqual = cardsAreEqual(Players.First(x => x.UserId == userId1).WarCardsOnTheTable.Last(), Players.First(x => x.UserId == userId2).WarCardsOnTheTable.Last());
      if (warCardsAreEqual)
      {
        GameStatus.SufficientWarCardsFromUsersOnTheTable = false;
        moveCardsOnTableToWinnerCardPile();
        GameStatus.WarCardsCanBeMovedToWinningCardPile = true;
        return;
      }

      if (Players.First(x => x.UserId == userId1).WarCardsOnTheTable.Last().Value > Players.First(x => x.UserId == userId2).WarCardsOnTheTable.Last().Value)
      {
        Players.First(x => x.UserId == userId1).GameAtributes.CanCollectCardsFromTable = true;
      }
      else
      {
        Players.First(x => x.UserId == userId2).GameAtributes.CanCollectCardsFromTable = true;
      }
      GameStatus.WarIsOn = false;
      moveCardsOnTableToWinnerCardPile();
    }

    private void moveCardsOnTableToWinnerCardPile()
    {
      List<Card> tmpNewCards = new();
      List<Card> tmpCardsOnTableWhichCanBeCollected = new();
      tmpCardsOnTableWhichCanBeCollected = _cardsOnTableWhichCanBeCollected.ToList();
      Players.ToList().ForEach(x => tmpNewCards.AddRange(x.GetAllCardsOnTheTable()));
      tmpCardsOnTableWhichCanBeCollected.AddRange(tmpNewCards);
      _cardsOnTableWhichCanBeCollected = tmpCardsOnTableWhichCanBeCollected.Distinct().ToList();
    }

    private int amountOfWarCardsToPlaceOnTablePrUser()
    {
      int maxNumberOfWarCardsOnTheTable = Players[0].config.MaxNumberOfWarCardsOnTheTable;

      foreach (IWarHand user in Players)
      {
        if (user.GetAllCardsOnHand.Count + user.WarCardsOnTheTable.Count < maxNumberOfWarCardsOnTheTable)
        {
          maxNumberOfWarCardsOnTheTable = user.GetAllCardsOnHand.Count() + user.WarCardsOnTheTable.Count;
        }
      }

      return maxNumberOfWarCardsOnTheTable;
    }

    private bool cardsAreEqual(Card card1, Card card2)
    {
      if (card1 is null || card2 is null)
      {
        return false;
      }
      int threshold = Math.Abs(card1.Value - card2.Value);
      return threshold <= _config.MaxDiffBetweenCardsToHaveWar;
    }

    #region methods for test

    // This Start method is for testing
    /*
    public void Start(IGameConfigurationWar config, IList<int> playersUsersAsUserIds)
    {
      Reset();
      GameStarted = true;
      _config = config;
      PlayersAsUserIds = playersUsersAsUserIds;
      _cardsOnTableWhichCanBeCollected = new ObservableCollection<Card>();
      _startupDeck = new FullFrenchDeck();
      _deckSize = _startupDeck.NumberOfCardsLeft();
      _dealer = new FrenchDeckDealer((FrenchDeck)_startupDeck);
      //_dealer.Shuffle();

      int amoutOfUsers = 2;
      playersUsersAsUserIds.Distinct().ToList();

      if (playersUsersAsUserIds is null || playersUsersAsUserIds.Count() != amoutOfUsers)
      {
        throw new ArgumentException($"input user id's are not as expected.");
      }
      for (int i = 0; i < amoutOfUsers; i++)
      {
        Players.Add(new WarHand(_config, PlayersAsUserIds[i]));
      }
      var hand1 = new List<Card>()
    {
      //new Card(Suit.Clubs, Symbol.Seven),
      //new Card(Suit.Clubs, Symbol.Three),
      //new Card(Suit.Clubs, Symbol.King),
      //new Card(Suit.Hearts, Symbol.Queen),
      new Card(Suit.Hearts, Symbol.Jack),

      new Card(Suit.Hearts, Symbol.Ten),
      new Card(Suit.Clubs, Symbol.Deuce),
      new Card(Suit.Clubs, Symbol.Four),
      new Card(Suit.Clubs, Symbol.Queen),
      new Card(Suit.Clubs, Symbol.Six),
      new Card(Suit.Clubs, Symbol.Seven),
      new Card(Suit.Clubs, Symbol.Eight),
      new Card(Suit.Clubs, Symbol.Nine),
      new Card(Suit.Clubs, Symbol.Ten),
      new Card(Suit.Clubs, Symbol.Jack)
  };

      var hand2 = new List<Card>()
    {
      //new Card(Suit.Hearts, Symbol.Six),
      //new Card(Suit.Hearts, Symbol.Four),
      //new Card(Suit.Hearts, Symbol.King),
      //new Card(Suit.Hearts, Symbol.Nine),
      new Card(Suit.Hearts, Symbol.Ace),

      new Card(Suit.Hearts, Symbol.Nine),
      new Card(Suit.Diamonds, Symbol.Ace),
      new Card(Suit.Diamonds, Symbol.Deuce),
      new Card(Suit.Diamonds, Symbol.Five),
      new Card(Suit.Diamonds, Symbol.King),
      new Card(Suit.Diamonds, Symbol.Seven),
      new Card(Suit.Diamonds, Symbol.Eight),
      new Card(Suit.Diamonds, Symbol.Six),
      new Card(Suit.Diamonds, Symbol.Ten),
      new Card(Suit.Diamonds, Symbol.Jack)
  };
      

      Players[0].ReceiveCards(hand1);
      Players[1].ReceiveCards(hand2);
    }
    */
    #endregion
  }
}


