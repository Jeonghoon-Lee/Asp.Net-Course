using MyFriends.Models;
using Microsoft.EntityFrameworkCore;

namespace MyFriends.Data
{
  public class MyFriendContext : DbContext
  {
    public DbSet<Friend> Friends { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseSqlite(@"Data source=MyFriends.db");
    }
  }
}