using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Library.Models
{
  public class ApplicationUser : IdentityUser
  {
    public ApplicationUser()
    {
      this.CopyPatronJoinEntities = new HashSet<CopyPatron>();
    }
    // public int PatronId { get; set; }
    // public string Name { get; set; }
    public virtual ICollection<CopyPatron> CopyPatronJoinEntities { get; set; }
  }
}