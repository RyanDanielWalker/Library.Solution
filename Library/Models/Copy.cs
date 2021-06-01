using System.Collections.Generic;

namespace Library.Models
{
  public class Copy
  {
    public int CopyId { get; set; }
    public bool IsCheckedOut { get; set; }
    public string Condition { get; set; }

    public int BookId { get; set; }
    public virtual Book Book { get; set; }
    public virtual ApplicationUser User { get; set; }

    public virtual ICollection<CopyPatron> CopyPatronJoinEntities { get; set; }

    public Copy()
    {
      this.CopyPatronJoinEntities = new HashSet<CopyPatron>();
    }
  }
}