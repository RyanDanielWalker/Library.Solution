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

namespace Library.Models
{
  [Authorize]
  public class CopiesController : Controller
  {
    private readonly LibraryContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    public CopiesController(UserManager<ApplicationUser> userManager, LibraryContext db)
    {
      _userManager = userManager;
      _db = db;
    }
    [AllowAnonymous]
    public async Task<ActionResult> Index()
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      var userCopies = _db.Copies.Where(entry => entry.User.Id == currentUser.Id).ToList();
      return View(userCopies);
    }

    public ActionResult Create()
    {
      ViewBag.BookId = new SelectList(_db.Books, "BookId", "Title");
      return View();
    }

    [HttpPost]
    public ActionResult Create(Copy copy)
    {
      _db.Copies.Add(copy);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
    [AllowAnonymous]
    public ActionResult Details(int id)
    {
      var thisCopy = _db.Copies
          .Include(copy => copy.Book)
          .Include(copy => copy.CopyPatronJoinEntities)
          .ThenInclude(join => join.Patron)
          .FirstOrDefault(copy => copy.CopyId == id);
      return View(thisCopy);
    }
    public ActionResult Edit(int id)
    {
      var thisCopy = _db.Copies
        .Include(copy => copy.Book)
        .FirstOrDefault(copy => copy.CopyId == id);
      ViewBag.BookId = new SelectList(_db.Books, "BookId", "Title");
      return View(thisCopy);
    }
    [HttpPost]
    public ActionResult Edit(Copy copy)
    {
      _db.Entry(copy).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult AddUser(int id)
    {
      var thisCopy = _db.Copies
        .Include(copy => copy.Book)
        .FirstOrDefault(copy => copy.CopyId == id);
      ViewBag.UserId = new SelectList(_userManager.Users, "Id", "UserName");
      return View(thisCopy);
    }

    [HttpPost]
    public ActionResult AddUser(Copy copy, int UserId)
    {
      if (UserId != 0)
      {
        _db.CopyPatron.Add(new CopyPatron() { CopyId = copy.CopyId, PatronId = UserId });
      }
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
      var thisCopy = _db.Copies
        .Include(copy => copy.Book)
        .FirstOrDefault(copy => copy.CopyId == id);
      return View(thisCopy);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      var thisCopy = _db.Copies
        .Include(copy => copy.Book)
        .FirstOrDefault(copy => copy.CopyId == id);
      _db.Copies.Remove(thisCopy);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
    [HttpPost]
    public ActionResult DeleteUser(int joinId)
    {
      var joinEntry = _db.CopyPatron.FirstOrDefault(entry => entry.CopyPatronId == joinId);
      _db.CopyPatron.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}