using System.Linq;
using System.Threading.Tasks;
using EnergySportsClub.Data;
using EnergySportsClub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EnergySportsClub.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class TerrainsController : Controller
    {
        private readonly DbcontextTest _context;

        public TerrainsController(DbcontextTest context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var terrains = await _context.Terrains.ToListAsync();
            return View("~/Views/Terrains/Index.cshtml", terrains);
        }

        public IActionResult Create()
        {
            return View("~/Views/Terrains/Create.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Type,Dimensions,TerrainStatus,PricePerHour")] Terrain terrain)
        {
            if (ModelState.IsValid)
            {
                _context.Add(terrain);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View("~/Views/Terrains/Create.cshtml", terrain);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var terrain = await _context.Terrains.FindAsync(id);
            if (terrain == null)
            {
                return NotFound();
            }

            return View("~/Views/Terrains/Edit.cshtml", terrain);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Type,Dimensions,TerrainStatus,PricePerHour")] Terrain terrain)
        {
            if (id != terrain.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(terrain);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TerrainExists(terrain.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View("~/Views/Terrains/Edit.cshtml", terrain);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var terrain = await _context.Terrains.FirstOrDefaultAsync(m => m.Id == id);
            if (terrain == null)
            {
                return NotFound();
            }

            return View("~/Views/Terrains/Delete.cshtml", terrain);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var terrain = await _context.Terrains.FindAsync(id);
            if (terrain != null)
            {
                _context.Terrains.Remove(terrain);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool TerrainExists(int id)
        {
            return _context.Terrains.Any(e => e.Id == id);
        }
    }
}
