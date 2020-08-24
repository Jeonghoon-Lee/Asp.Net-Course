using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;

using Blog.Models;
using Blog.Data;

namespace Blog.Pages
{
  public class IndexModel : PageModel
  {
    private readonly ILogger<IndexModel> _logger;
    private readonly ApplicationDbContext _db;
    private readonly IWebHostEnvironment _hostingEnvironment;

    public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext db, IWebHostEnvironment hostingEnvironment)
    {
      _logger = logger;
      _db = db;
      _hostingEnvironment = hostingEnvironment;
    }

    public List<Article> Articles { get; set; } = new List<Article>();

    // for pagination
    [BindProperty(SupportsGet = true)]
    public int CurrentPage { get; set; } = 1;
    public int Count { get; set; }
    public int PageSize { get; set; } = 3;

    public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count, PageSize));

    public async void OnGetAsync()
    {
      // JH, 2020-08-21
      // added Include to get related data
      // Articles = await _db.Articles
      //   .Include(article => article.Author)   // load user data
      //   .ToListAsync();

      // JH, 2020-08-23
      // Modified for pagination
      Count = _db.Articles.Count();

      Articles = await _db.Articles
          .Include(article => article.Author)   // load user data
          .Skip((CurrentPage - 1) * PageSize)
          .Take(PageSize)
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