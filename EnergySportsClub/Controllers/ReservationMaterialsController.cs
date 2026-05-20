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
    public class ReservationMaterialsController : Controller
    {
        private readonly DbcontextTest _context;

        public ReservationMaterialsController(DbcontextTest context)
        {
            _context = context;
        }

        // GET: ReservationMaterials
        public async Task<IActionResult> Index()
        {
            var dbcontextTest = _context.ReservationMaterials.Include(r => r.Material).Include(r => r.User);
            return View(await dbcontextTest.ToListAsync());
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
        public IActionResult Create()
        {
            ViewData["MaterialId"] = new SelectList(_context.Materials, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: ReservationMaterials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ReservationDate,StartTime,EndTime,ReturnTime,MaterialId,UserId")] ReservationMaterial reservationMaterial)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reservationMaterial);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaterialId"] = new SelectList(_context.Materials, "Id", "Id", reservationMaterial.MaterialId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", reservationMaterial.UserId);
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
            ViewData["MaterialId"] = new SelectList(_context.Materials, "Id", "Id", reservationMaterial.MaterialId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", reservationMaterial.UserId);
            return View(reservationMaterial);
        }

        // POST: ReservationMaterials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ReservationDate,StartTime,EndTime,ReturnTime,MaterialId,UserId")] ReservationMaterial reservationMaterial)
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
            ViewData["MaterialId"] = new SelectList(_context.Materials, "Id", "Id", reservationMaterial.MaterialId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", reservationMaterial.UserId);
            return View(reservationMaterial);
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
