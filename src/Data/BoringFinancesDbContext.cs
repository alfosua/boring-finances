using Microsoft.EntityFrameworkCore;

namespace BoringSoftware.Finances.Data
{
    public class BoringFinancesDbContext : DbContext
    {
        #region Constructor

        public BoringFinancesDbContext(DbContextOptions<BoringFinancesDbContext> options) : base(options)
        {
        }

        #endregion Constructor
    }
}
