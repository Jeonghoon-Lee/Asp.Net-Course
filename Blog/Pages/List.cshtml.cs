using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

using Blog.Services;
using Blog.Models;

namespace Blog.Pages
{
  public class ListModel : PageModel
  {
    // private IArticleService _articleService;
    private IArticleService _articleService;
    public ListModel(IArticleService articleService) => _articleService = articleService;

    public List<Article> Articles { get; set; }
    public int CurrentPage { get; set; } = 1;
    public int TotalPages { get; set; }
    public int Count { get; set; }

    public void OnGet()
    {
      TotalPages = _articleService.GetTotalPages();
      Count = _articleService.GetArticleCount();
    }

    // public PartialViewResult OnGetArticlePartial()
    // {
    //   Articles = _articleService.GetArticlesByPage(1);
    //   return Partial("_ArticlePartial", Articles);
    // }
    public PartialViewResult OnGetArticlePartial(int currentPage)
    {
      CurrentPage = currentPage;
      Articles = _articleService.GetArticlesByPage(currentPage);
      return new PartialViewResult
      {
        ViewName = "_ArticlePartial",
        ViewData = new ViewDataDictionary<List<Article>>(ViewData, Articles)
      };
    }
  }
}
