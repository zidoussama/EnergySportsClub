using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EnergySportsClub.Data;
using EnergySportsClub.Models;

namespace EnergySportsClub.Controllers
{
    public class ReservationTerrainsController : Controller
    {
        private readonly DbcontextTest _context;

        public ReservationTerrainsController(DbcontextTest context)
        {
            _context = context;
        }

        // GET: ReservationTerrains
        public async Task<IActionResult> Index()
        {
            var dbcontextTest = _context.ReservationTerrains.Include(r => r.Terrain).Include(r => r.User);
            return View(await dbcontextTest.ToListAsync());
        }

        // GET: ReservationTerrains/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservationTerrain = await _context.ReservationTerrains
                .Include(r => r.Terrain)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservationTerrain == null)
            {
                return NotFound();
            }

            return View(reservationTerrain);
        }

        // GET: ReservationTerrains/Create
        public IActionResult Create()
        {
            ViewData["TerrainId"] = new SelectList(_context.Terrains, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: ReservationTerrains/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ReservationDate,StartTime,EndTime,UserId,TerrainId")] ReservationTerrain reservationTerrain)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reservationTerrain);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TerrainId"] = new SelectList(_context.Terrains, "Id", "Id", reservationTerrain.TerrainId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", reservationTerrain.UserId);
            return View(reservationTerrain);
        }

        // GET: ReservationTerrains/Edit/5
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
            ViewData["TerrainId"] = new SelectList(_context.Terrains, "Id", "Id", reservationTerrain.TerrainId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", reservationTerrain.UserId);
            return View(reservationTerrain);
        }

        // POST: ReservationTerrains/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ReservationDate,StartTime,EndTime,UserId,TerrainId")] ReservationTerrain reservationTerrain)
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
                    if (!ReservationTerrainExists(reservationTerrain.Id))
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
            ViewData["TerrainId"] = new SelectList(_context.Terrains, "Id", "Id", reservationTerrain.TerrainId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", reservationTerrain.UserId);
            return View(reservationTerrain);
        }

        // GET: ReservationTerrains/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservationTerrain = await _context.ReservationTerrains
                .Include(r => r.Terrain)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservationTerrain == null)
            {
                return NotFound();
            }

            return View(reservationTerrain);
        }

        // POST: ReservationTerrains/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservationTerrain = await _context.ReservationTerrains.FindAsync(id);
            if (reservationTerrain != null)
            {
                _context.ReservationTerrains.Remove(reservationTerrain);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationTerrainExists(int id)
        {
            return _context.ReservationTerrains.Any(e => e.Id == id);
        }
    }
}
