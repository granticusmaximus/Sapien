using Microsoft.AspNetCore.Mvc;
using Server.Data;
using Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CGroupController : ControllerBase
    {
        private readonly ILogger<CGroupController> _logger;
        private readonly ApplicationDbContext _context;

        public CGroupController(ILogger<CGroupController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<CategoryGroup>>> GetCGroup()
        {
            try
            {
                _logger.LogInformation("Getting all category groups");
                var cgroups = await _context.CategoryGroups.ToListAsync();
                return Ok(cgroups);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting category groups");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryGroup>> GetSingleCGroup(int id)
        {
            try
            {
                _logger.LogInformation("Getting category group with ID {CategoryGroupId}", id);
                var cgroup = await _context.CategoryGroups.FirstOrDefaultAsync(h => h.Id == id);

                if (cgroup == null)
                {
                    _logger.LogWarning("Category group with ID {CategoryGroupId} not found", id);
                    return NotFound("Sorry, no category group here. :/");
                }

                return Ok(cgroup);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting category group with ID {CategoryGroupId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<List<CategoryGroup>>> CreateCGroup(CategoryGroup categoryGroup)
        {
            try
            {
                _logger.LogInformation("Creating a new category group with Title {CategoryGroupTitle}", categoryGroup.Title);
                _context.CategoryGroups.Add(categoryGroup);
                await _context.SaveChangesAsync();

                return Ok(await GetDbCGroups());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new category group");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<List<CategoryGroup>>> UpdateCGroup(CategoryGroup categoryGroup, int id)
        {
            try
            {
                _logger.LogInformation("Updating category group with ID {CategoryGroupId}", id);
                var dbCG = await _context.CategoryGroups.FirstOrDefaultAsync(cg => cg.Id == id);

                if (dbCG == null)
                {
                    _logger.LogWarning("Category group with ID {CategoryGroupId} not found for update", id);
                    return NotFound("Sorry, no category group here.");
                }

                dbCG.Title = categoryGroup.Title;
                dbCG.UpdatedBy = categoryGroup.UpdatedBy;
                dbCG.UdatedAt = categoryGroup.UdatedAt;
                await _context.SaveChangesAsync();

                return Ok(await GetDbCGroups());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating category group with ID {CategoryGroupId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<CategoryGroup>>> DeleteCGroup(int id)
        {
            try
            {
                _logger.LogInformation("Deleting category group with ID {CategoryGroupId}", id);
                var dbCG = await _context.CategoryGroups.FirstOrDefaultAsync(cg => cg.Id == id);

                if (dbCG == null)
                {
                    _logger.LogWarning("Category group with ID {CategoryGroupId} not found for deletion", id);
                    return NotFound("Sorry, no category group here.");
                }

                _context.CategoryGroups.Remove(dbCG);
                await _context.SaveChangesAsync();

                return Ok(await GetDbCGroups());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting category group with ID {CategoryGroupId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        private async Task<List<CategoryGroup>> GetDbCGroups()
        {
            try
            {
                return await _context.CategoryGroups.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching category groups from database");
                throw;
            }
        }
    }
}