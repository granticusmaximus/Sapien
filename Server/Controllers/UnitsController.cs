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
    public class UnitsController : ControllerBase
    {
        private readonly ILogger<UnitsController> _logger;
        private readonly ApplicationDbContext _context;

        public UnitsController(ILogger<UnitsController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Unit>>> GetUnit()
        {
            try
            {
                _logger.LogInformation("Getting all units");
                var units = await _context.Units.ToListAsync();
                return Ok(units);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting units");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Unit>> GetSingleUnit(int id)
        {
            try
            {
                _logger.LogInformation("Getting unit with ID {UnitId}", id);
                var unit = await _context.Units.FirstOrDefaultAsync(u => u.Id == id);

                if (unit == null)
                {
                    _logger.LogWarning("Unit with ID {UnitId} not found", id);
                    return NotFound("Sorry, no unit here. :/");
                }

                return Ok(unit);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting unit with ID {UnitId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<List<Unit>>> CreateUnit(Unit unit)
        {
            try
            {
                _logger.LogInformation("Creating a new unit with Type {UnitType}", unit.Type);
                _context.Units.Add(unit);
                await _context.SaveChangesAsync();
                return Ok(await GetDbUnits());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new unit");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<List<Unit>>> UpdateUnit(Unit unit, int id)
        {
            try
            {
                _logger.LogInformation("Updating unit with ID {UnitId}", id);
                var dbUnit = await _context.Units.FirstOrDefaultAsync(u => u.Id == id);

                if (dbUnit == null)
                {
                    _logger.LogWarning("Unit with ID {UnitId} not found for update", id);
                    return NotFound("Sorry, but no unit for you");
                }

                dbUnit.Type = unit.Type;
                dbUnit.UpdatedBy = unit.UpdatedBy;
                dbUnit.UdatedAt = unit.UdatedAt;

                await _context.SaveChangesAsync();
                return Ok(await GetDbUnits());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating unit with ID {UnitId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Unit>>> DeleteUnit(int id)
        {
            try
            {
                _logger.LogInformation("Deleting unit with ID {UnitId}", id);
                var dbUnit = await _context.Units.FirstOrDefaultAsync(u => u.Id == id);

                if (dbUnit == null)
                {
                    _logger.LogWarning("Unit with ID {UnitId} not found for deletion", id);
                    return NotFound("Sorry, but no unit for you");
                }

                _context.Units.Remove(dbUnit);
                await _context.SaveChangesAsync();
                return Ok(await GetDbUnits());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting unit with ID {UnitId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        private async Task<List<Unit>> GetDbUnits()
        {
            try
            {
                return await _context.Units.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching units from database");
                throw;
            }
        }
    }
}