using System.Collections.Generic;

namespace Library.Models
{
  public class Book
  {
    public int BookId { get; set; }
    public string Title { get; set; }
    public string Genre { get; set; }

    public virtual ICollection<AuthorBook> AuthorBookJoinEntities { get; }
    public virtual ICollection<Copy> Copies { get; set; }
    // public virtual ApplicationUser User { get; set; }

    public Book()
    {
      this.AuthorBookJoinEntities = new HashSet<AuthorBook>();
      this.Copies = new HashSet<Copy>();
    }
  }
}