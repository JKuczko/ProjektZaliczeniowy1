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
    public class ForumController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ForumController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            //var applicationDbContext = _context.Dane.Include(d => d.User);
            var applicationDbContext = _context.Dane;
            return View(await applicationDbContext.ToListAsync());
            // return View(session);
        }
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
    }
}
