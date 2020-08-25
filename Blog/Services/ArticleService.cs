using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using Blog.Models;
using Blog.Data;

namespace Blog.Services
{
  public interface IArticleService
  {
    List<Article> GetArticlesByPage(int currentPage);
    Task<List<Article>> GetArticlesByPageAsync(int currentPage);

    int GetTotalPages();
    int GetArticleCount();
  }

  public class ArticleService : IArticleService
  {
    private readonly ApplicationDbContext _db;

    public ArticleService(ApplicationDbContext db) => _db = db;

    public int CurrentPage { get; set; } = 1;
    public int PageSize { get; set; } = 3;
    public async Task<List<Article>> GetArticlesByPageAsync(int currentPage)
    {
      CurrentPage = currentPage;
      // Count = _db.Articles.Count();

      var articles = await _db.Articles
          .Include(article => article.Author)   // load user data
          .Skip((CurrentPage - 1) * PageSize)
          .Take(PageSize)
          .ToListAsync();

      return articles;
    }

    public List<Article> GetArticlesByPage(int currentPage)
    {
      CurrentPage = currentPage;
      // Count = _db.Articles.Count();

      var articles = _db.Articles
          .Include(article => article.Author)   // load user data
          .Skip((CurrentPage - 1) * PageSize)
          .Take(PageSize)
          .ToList();

      return articles;
    }

    public int GetTotalPages()
    {
      return (int)Math.Ceiling(decimal.Divide(_db.Articles.Count(), PageSize));
    }

    public int GetArticleCount()
    {
      return _db.Articles.Count();
    }
  }
}