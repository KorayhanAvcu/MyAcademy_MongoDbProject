using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Travel.Web.DTOs.CategoryDtos;
using Travel.Web.Entities;
using Travel.Web.Services.CategoryServices;

namespace Travel.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController(
        ICategoryService _categoryService,
        IMapper _mapper) : Controller
    {
        // GET: /Admin/Category
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllAsync();

            var values = _mapper.Map<List<CategoryListItemDto>>(
                categories);

            return View(values);
        }

        // GET: /Admin/Category/CategoryCreate
        [HttpGet]
        public IActionResult CategoryCreate()
        {
            return View();
        }

        // POST: /Admin/Category/CategoryCreate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CategoryCreate(
            CategoryCreateDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var category = _mapper.Map<Category>(model);

            await _categoryService.CreateAsync(category);

            return RedirectToAction(nameof(Index));
        }

        // GET: /Admin/Category/CategoryUpdate/id
        [HttpGet]
        public async Task<IActionResult> CategoryUpdate(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest();

            var category = await _categoryService.GetByIdAsync(id);

            if (category == null)
                return NotFound();

            var model = _mapper.Map<CategoryUpdateDto>(
                category);

            return View(model);
        }

        // POST: /Admin/Category/CategoryUpdate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CategoryUpdate(
            CategoryUpdateDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var existingCategory =
                await _categoryService.GetByIdAsync(model.Id);

            if (existingCategory == null)
                return NotFound();

            var category = _mapper.Map<Category>(model);

            category.Id = existingCategory.Id;

            await _categoryService.UpdateAsync(category);

            return RedirectToAction(nameof(Index));
        }

        // POST: /Admin/Category/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest();

            var category =
                await _categoryService.GetByIdAsync(id);

            if (category == null)
                return NotFound();

            await _categoryService.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}