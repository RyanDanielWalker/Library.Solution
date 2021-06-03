using System;

namespace Library.Models
{
  public class CopyPatron
  {
    public int CopyPatronId { get; set; }
    public int CopyId { get; set; }
    public string PatronId { get; set; }
    public DateTime CheckoutDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime ReturnedDate { get; set; }
    public bool HasBeenReturned { get; set; }

    public virtual Copy Copy { get; set; }
    public virtual ApplicationUser Patron { get; set; }
  }
}