using System.Linq;
using System.Threading.Tasks;
using EnergySportsClub.Data;
using EnergySportsClub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EnergySportsClub.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ReservationTerrainsController : Controller
    {
        private readonly DbcontextTest _context;

        public ReservationTerrainsController(DbcontextTest context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var reservations = await _context.ReservationTerrains
                .Include(reservation => reservation.Terrain)
                .Include(reservation => reservation.User)
                .ToListAsync();
            return View("~/Views/ReservationTerrains/Index.cshtml", reservations);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservationTerrain = await _context.ReservationTerrains
                .Include(reservation => reservation.Terrain)
                .Include(reservation => reservation.User)
                .FirstOrDefaultAsync(reservation => reservation.Id == id);

            if (reservationTerrain == null)
            {
                return NotFound();
            }

            return View("~/Views/ReservationTerrains/Details.cshtml", reservationTerrain);
        }

        public IActionResult Create()
        {
            ViewData["TerrainId"] = new SelectList(_context.Terrains, "Id", "Type");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email");
            return View("~/Views/ReservationTerrains/Create.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ReservationDate,StartTime,EndTime,TerrainId,UserId")] ReservationTerrain reservationTerrain)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reservationTerrain);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["TerrainId"] = new SelectList(_context.Terrains, "Id", "Type", reservationTerrain.TerrainId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", reservationTerrain.UserId);
            return View("~/Views/ReservationTerrains/Create.cshtml", reservationTerrain);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservationTerrain = await _context.ReservationTerrains.FindAsync(id);
            if (reservationTerrain == null)
            {
                return NotFound();
            }

            ViewData["TerrainId"] = new SelectList(_context.Terrains, "Id", "Type", reservationTerrain.TerrainId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", reservationTerrain.UserId);
            return View("~/Views/ReservationTerrains/Edit.cshtml", reservationTerrain);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ReservationDate,StartTime,EndTime,TerrainId,UserId")] ReservationTerrain reservationTerrain)
        {
            if (id != reservationTerrain.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservationTerrain);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.ReservationTerrains.Any(reservation => reservation.Id == reservationTerrain.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["TerrainId"] = new SelectList(_context.Terrains, "Id", "Type", reservationTerrain.TerrainId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", reservationTerrain.UserId);
            return View("~/Views/ReservationTerrains/Edit.cshtml", reservationTerrain);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservationTerrain = await _context.ReservationTerrains
                .Include(reservation => reservation.Terrain)
                .Include(reservation => reservation.User)
                .FirstOrDefaultAsync(reservation => reservation.Id == id);

            if (reservationTerrain == null)
            {
                return NotFound();
            }

            return View("~/Views/ReservationTerrains/Delete.cshtml", reservationTerrain);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservationTerrain = await _context.ReservationTerrains.FindAsync(id);
            if (reservationTerrain != null)
            {
                _context.ReservationTerrains.Remove(reservationTerrain);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
