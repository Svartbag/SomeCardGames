using System.Data.Entity;
using CardGames.Model;

namespace CardGames.DataAccess
{
  public class CardGameOrganizerDbContext : DbContext
  {
    // Todo: no connection to database yet. Obsolete class for now 

    public CardGameOrganizerDbContext() : base("CardGameOrganizerDb")
    {
    }

    public DbSet<User> Users { get; set; }
  }


}