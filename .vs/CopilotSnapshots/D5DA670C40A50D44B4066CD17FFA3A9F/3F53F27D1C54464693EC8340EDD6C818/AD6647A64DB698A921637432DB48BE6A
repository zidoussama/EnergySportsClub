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
    public class TerrainMaterialsController : Controller
    {
        private readonly DbcontextTest _context;

        public TerrainMaterialsController(DbcontextTest context)
        {
            _context = context;
        }

        // GET: TerrainMaterials
        public async Task<IActionResult> Index()
        {
            return View(await _context.TerrainMaterial.ToListAsync());
        }

        // GET: TerrainMaterials/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var terrainMaterial = await _context.TerrainMaterial
                .FirstOrDefaultAsync(m => m.Id == id);
            if (terrainMaterial == null)
            {
                return NotFound();
            }

            return View(terrainMaterial);
        }

        // GET: TerrainMaterials/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TerrainMaterials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id")] TerrainMaterial terrainMaterial)
        {
            if (ModelState.IsValid)
            {
                _context.Add(terrainMaterial);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(terrainMaterial);
        }

        // GET: TerrainMaterials/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var terrainMaterial = await _context.TerrainMaterial.FindAsync(id);
            if (terrainMaterial == null)
            {
                return NotFound();
            }
            return View(terrainMaterial);
        }

        // POST: TerrainMaterials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id")] TerrainMaterial terrainMaterial)
        {
            if (id != terrainMaterial.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(terrainMaterial);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TerrainMaterialExists(terrainMaterial.Id))
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
            return View(terrainMaterial);
        }

        // GET: TerrainMaterials/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var terrainMaterial = await _context.TerrainMaterial
                .FirstOrDefaultAsync(m => m.Id == id);
            if (terrainMaterial == null)
            {
                return NotFound();
            }

            return View(terrainMaterial);
        }

        // POST: TerrainMaterials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var terrainMaterial = await _context.TerrainMaterial.FindAsync(id);
            if (terrainMaterial != null)
            {
                _context.TerrainMaterial.Remove(terrainMaterial);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TerrainMaterialExists(int id)
        {
            return _context.TerrainMaterial.Any(e => e.Id == id);
        }
    }
}
