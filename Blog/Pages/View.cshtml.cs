using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Blog.Data;
using Blog.Models;

// JH, 2020-08-20
namespace Blog.Pages
{
  public class ViewModel : PageModel
  {
    private ApplicationDbContext _db;
    public ViewModel(ApplicationDbContext db) => _db = db;

    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }

    public Article Article { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
      Article = await _db.Articles.FindAsync(Id);
      if (Article == null)
      {
        return RedirectToPage("Index");
      }

      // JH, 2020-08-21
      // get auther information explicitly
      await _db.Entry(Article)
        .Reference(article => article.Author)
        .LoadAsync();

      return Page();
    }
  }
}
