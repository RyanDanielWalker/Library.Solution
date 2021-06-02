using System.Collections.Generic;

namespace Library.Models
{
  public class Author
  {
    public Author()
    {
      this.AuthorBookJoinEntities = new HashSet<AuthorBook>();
    }

    public int AuthorId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public virtual ICollection<AuthorBook> AuthorBookJoinEntities { get; set; }

  }
}