using Shared.Models;

namespace Client.Services
{
    public interface ICOAService
    {
        List<ChartOfAccount> ChartOfAccounts { get; set; }

        Task GetCOA();

        Task<ChartOfAccount> GetCOAById(int id);

        Task CreateCOA(ChartOfAccount chartOfAccount);

        Task UpdateCOA(ChartOfAccount chartOfAccount);

        Task DeleteCOA(int id);
    }
}