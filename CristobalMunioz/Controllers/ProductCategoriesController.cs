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

    public class ProductCategoriesController : Controller
    {
        private readonly AdventureWorks2019Context _context;

        public ProductCategoriesController(AdventureWorks2019Context context)
        {
            _context = context;
        }

        // GET: ProductCategories
        [Authorize("2")]
        [Authorize("3")]
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 5)
        {
            try
            {
                // Usar IQueryable para permitir la paginación en la base de datos
                var datos = _context.ProductCategories.AsQueryable();

                // Aplicar ordenamiento inicial si es necesario
                datos = datos.OrderBy(p => p.ProductCategoryId); // Reemplaza "Nombre" con el campo que desees ordenar

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
                List<ProductCategory> itemsToDisplay = datos.Skip(startIndex).Take(pageSize).ToList();

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

        // GET: ProductCategories/Details/5
        [Authorize("2")]
        [Authorize("3")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ProductCategories == null)
            {
                return NotFound();
            }

            var productCategory = await _context.ProductCategories
                .FirstOrDefaultAsync(m => m.ProductCategoryId == id);
            if (productCategory == null)
            {
                return NotFound();
            }

            return View(productCategory);
        }

        // GET: ProductCategories/Create
        [Authorize("3")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProductCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize("3")]

        public async Task<IActionResult> Create([Bind("ProductCategoryId,Name,Rowguid,ModifiedDate")] ProductCategory productCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productCategory);
        }

        // GET: ProductCategories/Edit/5
        [Authorize("3")]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ProductCategories == null)
            {
                return NotFound();
            }

            var productCategory = await _context.ProductCategories.FindAsync(id);
            if (productCategory == null)
            {
                return NotFound();
            }
            return View(productCategory);
        }

        // POST: ProductCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize("3")]

        public async Task<IActionResult> Edit(int id, [Bind("ProductCategoryId,Name,Rowguid,ModifiedDate")] ProductCategory productCategory)
        {
            if (id != productCategory.ProductCategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductCategoryExists(productCategory.ProductCategoryId))
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
            return View(productCategory);
        }

        // GET: ProductCategories/Delete/5
        [Authorize("5")]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ProductCategories == null)
            {
                return NotFound();
            }

            var productCategory = await _context.ProductCategories
                .FirstOrDefaultAsync(m => m.ProductCategoryId == id);
            if (productCategory == null)
            {
                return NotFound();
            }

            return View(productCategory);
        }

        // POST: ProductCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize("5")]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ProductCategories == null)
            {
                return Problem("Entity set 'AdventureWorks2019Context.ProductCategories'  is null.");
            }
            var productCategory = await _context.ProductCategories.FindAsync(id);
            if (productCategory != null)
            {
                _context.ProductCategories.Remove(productCategory);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductCategoryExists(int id)
        {
          return (_context.ProductCategories?.Any(e => e.ProductCategoryId == id)).GetValueOrDefault();
        }
    }
}
