using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using Server.Data;
using Microsoft.EntityFrameworkCore;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ILogger<CategoriesController> _logger;
        private readonly ApplicationDbContext _context;

        public CategoriesController(ILogger<CategoriesController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Category>>> GetCatagories()
        {
            try
            {
                _logger.LogInformation("Getting all categories");
                var categories = await _context.Categories.Include(c => c.CategoryGroup).ToListAsync();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting categories");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategoryById(int id)
        {
            try
            {
                _logger.LogInformation("Getting category with ID {CategoryId}", id);
                var category = await _context.Categories
                    .Include(c => c.CategoryGroup)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (category == null)
                {
                    _logger.LogWarning("Category with ID {CategoryId} not found", id);
                    return NotFound("Sorry, no Category here. :/");
                }

                return Ok(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting category with ID {CategoryId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<List<Category>>> CreateCategory(Category category)
        {
            try
            {
                _logger.LogInformation("Creating a new category with Title {CategoryTitle}", category.Title);
                category.CategoryGroup = null;
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();

                return Ok(await GetDbCategories());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new category");
                return StatusCode(500, "Internal server error");
            }
        }

        // Similarly, update other actions like UpdateCategory, DeleteCategory, etc., with appropriate logging

        private async Task<List<Category>> GetDbCategories()
        {
            try
            {
                return await _context.Categories.Include(c => c.CategoryGroup).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching categories from database");
                throw; // Rethrow the exception to be handled by the framework's exception handler
            }
        }
    }
}