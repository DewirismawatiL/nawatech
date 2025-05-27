using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using technicalTes_Nawatech.Data;
using technicalTes_Nawatech.Models;

namespace technicalTes_Nawatech.Controllers
{
    [Authorize]
    public class ProductCategoryController : Controller
	{
		private readonly AppDbContext _context;

		public ProductCategoryController(AppDbContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index()
		{
			var categories = await _context.ProductCategories
				.Where(c => !c.IsDeleted)
				.ToListAsync();
			return View(categories);
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCategory model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.IsDeleted = false;
            _context.ProductCategories.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
		{
			var category = await _context.ProductCategories.FindAsync(id);
			if (category == null || category.IsDeleted)
			{
				return NotFound();
			}
			return View(category);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(int id, ProductCategory category)
		{
			if (id != category.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				_context.Update(category);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(category);
		}

		public async Task<IActionResult> Delete(int id)
		{
			var category = await _context.ProductCategories.FindAsync(id);
			if (category == null)
			{
				return NotFound();
			}
			category.IsDeleted = true;
			_context.Update(category);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}
	}
}
