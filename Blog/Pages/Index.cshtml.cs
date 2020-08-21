using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

using Blog.Models;
using Blog.Data;

namespace Blog.Pages
{
  public class IndexModel : PageModel
  {
    private readonly ILogger<IndexModel> _logger;
    private readonly ApplicationDbContext _db;

    public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext db)
    {
      _logger = logger;
      _db = db;
    }

    public List<Article> Articles { get; set; } = new List<Article>();

    public async void OnGetAsync()
    {
      // JH, 2020-08-21
      // added Include to get related data
      Articles = await _db.Articles
        .Include(article => article.Author)   // load user data
        .ToListAsync();
    }

    public async Task<IActionResult> OnGetDeleteAsync(int id)
    {
      Article article = await _db.Articles.FindAsync(id);

      if (article != null)
      {
        _db.Remove(article);
        await _db.SaveChangesAsync();
      }

      return RedirectToPage("Index");
    }
  }
}