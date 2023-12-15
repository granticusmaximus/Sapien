using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Server.Data;
using Shared.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly ILogger<ItemController> _logger;
        private readonly ApplicationDbContext _context;

        public ItemController(ILogger<ItemController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Item>>> GetItems()
        {
            try
            {
                _logger.LogInformation("Getting all items");
                var items = await _context.Items
                    .Include(it => it.Category)
                    .Include(it => it.Category.CategoryGroup)
                    .Include(it => it.Unit).ToListAsync();
                return Ok(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting items");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Item>> GetSingleItems(int id)
        {
            try
            {
                _logger.LogInformation("Getting item with ID {ItemId}", id);
                var item = await _context.Items
                    .Include(it => it.Category)
                    .Include(it => it.Category.CategoryGroup)
                    .Include(it => it.Unit)
                    .FirstOrDefaultAsync(it => it.Id == id);

                if (item == null)
                {
                    _logger.LogWarning("Item with ID {ItemId} not found", id);
                    return NotFound("Sorry, there are no items!");
                }

                return Ok(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting item with ID {ItemId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<List<Item>>> CreateItem(Item item)
        {
            try
            {
                _logger.LogInformation("Creating a new item with Title {ItemTitle}", item.Title);
                item.Category = null;
                item.Unit = null;
                _context.Items.Add(item);
                await _context.SaveChangesAsync();

                return Ok(await GetDbItems());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new item");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<List<Item>>> UpdateItem(Item item, int id)
        {
            try
            {
                _logger.LogInformation("Updating item with ID {ItemId}", id);
                var dbItem = await _context.Items
                    .Include(it => it.Category)
                    .Include(it => it.Category.CategoryGroup)
                    .Include(it => it.Unit)
                    .FirstOrDefaultAsync(it => it.Id == id);

                if (dbItem == null)
                {
                    _logger.LogWarning("Item with ID {ItemId} not found for update", id);
                    return NotFound("Sorry, but no Item for you");

                }

                // Update item properties
                // ...

                await _context.SaveChangesAsync();
                return Ok(await GetDbItems());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating item with ID {ItemId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Item>>> DeleteItem(int id)
        {
            try
            {
                _logger.LogInformation("Deleting item with ID {ItemId}", id);
                var dbItem = await _context.Items
                    .Include(it => it.Category)
                    .Include(it => it.Category.CategoryGroup)
                    .Include(it => it.Unit)
                    .FirstOrDefaultAsync(it => it.Id == id);

                if (dbItem == null)
                {
                    _logger.LogWarning("Item with ID {ItemId} not found for deletion", id);
                    return NotFound("Sorry, but no Item for you");
                }

                _context.Items.Remove(dbItem);
                await _context.SaveChangesAsync();
                return Ok(await GetDbItems());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting item with ID {ItemId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        private async Task<List<Item>> GetDbItems()
        {
            try
            {
                return await _context.Items
                    .Include(it => it.Category)
                    .Include(it => it.Category.CategoryGroup)
                    .Include(it => it.Unit)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching items from database");
                throw;
            }
        }
    }
}