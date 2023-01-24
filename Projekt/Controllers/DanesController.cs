using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projekt.Data;
using Projekt.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Projekt.Controllers
{
    [Authorize]
    public class DanesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public DanesController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        // GET: Danes
        public async Task<IActionResult> Index()
        {
            //var applicationDbContext = _context.Dane.Include(d => d.User);
            var applicationDbContext = _context.Dane.Where(s => s.UserId == _userManager.GetUserId(User));
            return View(await applicationDbContext.ToListAsync());
            // return View(session);

        }

        // GET: Danes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Dane == null)
            {
                return NotFound();
            }

            var dane = await _context.Dane
                .Include(d => d.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dane == null)
            {
                return NotFound();
            }

            return View(dane);
        }

        // GET: Danes/Create

        public IActionResult Create()
        {
            ViewData["UserId"] = _userManager;

            return View();
        }

        // POST: Danes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Topic,Content,UserId")] Dane dane)
        {

            dane.UserId = _userManager.GetUserId(User);

            if (ModelState.IsValid)
            {
                _context.Add(dane);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dane);
        }

        // GET: Danes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Dane == null)
            {
                return NotFound();
            }

            var dane = await _context.Dane.FindAsync(id);
            if (dane == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = _userManager;
            return View(dane);
        }

        // POST: Danes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Topic,Content,UserId")] Dane dane)
        {
            dane.UserId = _userManager.GetUserId(User);
            if (id != dane.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dane);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DaneExists(dane.Id))
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
            
            return View(dane);
        }

        // GET: Danes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Dane == null)
            {
                return NotFound();
            }

            var dane = await _context.Dane
                .Include(d => d.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dane == null)
            {
                return NotFound();
            }

            return View(dane);
        }

        // POST: Danes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Dane == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Dane'  is null.");
            }
            var dane = await _context.Dane.FindAsync(id);
            if (dane != null)
            {
                _context.Dane.Remove(dane);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DaneExists(int id)
        {
          return _context.Dane.Any(e => e.Id == id);
        }
    }
}
