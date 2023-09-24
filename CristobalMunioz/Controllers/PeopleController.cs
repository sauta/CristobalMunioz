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
    public class PeopleController : Controller
    {
        private readonly AdventureWorks2019Context _context;

        public PeopleController(AdventureWorks2019Context context)
        {
            _context = context;
        }

        // GET: People

        //public async Task<IActionResult> Index()
        //{
        //    var adventureWorks2019Context = _context.People.Include(p => p.BusinessEntity);
        //    return View(await adventureWorks2019Context.ToListAsync());
        //}
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 5)
        {
            try
            {
                // Usar IQueryable para permitir la paginación en la base de datos
                var datos = _context.People.AsQueryable();

                // Aplicar ordenamiento inicial si es necesario
                datos = datos.OrderBy(p => p.BusinessEntityId); // Reemplaza "Nombre" con el campo que desees ordenar

                // Validar los parámetros de paginación
                if (pageNumber < 1)
                {
                    pageNumber = 1;
                }
                if (pageSize < 1)
                {
                    pageSize = 5;
                }

                // Calcula el índice de inicio y fin de la página actual
                int startIndex = (pageNumber - 1) * pageSize;
                int endIndex = startIndex + pageSize;

                // Obtiene los elementos de la página actual
                List<Person> itemsToDisplay = datos.Skip(startIndex).Take(pageSize).ToList();

                // Calcula el número total de páginas
                int totalItems = datos.Count();
                int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

                // Pasa los datos a la vista junto con información de paginación
                ViewData["PageNumber"] = pageNumber;
                ViewData["PageSize"] = pageSize;
                ViewData["TotalPages"] = totalPages;
                ViewData["TotalItems"] = totalItems;

                return View(itemsToDisplay);
            }
            catch (Exception ex)
            {
                // Manejar excepciones y mostrar un mensaje de error
                ViewData["ErrorMessage"] = "Error al cargar los datos: " + ex.Message;
                return View(new List<Person>());
            }
        }

        // GET: People/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.People == null)
            {
                return NotFound();
            }

            var person = await _context.People
                .Include(p => p.BusinessEntity)
                .FirstOrDefaultAsync(m => m.BusinessEntityId == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // GET: People/Create
        public IActionResult Create()
        {
            ViewData["BusinessEntityId"] = new SelectList(_context.BusinessEntities, "BusinessEntityId", "BusinessEntityId");
            return View();
        }

        // POST: People/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BusinessEntityId,PersonType,NameStyle,Title,FirstName,MiddleName,LastName,Suffix,EmailPromotion,AdditionalContactInfo,Demographics,Rowguid,ModifiedDate")] Person person)
        {
            if (ModelState.IsValid)
            {
                _context.Add(person);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BusinessEntityId"] = new SelectList(_context.BusinessEntities, "BusinessEntityId", "BusinessEntityId", person.BusinessEntityId);
            return View(person);
        }

        // GET: People/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.People == null)
            {
                return NotFound();
            }

            var person = await _context.People.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }
            ViewData["BusinessEntityId"] = new SelectList(_context.BusinessEntities, "BusinessEntityId", "BusinessEntityId", person.BusinessEntityId);
            return View(person);
        }

        // POST: People/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BusinessEntityId,PersonType,NameStyle,Title,FirstName,MiddleName,LastName,Suffix,EmailPromotion,AdditionalContactInfo,Demographics,Rowguid,ModifiedDate")] Person person)
        {
            if (id != person.BusinessEntityId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(person);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonExists(person.BusinessEntityId))
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
            ViewData["BusinessEntityId"] = new SelectList(_context.BusinessEntities, "BusinessEntityId", "BusinessEntityId", person.BusinessEntityId);
            return View(person);
        }

        // GET: People/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.People == null)
            {
                return NotFound();
            }

            var person = await _context.People
                .Include(p => p.BusinessEntity)
                .FirstOrDefaultAsync(m => m.BusinessEntityId == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.People == null)
            {
                return Problem("Entity set 'AdventureWorks2019Context.People'  is null.");
            }
            var person = await _context.People.FindAsync(id);
            if (person != null)
            {
                _context.People.Remove(person);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonExists(int id)
        {
          return (_context.People?.Any(e => e.BusinessEntityId == id)).GetValueOrDefault();
        }
    }
}
