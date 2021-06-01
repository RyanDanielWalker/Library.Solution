using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Library.Models
{
  public class LibraryContext : IdentityDbContext<ApplicationUser>
  {
    public virtual DbSet<Author> Authors { get; set; }
    public virtual DbSet<Book> Books { get; set; }
    public DbSet<AuthorBook> AuthorBook { get; set; }
    // public virtual DbSet<ApplicationUser> ApplicationUser { get; set; }
    public virtual DbSet<Copy> Copies { get; set; }
    public DbSet<CopyPatron> CopyPatron { get; set; }


    public LibraryContext(DbContextOptions options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseLazyLoadingProxies();
    }
  }
}