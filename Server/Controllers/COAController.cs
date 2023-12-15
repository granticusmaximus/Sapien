using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Shared.Models;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class COAController : ControllerBase
    {
        private readonly ILogger<COAController> _logger;
        private readonly ApplicationDbContext _context;

        public COAController(ILogger<COAController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<ChartOfAccount>>> GetCOAs()
        {
            try
            {
                _logger.LogInformation("Getting all charts of accounts");
                var coas = await _context.chartOfAccounts.ToListAsync();
                return Ok(coas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting charts of accounts");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ChartOfAccount>> GetSingleCOA(int id)
        {
            try
            {
                _logger.LogInformation("Getting chart of account with ID {ChartOfAccountId}", id);
                var coa = await _context.chartOfAccounts.FirstOrDefaultAsync(u => u.Id == id);

                if (coa == null)
                {
                    _logger.LogWarning("Chart of account with ID {ChartOfAccountId} not found", id);
                    return NotFound("Sorry, but no Chart of Account for you");
                }

                return Ok(coa);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting chart of account with ID {ChartOfAccountId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<List<ChartOfAccount>>> CreateCOA(ChartOfAccount chartOfAccount)
        {
            try
            {
                _logger.LogInformation("Creating a new chart of account with Name {COAName}", chartOfAccount.Name);
                _context.chartOfAccounts.Add(chartOfAccount);
                await _context.SaveChangesAsync();

                return Ok(await GetDbCOAs());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new chart of account");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<List<ChartOfAccount>>> UpdateCOA(ChartOfAccount chartOfAccount, int id)
        {
            try
            {
                _logger.LogInformation("Updating chart of account with ID {ChartOfAccountId}", id);
                var dbCOA = await _context.chartOfAccounts.FirstOrDefaultAsync(coa => coa.Id == id);

                if (dbCOA == null)
                {
                    _logger.LogWarning("Chart of account with ID {ChartOfAccountId} not found for update", id);
                    return NotFound("Sorry, but no Chart of Account for you");
                }

                dbCOA.Name = chartOfAccount.Name;
                dbCOA.Type = chartOfAccount.Type;
                dbCOA.UdatedAt = chartOfAccount.UdatedAt;
                dbCOA.UpdatedBy = chartOfAccount.UpdatedBy;
                await _context.SaveChangesAsync();

                return Ok(await GetDbCOAs());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating chart of account with ID {ChartOfAccountId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<ChartOfAccount>>> DeleteCOA(int id)
        {
            try
            {
                _logger.LogInformation("Deleting chart of account with ID {ChartOfAccountId}", id);
                var dbCOA = await _context.chartOfAccounts.FirstOrDefaultAsync(coa => coa.Id == id);

                if (dbCOA == null)
                {
                    _logger.LogWarning("Chart of account with ID {ChartOfAccountId} not found for deletion", id);
                    return NotFound("Sorry, but no Chart of Account for you");

                }

                _context.chartOfAccounts.Remove(dbCOA);
                await _context.SaveChangesAsync();

                return Ok(await GetDbCOAs());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting chart of account with ID {ChartOfAccountId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        private async Task<List<ChartOfAccount>> GetDbCOAs()
        {
            try
            {
                return await _context.chartOfAccounts.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching charts of accounts from database");
                throw;
            }
        }
    }

}
