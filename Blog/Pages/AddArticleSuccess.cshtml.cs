using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

using Blog.Data;
using Blog.Models;

// JH, 2020-08-20
namespace Blog.Pages
{
  [Authorize]
  public class AddArticleSuccessModel : PageModel
  {
    private ApplicationDbContext _db;
    public AddArticleSuccessModel(ApplicationDbContext db) => _db = db;

    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }

    public string ArticleTitle;

    public async Task<IActionResult> OnGetAsync()
    {
      var article = await _db.Articles.FindAsync(Id);
      if (article == null)
      {
        return RedirectToPage("Index");
      }
      // set title.
      ArticleTitle = article.Title;
      return Page();
    }
  }
}
