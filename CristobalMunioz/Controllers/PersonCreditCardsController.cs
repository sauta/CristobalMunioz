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
    public class PersonCreditCardsController : Controller
    {
        private readonly AdventureWorks2019Context _context;

        public PersonCreditCardsController(AdventureWorks2019Context context)
        {
            _context = context;
        }

        // GET: PersonCreditCards
        public async Task<IActionResult> Index()
        {
            var adventureWorks2019Context = _context.PersonCreditCards.Include(p => p.BusinessEntity).Include(p => p.CreditCard);
            return View(await adventureWorks2019Context.ToListAsync());
        }

        // GET: PersonCreditCards/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PersonCreditCards == null)
            {
                return NotFound();
            }

            var personCreditCard = await _context.PersonCreditCards
                .Include(p => p.BusinessEntity)
                .Include(p => p.CreditCard)
                .FirstOrDefaultAsync(m => m.BusinessEntityId == id);
            if (personCreditCard == null)
            {
                return NotFound();
            }

            return View(personCreditCard);
        }

        // GET: PersonCreditCards/Create
        public IActionResult Create()
        {
            ViewData["BusinessEntityId"] = new SelectList(_context.People, "BusinessEntityId", "BusinessEntityId");
            ViewData["CreditCardId"] = new SelectList(_context.CreditCards, "CreditCardId", "CreditCardId");
            return View();
        }

        // POST: PersonCreditCards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BusinessEntityId,CreditCardId,ModifiedDate")] PersonCreditCard personCreditCard)
        {
            if (ModelState.IsValid)
            {
                _context.Add(personCreditCard);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BusinessEntityId"] = new SelectList(_context.People, "BusinessEntityId", "BusinessEntityId", personCreditCard.BusinessEntityId);
            ViewData["CreditCardId"] = new SelectList(_context.CreditCards, "CreditCardId", "CreditCardId", personCreditCard.CreditCardId);
            return View(personCreditCard);
        }

        // GET: PersonCreditCards/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PersonCreditCards == null)
            {
                return NotFound();
            }

            var personCreditCard = await _context.PersonCreditCards.FindAsync(id);
            if (personCreditCard == null)
            {
                return NotFound();
            }
            ViewData["BusinessEntityId"] = new SelectList(_context.People, "BusinessEntityId", "BusinessEntityId", personCreditCard.BusinessEntityId);
            ViewData["CreditCardId"] = new SelectList(_context.CreditCards, "CreditCardId", "CreditCardId", personCreditCard.CreditCardId);
            return View(personCreditCard);
        }

        // POST: PersonCreditCards/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BusinessEntityId,CreditCardId,ModifiedDate")] PersonCreditCard personCreditCard)
        {
            if (id != personCreditCard.BusinessEntityId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(personCreditCard);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonCreditCardExists(personCreditCard.BusinessEntityId))
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
            ViewData["BusinessEntityId"] = new SelectList(_context.People, "BusinessEntityId", "BusinessEntityId", personCreditCard.BusinessEntityId);
            ViewData["CreditCardId"] = new SelectList(_context.CreditCards, "CreditCardId", "CreditCardId", personCreditCard.CreditCardId);
            return View(personCreditCard);
        }

        // GET: PersonCreditCards/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PersonCreditCards == null)
            {
                return NotFound();
            }

            var personCreditCard = await _context.PersonCreditCards
                .Include(p => p.BusinessEntity)
                .Include(p => p.CreditCard)
                .FirstOrDefaultAsync(m => m.BusinessEntityId == id);
            if (personCreditCard == null)
            {
                return NotFound();
            }

            return View(personCreditCard);
        }

        // POST: PersonCreditCards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PersonCreditCards == null)
            {
                return Problem("Entity set 'AdventureWorks2019Context.PersonCreditCards'  is null.");
            }
            var personCreditCard = await _context.PersonCreditCards.FindAsync(id);
            if (personCreditCard != null)
            {
                _context.PersonCreditCards.Remove(personCreditCard);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonCreditCardExists(int id)
        {
          return (_context.PersonCreditCards?.Any(e => e.BusinessEntityId == id)).GetValueOrDefault();
        }
    }
}
