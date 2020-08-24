using System;
using Microsoft.AspNetCore.Identity;

namespace Blog.Models
{
  public class Comment
  {
    public int Id { get; set; }
    public string Body { get; set; }
    public DateTime TimeStamp { get; set; }
    public Article Article { get; set; }
    public IdentityUser Author { get; set; }
  }
}