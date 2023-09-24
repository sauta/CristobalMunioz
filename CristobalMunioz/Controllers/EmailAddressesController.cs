using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CristobalMunioz.Models;
using Microsoft.AspNetCore.Authorization;

namespace CristobalMunioz.Controllers
{
    [Authorize]
    public class EmailAddressesController : Controller
    {
        private readonly AdventureWorks2019Context _context;

        public EmailAddressesController(AdventureWorks2019Context context)
        {
            _context = context;
        }

        // GET: EmailAddresses
        public async Task<IActionResult> Index()
        {
            var adventureWorks2019Context = _context.EmailAddresses.Include(e => e.BusinessEntity);
            return View(await adventureWorks2019Context.ToListAsync());
        }

        // GET: EmailAddresses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.EmailAddresses == null)
            {
                return NotFound();
            }

            var emailAddress = await _context.EmailAddresses
                .Include(e => e.BusinessEntity)
                .FirstOrDefaultAsync(m => m.BusinessEntityId == id);
            if (emailAddress == null)
            {
                return NotFound();
            }

            return View(emailAddress);
        }

        // GET: EmailAddresses/Create
        public IActionResult Create()
        {
            ViewData["BusinessEntityId"] = new SelectList(_context.People, "BusinessEntityId", "BusinessEntityId");
            return View();
        }

        // POST: EmailAddresses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BusinessEntityId,EmailAddressId,EmailAddress1,Rowguid,ModifiedDate")] EmailAddress emailAddress)
        {
            if (ModelState.IsValid)
            {
                _context.Add(emailAddress);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BusinessEntityId"] = new SelectList(_context.People, "BusinessEntityId", "BusinessEntityId", emailAddress.BusinessEntityId);
            return View(emailAddress);
        }

        // GET: EmailAddresses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.EmailAddresses == null)
            {
                return NotFound();
            }

            var emailAddress = await _context.EmailAddresses.FindAsync(id);
            if (emailAddress == null)
            {
                return NotFound();
            }
            ViewData["BusinessEntityId"] = new SelectList(_context.People, "BusinessEntityId", "BusinessEntityId", emailAddress.BusinessEntityId);
            return View(emailAddress);
        }

        // POST: EmailAddresses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BusinessEntityId,EmailAddressId,EmailAddress1,Rowguid,ModifiedDate")] EmailAddress emailAddress)
        {
            if (id != emailAddress.BusinessEntityId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(emailAddress);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmailAddressExists(emailAddress.BusinessEntityId))
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
            ViewData["BusinessEntityId"] = new SelectList(_context.People, "BusinessEntityId", "BusinessEntityId", emailAddress.BusinessEntityId);
            return View(emailAddress);
        }

        // GET: EmailAddresses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.EmailAddresses == null)
            {
                return NotFound();
            }

            var emailAddress = await _context.EmailAddresses
                .Include(e => e.BusinessEntity)
                .FirstOrDefaultAsync(m => m.BusinessEntityId == id);
            if (emailAddress == null)
            {
                return NotFound();
            }

            return View(emailAddress);
        }

        // POST: EmailAddresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.EmailAddresses == null)
            {
                return Problem("Entity set 'AdventureWorks2019Context.EmailAddresses'  is null.");
            }
            var emailAddress = await _context.EmailAddresses.FindAsync(id);
            if (emailAddress != null)
            {
                _context.EmailAddresses.Remove(emailAddress);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmailAddressExists(int id)
        {
          return (_context.EmailAddresses?.Any(e => e.BusinessEntityId == id)).GetValueOrDefault();
        }
    }
}
