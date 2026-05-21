using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<ApplicationUser> _userManager;

        public ReservationTerrainsController(DbcontextTest context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: ReservationTerrains
        public async Task<IActionResult> Index()
        {
            IQueryable<ReservationTerrain> query = _context.ReservationTerrains
                .Include(r => r.Terrain)
                .Include(r => r.User);
            if (User.IsInRole("User"))
            {
                var userId = _userManager.GetUserId(User);
                query = query.Where(reservation => reservation.UserId == userId);
            }

            return View(await query.ToListAsync());
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
        [Authorize(Roles = "User")]
        public IActionResult Create(int? terrainId)
        {
            var terrainItems = _context.Terrains
                .Select(terrain => new SelectListItem
                {
                    Value = terrain.Id.ToString(),
                    Text = $"{terrain.Type} ({terrain.TerrainStatus})"
                })
                .ToList();
            ViewData["TerrainId"] = new SelectList(terrainItems, "Value", "Text", terrainId?.ToString());
            var model = new ReservationTerrain
            {
                TerrainId = terrainId ?? 0,
                ReservationDate = DateTime.Today
            };
            return View(model);
        }

        // POST: ReservationTerrains/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Create([Bind("Id,ReservationDate,StartTime,EndTime,TerrainId")] ReservationTerrain reservationTerrain)
        {
            reservationTerrain.UserId = _userManager.GetUserId(User) ?? string.Empty;
            ModelState.Remove(nameof(ReservationTerrain.UserId));

            if (reservationTerrain.ReservationDate.Date < DateTime.Today)
            {
                ModelState.AddModelError(nameof(ReservationTerrain.ReservationDate), "Reservation date must be today or later.");
            }

            if (ModelState.IsValid)
            {
                var terrain = await _context.Terrains.FindAsync(reservationTerrain.TerrainId);
                if (terrain == null)
                {
                    ModelState.AddModelError(string.Empty, "Selected terrain was not found.");
                }
                else
                {
                    var now = DateTime.Now;
                    var reservationDate = reservationTerrain.ReservationDate.Date;
                    var reservationStart = reservationDate.Add(reservationTerrain.StartTime);
                    var reservationEnd = reservationDate.Add(reservationTerrain.EndTime);

                    var hasOverlap = await _context.ReservationTerrains
                        .AnyAsync(reservation => reservation.TerrainId == reservationTerrain.TerrainId
                            && reservation.ReservationDate.Date == reservationDate
                            && reservationTerrain.StartTime < reservation.EndTime
                            && reservationTerrain.EndTime > reservation.StartTime);

                    if (hasOverlap)
                    {
                        ModelState.AddModelError(string.Empty, "Selected terrain is not available for this time range.");
                    }
                    else
                    {
                        _context.Add(reservationTerrain);
                        await _context.SaveChangesAsync();
                        await UpdateTerrainStatusAsync(reservationTerrain.TerrainId);
                        TempData["Message"] = "Reservation created successfully.";
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            var terrainItems = _context.Terrains
                .Select(terrain => new SelectListItem
                {
                    Value = terrain.Id.ToString(),
                    Text = $"{terrain.Type} ({terrain.TerrainStatus})"
                })
                .ToList();
            ViewData["TerrainId"] = new SelectList(terrainItems, "Value", "Text", reservationTerrain.TerrainId.ToString());
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
            var terrainItems = _context.Terrains
                .Select(terrain => new SelectListItem
                {
                    Value = terrain.Id.ToString(),
                    Text = $"{terrain.Type} - {terrain.Dimensions}"
                })
                .ToList();
            ViewData["TerrainId"] = new SelectList(terrainItems, "Value", "Text", reservationTerrain.TerrainId.ToString());
            return View(reservationTerrain);
        }

        // POST: ReservationTerrains/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ReservationDate,StartTime,EndTime,TerrainId")] ReservationTerrain reservationTerrain)
        {
            if (id != reservationTerrain.Id)
            {
                return NotFound();
            }

            var existingReservation = await _context.ReservationTerrains.AsNoTracking()
                .FirstOrDefaultAsync(reservation => reservation.Id == id);
            if (existingReservation == null)
            {
                return NotFound();
            }

            reservationTerrain.UserId = existingReservation.UserId;

            if (reservationTerrain.ReservationDate.Date < DateTime.Today)
            {
                ModelState.AddModelError(nameof(ReservationTerrain.ReservationDate), "Reservation date must be today or later.");
            }

            if (ModelState.IsValid)
            {
                var reservationDate = reservationTerrain.ReservationDate.Date;
                var hasOverlap = await _context.ReservationTerrains
                    .AnyAsync(reservation => reservation.TerrainId == reservationTerrain.TerrainId
                        && reservation.ReservationDate.Date == reservationDate
                        && reservation.Id != reservationTerrain.Id
                        && reservationTerrain.StartTime < reservation.EndTime
                        && reservationTerrain.EndTime > reservation.StartTime);

                if (hasOverlap)
                {
                    ModelState.AddModelError(string.Empty, "Selected terrain is not available for this time range.");
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservationTerrain);
                    await _context.SaveChangesAsync();
                    await UpdateTerrainStatusAsync(reservationTerrain.TerrainId);
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
            var terrainItems = _context.Terrains
                .Select(terrain => new SelectListItem
                {
                    Value = terrain.Id.ToString(),
                    Text = $"{terrain.Type} - {terrain.Dimensions}"
                })
                .ToList();
            ViewData["TerrainId"] = new SelectList(terrainItems, "Value", "Text", reservationTerrain.TerrainId.ToString());
            return View(reservationTerrain);
        }

        [Authorize(Roles = "User")]
        [HttpGet]
        public async Task<IActionResult> AvailableTerrains(DateTime reservationDate, TimeSpan startTime, TimeSpan endTime)
        {
            if (endTime <= startTime)
            {
                return Ok(Array.Empty<object>());
            }

            var date = reservationDate.Date;
            var availableTerrains = await _context.Terrains
                .Where(terrain => !_context.ReservationTerrains.Any(reservation => reservation.TerrainId == terrain.Id
                    && reservation.ReservationDate.Date == date
                    && startTime < reservation.EndTime
                    && endTime > reservation.StartTime))
                .Select(terrain => new
                {
                    terrain.Id,
                    Label = $"{terrain.Type} - {terrain.Dimensions}"
                })
                .ToListAsync();

            return Ok(availableTerrains);
        }

        private async Task UpdateTerrainStatusAsync(int terrainId)
        {
            var terrain = await _context.Terrains.FindAsync(terrainId);
            if (terrain == null)
            {
                return;
            }

            var now = DateTime.Now;
            var today = DateTime.Today;
            var nowTime = now.TimeOfDay;
            var hasActiveReservation = await _context.ReservationTerrains
                .AnyAsync(reservation => reservation.TerrainId == terrainId
                    && reservation.ReservationDate.Date == today
                    && reservation.StartTime <= nowTime
                    && reservation.EndTime > nowTime);

            terrain.TerrainStatus = hasActiveReservation ? Terrain.Status.Unavailable : Terrain.Status.Available;
            await _context.SaveChangesAsync();
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
