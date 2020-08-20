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
  public class AddModel : PageModel
  {
    private MyFriendContext db;
    public AddModel(MyFriendContext db) => this.db = db;

    [BindProperty, Required, MinLength(1), MaxLength(50), Display(Name = "Name")]
    public string Name { get; set; }

    [BindProperty, Required(ErrorMessage = "Age must be between 1 and 150."), Range(1, 150), Display(Name = "Age")]
    public int Age { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
      if (ModelState.IsValid)
      {
        var newFriend = new Friend { Name = Name, Age = Age };

        db.Add(newFriend);
        await db.SaveChangesAsync();

        return RedirectToPage("AddSuccess");
      }
      return Page();
    }
  }
}
