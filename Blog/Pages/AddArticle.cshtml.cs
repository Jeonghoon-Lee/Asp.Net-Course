using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

using Blog.Data;
using Blog.Models;

namespace Blog.Pages
{
  [Authorize]
  public class AddArticleModel : PageModel
  {
    private ApplicationDbContext db;
    public AddArticleModel(ApplicationDbContext db) => this.db = db;

    [BindProperty, Required, MinLength(1), Display(Name = "Title")]
    public string Title { get; set; }

    [BindProperty, Required, MinLength(1), Display(Name = "Content")]
    public string Body { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
      if (ModelState.IsValid)
      {
        var newArticle = new Article { Title = Title, Body = Body, Author = db.Users.FirstOrDefault(user => user.UserName == User.Identity.Name) };

        newArticle.TimeStamp = DateTime.Now;
        db.Add(newArticle);
        await db.SaveChangesAsync();

        return RedirectToPage("AddArticleSuccess", new { id = newArticle.Id });
      }
      return Page();
    }
  }
}