using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Library.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;
using System;

namespace Library.Models
{
  [Authorize]
  public class BooksController : Controller
  {
    private readonly LibraryContext _db;
    // private readonly UserManager<ApplicationUser> _userManager;
    public BooksController(UserManager<ApplicationUser> userManager, LibraryContext db)
    {
      // _userManager = userManager;
      _db = db;
    }
    // [AllowAnonymous]
    // public ActionResult Index()
    // {
    //   return View(_db.Books.ToList());
    // }

    public async Task<ActionResult> Index(string searchString)
    {
      var books = from b in _db.Books
                  select b;
      if (!String.IsNullOrEmpty(searchString))
      {
        books = books.Where(s => s.Title.Contains(searchString));
      }
      return View(await books.ToListAsync());
    }

    public ActionResult Create()
    {
      ViewBag.AuthorId = new SelectList(_db.Authors, "AuthorId", "FirstName");
      return View();
    }

    [HttpPost]
    public ActionResult Create(Book book, int authorId)
    {
      // var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      // var currentUser = await _userManager.FindByIdAsync(userId);
      // bool authorExists = _db.Authors
      //   .Any(entry => entry.FirstName == author.FirstName && entry.LastName == author.LastName);
      // if (!authorExists)
      // {
      //   _db.Authors.Add(author);
      //   Author selectedAuthor = author;
      // }
      // else
      // {
      //   Author selectedAuthor = _db.Authors
      //     .FirstOrDefault(entry => entry.FirstName == author.FirstName && entry.LastName == author.LastName);
      // }
      // book.User = currentUser;
      _db.Books.Add(book);
      _db.SaveChanges();
      if (authorId != 0)
      {
        _db.AuthorBook.Add(new AuthorBook() { AuthorId = authorId, BookId = book.BookId });
        _db.SaveChanges();
      }
      return RedirectToAction("Index");
    }
    [AllowAnonymous]
    public ActionResult Details(int id)
    {
      var thisBook = _db.Books
          .Include(book => book.AuthorBookJoinEntities)
          .ThenInclude(join => join.Author)
          .FirstOrDefault(book => book.BookId == id);

      var bookCopies = thisBook.Copies;
      var numberAvailable = 0;
      foreach (Copy copy in bookCopies)
      {
        if (!copy.IsCheckedOut)
        {
          numberAvailable++;
        }
      }
      if (numberAvailable == 0)
      {
        ViewBag.AreCopiesAvailable = false;
      }
      else
      {
        ViewBag.AreCopiesAvailable = true;
      }

      ViewBag.UserId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

      return View(thisBook);
    }
    public ActionResult Edit(int id)
    {
      var thisBook = _db.Books.FirstOrDefault(book => book.BookId == id);
      ViewBag.AuthorId = new SelectList(_db.Authors, "AuthorId", "FirstName");
      return View(thisBook);
    }
    [HttpPost]
    public ActionResult Edit(Book book, int AuthorId)
    {
      if (AuthorId != 0)
      {
        _db.AuthorBook.Add(new AuthorBook() { AuthorId = AuthorId, BookId = book.BookId });
      }
      _db.Entry(book).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult AddAuthor(int id)
    {
      var thisBook = _db.Books.FirstOrDefault(book => book.BookId == id);
      ViewBag.AuthorId = new SelectList(_db.Authors, "AuthorId", "FirstName");
      return View(thisBook);
    }

    [HttpPost]
    public ActionResult AddAuthor(Book book, int AuthorId)
    {
      if (AuthorId != 0)
      {
        _db.AuthorBook.Add(new AuthorBook() { AuthorId = AuthorId, BookId = book.BookId });
      }
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
      var thisBook = _db.Books.FirstOrDefault(book => book.BookId == id);
      return View(thisBook);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      var thisBook = _db.Books.FirstOrDefault(book => book.BookId == id);
      _db.Books.Remove(thisBook);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
    [HttpPost]
    public ActionResult DeleteAuthor(int joinId)
    {
      var joinEntry = _db.AuthorBook.FirstOrDefault(entry => entry.AuthorBookId == joinId);
      _db.AuthorBook.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}

