using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CristobalMunioz.Models;

namespace CristobalMunioz.Controllers
{
    public class EmployeeDepartmentHistoriesController : Controller
    {
        private readonly AdventureWorks2019Context _context;

        public EmployeeDepartmentHistoriesController(AdventureWorks2019Context context)
        {
            _context = context;
        }

        // GET: EmployeeDepartmentHistories
        public async Task<IActionResult> Index()
        {
            var adventureWorks2019Context = _context.EmployeeDepartmentHistories.Include(e => e.BusinessEntity).Include(e => e.Department).Include(e => e.Shift);
            return View(await adventureWorks2019Context.ToListAsync());
        }

        // GET: EmployeeDepartmentHistories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.EmployeeDepartmentHistories == null)
            {
                return NotFound();
            }

            var employeeDepartmentHistory = await _context.EmployeeDepartmentHistories
                .Include(e => e.BusinessEntity)
                .Include(e => e.Department)
                .Include(e => e.Shift)
                .FirstOrDefaultAsync(m => m.BusinessEntityId == id);
            if (employeeDepartmentHistory == null)
            {
                return NotFound();
            }

            return View(employeeDepartmentHistory);
        }

        // GET: EmployeeDepartmentHistories/Create
        public IActionResult Create()
        {
            ViewData["BusinessEntityId"] = new SelectList(_context.Employees, "BusinessEntityId", "BusinessEntityId");
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentId");
            ViewData["ShiftId"] = new SelectList(_context.Shifts, "ShiftId", "ShiftId");
            return View();
        }

        // POST: EmployeeDepartmentHistories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BusinessEntityId,DepartmentId,ShiftId,StartDate,EndDate,ModifiedDate")] EmployeeDepartmentHistory employeeDepartmentHistory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employeeDepartmentHistory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BusinessEntityId"] = new SelectList(_context.Employees, "BusinessEntityId", "BusinessEntityId", employeeDepartmentHistory.BusinessEntityId);
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentId", employeeDepartmentHistory.DepartmentId);
            ViewData["ShiftId"] = new SelectList(_context.Shifts, "ShiftId", "ShiftId", employeeDepartmentHistory.ShiftId);
            return View(employeeDepartmentHistory);
        }

        // GET: EmployeeDepartmentHistories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.EmployeeDepartmentHistories == null)
            {
                return NotFound();
            }

            var employeeDepartmentHistory = await _context.EmployeeDepartmentHistories.FindAsync(id);
            if (employeeDepartmentHistory == null)
            {
                return NotFound();
            }
            ViewData["BusinessEntityId"] = new SelectList(_context.Employees, "BusinessEntityId", "BusinessEntityId", employeeDepartmentHistory.BusinessEntityId);
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentId", employeeDepartmentHistory.DepartmentId);
            ViewData["ShiftId"] = new SelectList(_context.Shifts, "ShiftId", "ShiftId", employeeDepartmentHistory.ShiftId);
            return View(employeeDepartmentHistory);
        }

        // POST: EmployeeDepartmentHistories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BusinessEntityId,DepartmentId,ShiftId,StartDate,EndDate,ModifiedDate")] EmployeeDepartmentHistory employeeDepartmentHistory)
        {
            if (id != employeeDepartmentHistory.BusinessEntityId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employeeDepartmentHistory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeDepartmentHistoryExists(employeeDepartmentHistory.BusinessEntityId))
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
            ViewData["BusinessEntityId"] = new SelectList(_context.Employees, "BusinessEntityId", "BusinessEntityId", employeeDepartmentHistory.BusinessEntityId);
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentId", employeeDepartmentHistory.DepartmentId);
            ViewData["ShiftId"] = new SelectList(_context.Shifts, "ShiftId", "ShiftId", employeeDepartmentHistory.ShiftId);
            return View(employeeDepartmentHistory);
        }

        // GET: EmployeeDepartmentHistories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.EmployeeDepartmentHistories == null)
            {
                return NotFound();
            }

            var employeeDepartmentHistory = await _context.EmployeeDepartmentHistories
                .Include(e => e.BusinessEntity)
                .Include(e => e.Department)
                .Include(e => e.Shift)
                .FirstOrDefaultAsync(m => m.BusinessEntityId == id);
            if (employeeDepartmentHistory == null)
            {
                return NotFound();
            }

            return View(employeeDepartmentHistory);
        }

        // POST: EmployeeDepartmentHistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.EmployeeDepartmentHistories == null)
            {
                return Problem("Entity set 'AdventureWorks2019Context.EmployeeDepartmentHistories'  is null.");
            }
            var employeeDepartmentHistory = await _context.EmployeeDepartmentHistories.FindAsync(id);
            if (employeeDepartmentHistory != null)
            {
                _context.EmployeeDepartmentHistories.Remove(employeeDepartmentHistory);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeDepartmentHistoryExists(int id)
        {
          return (_context.EmployeeDepartmentHistories?.Any(e => e.BusinessEntityId == id)).GetValueOrDefault();
        }
    }
}
