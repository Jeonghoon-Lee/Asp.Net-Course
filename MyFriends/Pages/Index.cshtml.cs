using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using MyFriends.Data;
using MyFriends.Models;

namespace MyFriends.Pages
{
  public class IndexModel : PageModel
  {
    private readonly MyFriendContext db;
    public IndexModel(MyFriendContext db) => this.db = db;

    public List<Friend> Friends { get; set; } = new List<Friend>();

    public async void OnGetAsync()
    {
      Friends = await db.Friends.ToListAsync();
    }

    public async Task<IActionResult> OnGetDelete(int id)
    {
      Friend friend = await db.Friends.FindAsync(id);

      if (friend != null)
      {
        db.Remove(friend);
        await db.SaveChangesAsync();
      }

      return RedirectToPage("Index");
    }
  }
}
