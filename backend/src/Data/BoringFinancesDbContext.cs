using BoringSoftware.Finances.Entities.Accounts;
using BoringSoftware.Finances.Entities.FinancialUnits;
using BoringSoftware.Finances.Entities.Operations;
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

        #region DbSets

        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountType> AccountTypes { get; set; }

        public DbSet<FinancialUnit> FinancialUnits { get; set; }
        public DbSet<FinancialUnitType> FinancialUnitTypes { get; set; }

        public DbSet<Operation> Operations { get; set; }
        public DbSet<OperationNote> OperationNotes { get; set; }
        public DbSet<OperationEntry> OperationEntries { get; set; }
        public DbSet<OperationEntryType> OperationEntryTypes { get; set; }
        public DbSet<OperationEntryNote> OperationEntryNotes { get; set; }

        #endregion DbSets

        #region Model Creation

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountType>(builder =>
            {
                AccountType[] accountTypes = new AccountType[]
                {
                    new AccountType { Id = (byte)AccountTypeOption.Equity,    Code = AccountTypeOption.Equity.ToString()    },
                    new AccountType { Id = (byte)AccountTypeOption.Good,      Code = AccountTypeOption.Good.ToString()      },
                    new AccountType { Id = (byte)AccountTypeOption.Liability, Code = AccountTypeOption.Liability.ToString() },
                    new AccountType { Id = (byte)AccountTypeOption.Income,    Code = AccountTypeOption.Income.ToString()    },
                    new AccountType { Id = (byte)AccountTypeOption.Expense,   Code = AccountTypeOption.Expense.ToString()   },
                    new AccountType { Id = (byte)AccountTypeOption.Trading,   Code = AccountTypeOption.Trading.ToString()   },
                };

                builder.HasData(accountTypes);
            });

            modelBuilder.Entity<FinancialUnitType>(builder =>
            {
                FinancialUnitType[] financialUnitTypes = new FinancialUnitType[]
                {
                    new FinancialUnitType { Id = (byte)FinancialUnitTypeOption.Currency, Code = FinancialUnitTypeOption.Currency.ToString() },
                    new FinancialUnitType { Id = (byte)FinancialUnitTypeOption.Other,    Code = FinancialUnitTypeOption.Other.ToString()    },
                };

                builder.HasData(financialUnitTypes);
            });

            modelBuilder.Entity<OperationEntryType>(builder =>
            {
                OperationEntryType[] operationEntryTypes = new OperationEntryType[]
                {
                    new OperationEntryType { Id = (byte)OperationEntryTypeOption.Debit,  Code = OperationEntryTypeOption.Debit.ToString()  },
                    new OperationEntryType { Id = (byte)OperationEntryTypeOption.Credit, Code = OperationEntryTypeOption.Credit.ToString() },
                };

                builder.HasData(operationEntryTypes);
            });

            base.OnModelCreating(modelBuilder);
        }

        #endregion Model Creation
    }
}
