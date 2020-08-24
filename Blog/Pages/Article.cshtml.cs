using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

using Blog.Data;
using Blog.Models;

// JH, 2020-08-20
namespace Blog.Pages
{
  public class ArticleModel : PageModel
  {
    private ApplicationDbContext _db;
    public ArticleModel(ApplicationDbContext db) => _db = db;

    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }

    [BindProperty, Required, MinLength(2), Display(Name = "Comment")]
    public string CommentBody { get; set; }

    public Article Article { get; set; }
    public List<Comment> Comments { get; set; } = new List<Comment>();

    private async void LoadAuthor()
    {
      // JH, 2020-08-21
      // Get auther information explicitly
      await _db.Entry(Article)
        .Reference(article => article.Author)
        .LoadAsync();
    }

    private async void LoadComments()
    {
      // Loading related data for Comments
      Comments = await _db.Comments
          .Where(comment => comment.Article == Article)
          .Include(comment => comment.Author)
          .ToListAsync();

      // Sort by time, recent first
      Comments.Sort((c1, c2) => c2.TimeStamp.CompareTo(c1.TimeStamp));
    }

    public async Task<IActionResult> OnGetAsync()
    {
      Article = await _db.Articles.FindAsync(Id);
      if (Article == null)
      {
        return RedirectToPage("Index");
      }
      LoadAuthor();
      LoadComments();

      return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
      Article = await _db.Articles.FindAsync(Id);
      if (Article == null)
      {
        return RedirectToPage("Index");
      }
      LoadAuthor();

      // JH, 2020-08-22
      // checking user authorizaion
      // Razor page doesn't support hanlder level authorization only support page level.
      if (!User.Identity.IsAuthenticated)
      {
        // redirct to login page
        return LocalRedirect("/Identity/Account/Login?ReturnUrl=/Article/" + Article.Id);
      }

      // JH, 2020-08-22
      // Add article
      if (ModelState.IsValid)
      {
        var newComment = new Comment
        {
          Body = CommentBody,
          Author = _db.Users.FirstOrDefault(user => user.UserName == User.Identity.Name),
          Article = Article
        };

        newComment.TimeStamp = DateTime.Now;
        _db.Add(newComment);
        await _db.SaveChangesAsync();

        // CommentBody = "";
        return RedirectToPage("Article", new { id = Article.Id });
      }

      return Page();
    }
  }
}
