using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projekt.Data;
using Projekt.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Projekt.Controllers
{
    public class KomentarzesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;


        public KomentarzesController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }
        public static class GlobalVariables
        {
            public static int IdPost = 0;
        }
        // GET: Komentarzes
        
        public async Task<IActionResult> Index(int name)
        {
            GlobalVariables.IdPost = name;
            var applicationDbContext = _context.Komentarze.Include(k => k.User).Include(k => k.Dane).Where(k => k.PostId == name);
            ViewData["h"] = name;
            ViewData["testhxd"] = GlobalVariables.IdPost;
            return View(await applicationDbContext.ToListAsync());
            

        }
        /*[HttpPost]
        public ActionResult SubmitForm(int name)
        {
            GlobalVariables.IdPost = name;
            //var applicationDbContext = _context.Komentarze.Include(k => k.User).Include(k => k.PostId).Where(k => k.PostId == name);
            ViewData["testh"] = name;
            ViewData["testh2"] = "debil";
            return RedirectToAction("Index", "Komentarzes");
        }*/
       
    
            // GET: Komentarzes/Details/5
            public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Komentarze == null)
            {
                return NotFound();
            }

            var komentarze = await _context.Komentarze
                .Include(k => k.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (komentarze == null)
            {
                return NotFound();
            } 
            return View(komentarze);
        }

        // GET: Komentarzes/Create
        public IActionResult Create()
        { 
            int IdPost = GlobalVariables.IdPost;
            return View();
        }

        // POST: Komentarzes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Comment,PostId,UserId")] Komentarze komentarze, int topicId, string Koment)
        {
            komentarze.PostId = topicId;
            komentarze.Comment = Koment;
            ViewData["hxd"] = topicId;
            var IdPost = GlobalVariables.IdPost;
          
            if (ModelState.IsValid)
            {
                _context.Add(komentarze);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), "Forum");
            }

            return RedirectToAction("Index", "Forum");
        }

        // GET: Komentarzes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Komentarze == null)
            {
                return NotFound();
            }

            var komentarze = await _context.Komentarze.FindAsync(id);
            if (komentarze == null)
            {
                return NotFound();
            }
            return View();
        }

        // POST: Komentarzes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Comment,PostId,UserId")] Komentarze komentarze)
        {
            if (id != komentarze.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(komentarze);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KomentarzeExists(komentarze.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            komentarze.UserId = _userManager.GetUserId(User);
            return View(komentarze);
        }

        // GET: Komentarzes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Komentarze == null)
            {
                return NotFound();
            }

            var komentarze = await _context.Komentarze
                .Include(k => k.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (komentarze == null)
            {
                return NotFound();
            }

            return View(komentarze);
        }

        // POST: Komentarzes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Komentarze == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Komentarze'  is null.");
            }
            var komentarze = await _context.Komentarze.FindAsync(id);
            if (komentarze != null)
            {
                _context.Komentarze.Remove(komentarze);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KomentarzeExists(int id)
        {
          return _context.Komentarze.Any(e => e.Id == id);
        }
    }
}
