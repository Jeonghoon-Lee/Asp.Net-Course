using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using MyFriends.Data;
using MyFriends.Models;

namespace MyFriends.Pages
{
  public class ViewModel : PageModel
  {
    private MyFriendContext db;
    public ViewModel(MyFriendContext db) => this.db = db;

    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }

    [BindProperty, Required, Display(Name = "Name")]
    public string Name { get; set; }

    [BindProperty, Required(ErrorMessage = "Age must be between 0 and 150."), Range(0, 150), Display(Name = "Age")]
    public int Age { get; set; }

    // public Friend Friend { get; set; }
    // public async Task OnGetAsync() => Friend = await db.Friends.FindAsync(Id);
    public async Task<IActionResult> OnGetAsync()
    {
      Friend friend = await db.Friends.FindAsync(Id);

      if (friend == null)
      {
        return RedirectToPage("Index");
      }
      Name = friend.Name;
      Age = friend.Age;

      return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
      if (ModelState.IsValid)
      {
        var friend = db.Friends.First(f => f.Id == Id);

        friend.Name = Name;
        friend.Age = Age;

        await db.SaveChangesAsync();

        return RedirectToPage("AddSuccess");
      }
      return Page();
    }
  }
}
