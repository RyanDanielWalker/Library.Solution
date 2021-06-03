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
  public class CopiesController : Controller
  {
    private readonly LibraryContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    public CopiesController(UserManager<ApplicationUser> userManager, LibraryContext db)
    {
      _userManager = userManager;
      _db = db;
    }
    // [AllowAnonymous]
    public async Task<ActionResult> Index()
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      var userCopies = _db.Copies
        .Include(entry => entry.CopyPatronJoinEntities)
        .Where(entry => entry.User.Id == currentUser.Id)
        .ToList();
      return View(userCopies);
      // var allCopies = _db.Copies.Include(copy => copy.User).ToList();
      // return View(allCopies);
    }

    public ActionResult Create()
    {
      ViewBag.BookId = new SelectList(_db.Books, "BookId", "Title");
      ViewBag.UserId = new SelectList(_userManager.Users, "Id", "UserName");
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Create(Copy copy, string UserId)
    {
      var selectedUser = await _userManager.FindByIdAsync(UserId);
      copy.User = selectedUser;
      _db.Copies.Add(copy);
      _db.SaveChanges();
      if (UserId != null)
      {
        _db.CopyPatron.Add(new CopyPatron() { CopyId = copy.CopyId, PatronId = UserId });
        _db.SaveChanges();
      }
      return RedirectToAction("Index");
    }
    [AllowAnonymous]
    public ActionResult Details(int id)
    {
      var thisCopy = _db.Copies
          .Include(copy => copy.CopyPatronJoinEntities)
          .ThenInclude(join => join.Patron)
          .Include(copy => copy.Book)
          .FirstOrDefault(copy => copy.CopyId == id);
      return View(thisCopy);
    }
    public ActionResult Edit(int id)
    {
      var thisCopy = _db.Copies
        .Include(copy => copy.Book)
        .FirstOrDefault(copy => copy.CopyId == id);
      ViewBag.BookId = new SelectList(_db.Books, "BookId", "Title");
      ViewBag.UserId = new SelectList(_userManager.Users, "Id", "UserName");
      return View(thisCopy);
    }
    [HttpPost]
    public async Task<ActionResult> Edit(Copy copy, string UserId)
    {
      var selectedUser = await _userManager.FindByIdAsync(UserId);
      copy.User = selectedUser;
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
    public async Task<ActionResult> AddUser(Copy copy, string UserId, DateTime dateCheckedOut)
    {
      var selectedUser = await _userManager.FindByIdAsync(UserId);
      copy.User = selectedUser;
      copy.IsCheckedOut = true;
      if (UserId != null)
      {
        _db.CopyPatron.Add(new CopyPatron() { CopyId = copy.CopyId, PatronId = UserId, CheckoutDate = dateCheckedOut, DueDate = dateCheckedOut.AddDays(10) });
      }
      _db.Entry(copy).State = EntityState.Modified;
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

    [HttpPost, ActionName("Details")]
    public ActionResult ReturnCopy(int id)
    {
      var thisCopy = _db.Copies
        .Include(copy => copy.CopyPatronJoinEntities)
        .ThenInclude(join => join.Patron)
        .FirstOrDefault(copy => copy.CopyId == id);
      thisCopy.User = null;
      thisCopy.IsCheckedOut = false;
      _db.Entry(thisCopy).State = EntityState.Modified;

      CopyPatron joinEntry = thisCopy.CopyPatronJoinEntities.ElementAt(thisCopy.CopyPatronJoinEntities.Count - 1);
      joinEntry.HasBeenReturned = true;
      _db.Entry(joinEntry).State = EntityState.Modified;

      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}