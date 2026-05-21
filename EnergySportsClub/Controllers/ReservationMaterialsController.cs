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
    public class ReservationMaterialsController : Controller
    {
        private readonly DbcontextTest _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReservationMaterialsController(DbcontextTest context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: ReservationMaterials
        public async Task<IActionResult> Index()
        {
            IQueryable<ReservationMaterial> query = _context.ReservationMaterials
                .Include(r => r.Material)
                .Include(r => r.User);
            if (User.IsInRole("User"))
            {
                var userId = _userManager.GetUserId(User);
                query = query.Where(reservation => reservation.UserId == userId);
            }

            return View(await query.ToListAsync());
        }

        // GET: ReservationMaterials/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservationMaterial = await _context.ReservationMaterials
                .Include(r => r.Material)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservationMaterial == null)
            {
                return NotFound();
            }

            return View(reservationMaterial);
        }

        // GET: ReservationMaterials/Create
        [Authorize(Roles = "User")]
        public IActionResult Create(int? materialId)
        {
            var materialItems = _context.Materials
                .Select(material => new SelectListItem
                {
                    Value = material.Id.ToString(),
                    Text = $"{material.Name} ({material.MaterialStatus}, Stock: {material.QteStock})"
                })
                .ToList();
            ViewData["MaterialId"] = new SelectList(materialItems, "Value", "Text", materialId?.ToString());
            var model = new ReservationMaterial
            {
                MaterialId = materialId ?? 0,
                ReservationDate = DateTime.Today,
                Quantity = 1
            };
            return View(model);
        }

        // POST: ReservationMaterials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Create([Bind("Id,ReservationDate,StartTime,EndTime,ReturnTime,MaterialId,Quantity")] ReservationMaterial reservationMaterial)
        {
            reservationMaterial.UserId = _userManager.GetUserId(User) ?? string.Empty;
            ModelState.Remove(nameof(ReservationMaterial.UserId));

            if (reservationMaterial.ReservationDate.Date < DateTime.Today)
            {
                ModelState.AddModelError(nameof(ReservationMaterial.ReservationDate), "Reservation date must be today or later.");
            }

            if (ModelState.IsValid)
            {
                var material = await _context.Materials.FirstOrDefaultAsync(item => item.Id == reservationMaterial.MaterialId);
                if (material == null)
                {
                    ModelState.AddModelError(nameof(ReservationMaterial.MaterialId), "Selected material was not found.");
                }
                else
                {
                var reservationDate = reservationMaterial.ReservationDate.Date;
                var hasOverlap = await _context.ReservationMaterials
                    .AnyAsync(reservation => reservation.MaterialId == reservationMaterial.MaterialId
                        && reservation.ReservationDate.Date == reservationDate
                        && reservationMaterial.StartTime < reservation.ReturnTime
                        && reservationMaterial.ReturnTime > reservation.StartTime);

                var reservedQuantity = await _context.ReservationMaterials
                    .Where(reservation => reservation.MaterialId == reservationMaterial.MaterialId
                        && reservation.ReservationDate.Date == reservationDate
                        && reservationMaterial.StartTime < reservation.ReturnTime
                        && reservationMaterial.ReturnTime > reservation.StartTime)
                    .SumAsync(reservation => reservation.Quantity);

                var remainingStock = material.QteStock - reservedQuantity;
                if (reservationMaterial.Quantity > remainingStock)
                {
                    ModelState.AddModelError(nameof(ReservationMaterial.Quantity), "Insufficient stock for the selected time range.");
                }
                else if (hasOverlap && remainingStock <= 0)
                {
                    ModelState.AddModelError(string.Empty, "Selected material is not available for this time range.");
                }
                else
                {
                    _context.Add(reservationMaterial);
                    await _context.SaveChangesAsync();
                    TempData["Message"] = "Reservation created successfully.";
                    return RedirectToAction(nameof(Index));
                }
                }
            }
            var materialItems = _context.Materials
                .Select(material => new SelectListItem
                {
                    Value = material.Id.ToString(),
                    Text = $"{material.Name} ({material.MaterialStatus}, Stock: {material.QteStock})"
                })
                .ToList();
            ViewData["MaterialId"] = new SelectList(materialItems, "Value", "Text", reservationMaterial.MaterialId.ToString());
            return View(reservationMaterial);
        }

        // GET: ReservationMaterials/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservationMaterial = await _context.ReservationMaterials.FindAsync(id);
            if (reservationMaterial == null)
            {
                return NotFound();
            }
            var materialItems = _context.Materials
                .Select(material => new SelectListItem
                {
                    Value = material.Id.ToString(),
                    Text = $"{material.Name} - {material.Description}"
                })
                .ToList();
            ViewData["MaterialId"] = new SelectList(materialItems, "Value", "Text", reservationMaterial.MaterialId.ToString());
            return View(reservationMaterial);
        }

        // POST: ReservationMaterials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ReservationDate,StartTime,EndTime,ReturnTime,MaterialId,Quantity")] ReservationMaterial reservationMaterial)
        {
            if (id != reservationMaterial.Id)
            {
                return NotFound();
            }

            var existingReservation = await _context.ReservationMaterials.AsNoTracking()
                .FirstOrDefaultAsync(reservation => reservation.Id == id);
            if (existingReservation == null)
            {
                return NotFound();
            }

            reservationMaterial.UserId = existingReservation.UserId;

            if (reservationMaterial.ReservationDate.Date < DateTime.Today)
            {
                ModelState.AddModelError(nameof(ReservationMaterial.ReservationDate), "Reservation date must be today or later.");
            }

            if (ModelState.IsValid)
            {
                var material = await _context.Materials.FirstOrDefaultAsync(item => item.Id == reservationMaterial.MaterialId);
                if (material == null)
                {
                    ModelState.AddModelError(nameof(ReservationMaterial.MaterialId), "Selected material was not found.");
                }
                else
                {
                    var reservationDate = reservationMaterial.ReservationDate.Date;
                    var reservedQuantity = await _context.ReservationMaterials
                        .Where(reservation => reservation.MaterialId == reservationMaterial.MaterialId
                            && reservation.ReservationDate.Date == reservationDate
                            && reservation.Id != reservationMaterial.Id
                            && reservationMaterial.StartTime < reservation.ReturnTime
                            && reservationMaterial.ReturnTime > reservation.StartTime)
                        .SumAsync(reservation => reservation.Quantity);

                    var remainingStock = material.QteStock - reservedQuantity;
                    if (reservationMaterial.Quantity > remainingStock)
                    {
                        ModelState.AddModelError(nameof(ReservationMaterial.Quantity), "Insufficient stock for the selected time range.");
                    }
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservationMaterial);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationMaterialExists(reservationMaterial.Id))
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
            var materialItems = _context.Materials
                .Select(material => new SelectListItem
                {
                    Value = material.Id.ToString(),
                    Text = $"{material.Name} - {material.Description}"
                })
                .ToList();
            ViewData["MaterialId"] = new SelectList(materialItems, "Value", "Text", reservationMaterial.MaterialId.ToString());
            return View(reservationMaterial);
        }

        [Authorize(Roles = "User")]
        [HttpGet]
        public async Task<IActionResult> RemainingStock(int materialId, DateTime reservationDate, TimeSpan startTime, TimeSpan returnTime)
        {
            if (returnTime <= startTime)
            {
                return BadRequest();
            }

            var material = await _context.Materials.FirstOrDefaultAsync(item => item.Id == materialId);
            if (material == null)
            {
                return NotFound();
            }

            var date = reservationDate.Date;
            var reservedQuantity = await _context.ReservationMaterials
                .Where(reservation => reservation.MaterialId == materialId
                    && reservation.ReservationDate.Date == date
                    && startTime < reservation.ReturnTime
                    && returnTime > reservation.StartTime)
                .SumAsync(reservation => reservation.Quantity);

            var remaining = material.QteStock - reservedQuantity;
            return Ok(new { remaining });
        }

        // GET: ReservationMaterials/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservationMaterial = await _context.ReservationMaterials
                .Include(r => r.Material)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservationMaterial == null)
            {
                return NotFound();
            }

            return View(reservationMaterial);
        }

        // POST: ReservationMaterials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservationMaterial = await _context.ReservationMaterials.FindAsync(id);
            if (reservationMaterial != null)
            {
                _context.ReservationMaterials.Remove(reservationMaterial);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationMaterialExists(int id)
        {
            return _context.ReservationMaterials.Any(e => e.Id == id);
        }
    }
}
