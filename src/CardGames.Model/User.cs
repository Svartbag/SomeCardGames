namespace CardGames.Model
{
  public class User
  {
    public int Id { get; set; }
    public string? Name { get; set; }
    public int Level { get; set; }
    public int GamesPlayed { get; set; }
    public int GamesWon { get; set; }
  }
}
