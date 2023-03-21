using CardGames.Core.French.Cards;

namespace CardGames.UI.helpers
{
  public static class CardToImageName
  {
    public static string Convert(Card card)
    {
      // Card scheme
      //      "2h", "3h", "4h", "5h", "6h", "7h", "8h", "9h", "Th", "Jh", "Qh", "Kh", "Ah",
      //      "2d", "3d", "4d", "5d", "6d", "7d", "8d", "9d", "Td", "Jd", "Qd", "Kd", "Ad",
      //      "2s", "3s", "4s", "5s", "6s", "7s", "8s", "9s", "Ts", "Js", "Qs", "Ks", "As",
      //      "2c", "3c", "4c", "5c", "6c", "7c", "8c", "9c", "Tc", "Jc", "Qc", "Kc", "Ac"
      string result = string.Empty;
      switch (card.Value)
      {
        case < 11:
          result = card.Value.ToString();
          break;

        case 11:
          result = "jack";
          break;

        case 12:
          result = "queen";
          break;

        case 13:
          result = "king";
          break;

        case 14:
          result = "ace";
          break;
      }

      result += "_of_";

      switch (card.Suit)
      {
        case Suit.Diamonds:
          result += "diamonds";
          break;

        case Suit.Clubs:
          result += "clubs";
          break;

        case Suit.Spades:
          result += "spades";
          break;

        case Suit.Hearts:
          result += "hearts";
          break;
      }
      if (result.Contains("jack") || result.Contains("queen") || result.Contains("king"))
      {
        result += "2";
      }
      return result;
    }
  }
}
