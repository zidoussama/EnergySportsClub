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
    public class ReservationMaterialsController : Controller
    {
        private readonly DbcontextTest _context;

        public ReservationMaterialsController(DbcontextTest context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var reservations = await _context.ReservationMaterials
                .Include(reservation => reservation.Material)
                .Include(reservation => reservation.User)
                .ToListAsync();
            return View("~/Views/ReservationMaterials/Index.cshtml", reservations);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservationMaterial = await _context.ReservationMaterials
                .Include(reservation => reservation.Material)
                .Include(reservation => reservation.User)
                .FirstOrDefaultAsync(reservation => reservation.Id == id);

            if (reservationMaterial == null)
            {
                return NotFound();
            }

            return View("~/Views/ReservationMaterials/Details.cshtml", reservationMaterial);
        }

        public IActionResult Create()
        {
            ViewData["MaterialId"] = new SelectList(_context.Materials, "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email");
            return View("~/Views/ReservationMaterials/Create.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ReservationDate,StartTime,EndTime,ReturnTime,MaterialId,Quantity,UserId")] ReservationMaterial reservationMaterial)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reservationMaterial);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["MaterialId"] = new SelectList(_context.Materials, "Id", "Name", reservationMaterial.MaterialId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", reservationMaterial.UserId);
            return View("~/Views/ReservationMaterials/Create.cshtml", reservationMaterial);
        }

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

            ViewData["MaterialId"] = new SelectList(_context.Materials, "Id", "Name", reservationMaterial.MaterialId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", reservationMaterial.UserId);
            return View("~/Views/ReservationMaterials/Edit.cshtml", reservationMaterial);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ReservationDate,StartTime,EndTime,ReturnTime,MaterialId,Quantity,UserId")] ReservationMaterial reservationMaterial)
        {
            if (id != reservationMaterial.Id)
            {
                return NotFound();
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
                    if (!_context.ReservationMaterials.Any(reservation => reservation.Id == reservationMaterial.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["MaterialId"] = new SelectList(_context.Materials, "Id", "Name", reservationMaterial.MaterialId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", reservationMaterial.UserId);
            return View("~/Views/ReservationMaterials/Edit.cshtml", reservationMaterial);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservationMaterial = await _context.ReservationMaterials
                .Include(reservation => reservation.Material)
                .Include(reservation => reservation.User)
                .FirstOrDefaultAsync(reservation => reservation.Id == id);

            if (reservationMaterial == null)
            {
                return NotFound();
            }

            return View("~/Views/ReservationMaterials/Delete.cshtml", reservationMaterial);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservationMaterial = await _context.ReservationMaterials.FindAsync(id);
            if (reservationMaterial != null)
            {
                _context.ReservationMaterials.Remove(reservationMaterial);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
