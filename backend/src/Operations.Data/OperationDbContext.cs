using BoringFinances.Operations.Data.Accounts;
using BoringFinances.Operations.Data.Annotations;
using BoringFinances.Operations.Data.FinancialUnits;
using BoringFinances.Operations.Data.Operations;
using Microsoft.EntityFrameworkCore;

namespace BoringFinances.Operations.Data;
public class OperationDbContext : DbContext
{
    public OperationDbContext(DbContextOptions<OperationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Note> Notes => Set<Note>();
    public DbSet<Tag> Tags => Set<Tag>();

    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<AccountType> AccountTypes => Set<AccountType>();
    
    public DbSet<FinancialUnit> FinancialUnits => Set<FinancialUnit>();
    public DbSet<FinancialUnitType> FinancialUnitTypes => Set<FinancialUnitType>();

    public DbSet<Operation> Operations => Set<Operation>();
    public DbSet<OperationEntry> OperationEntries => Set<OperationEntry>();
    public DbSet<OperationEntryVersion> OperationEntryVersions => Set<OperationEntryVersion>();
    public DbSet<OperationAction> OperationActions => Set<OperationAction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Note>()
            .Property(x => x.NoteId).ValueGeneratedOnAdd();

        modelBuilder.Entity<Tag>()
            .Property(x => x.TagId).ValueGeneratedOnAdd();
        modelBuilder.Entity<Tag>()
            .HasIndex(x => x.Name)
            .IsUnique();

        modelBuilder.Entity<Account>()
            .Property(x => x.AccountId).ValueGeneratedOnAdd();
        modelBuilder.Entity<Account>()
            .HasIndex(x => x.Code)
            .IsUnique();

        modelBuilder.Entity<FinancialUnit>()
            .Property(x => x.FinancialUnitId).ValueGeneratedOnAdd();
        modelBuilder.Entity<FinancialUnit>()
            .HasIndex(x => x.Code)
            .IsUnique();

        modelBuilder.Entity<Operation>()
            .Property(x => x.OperationId).ValueGeneratedOnAdd();

        modelBuilder.Entity<OperationEntry>()
            .Property(x => x.OperationEntryId).ValueGeneratedOnAdd();
        modelBuilder.Entity<OperationEntry>()
            .HasKey(x => new { x.OperationId, x.OperationEntryId });

        modelBuilder.Entity<OperationEntryVersion>()
            .HasKey(x => new { x.OperationId, x.OperationEntryId, x.Effective });

        modelBuilder.Entity<AccountType>().HasData(new AccountType[]
        {
            new() { AccountTypeId = (byte)AccountTypeOptions.Equity,    Code = nameof(AccountTypeOptions.Equity)    },
            new() { AccountTypeId = (byte)AccountTypeOptions.Asset,     Code = nameof(AccountTypeOptions.Asset)     },
            new() { AccountTypeId = (byte)AccountTypeOptions.Liability, Code = nameof(AccountTypeOptions.Liability) },
            new() { AccountTypeId = (byte)AccountTypeOptions.Income,    Code = nameof(AccountTypeOptions.Income)    },
            new() { AccountTypeId = (byte)AccountTypeOptions.Expense,   Code = nameof(AccountTypeOptions.Expense)   },
            new() { AccountTypeId = (byte)AccountTypeOptions.Exchange,  Code = nameof(AccountTypeOptions.Exchange)  },
        });

        modelBuilder.Entity<FinancialUnitType>().HasData(new FinancialUnitType[]
        {
            new() { FinancialUnitTypeId = (byte)FinancialUnitTypeOptions.Other,          Code = nameof(FinancialUnitTypeOptions.Other)          },
            new() { FinancialUnitTypeId = (byte)FinancialUnitTypeOptions.FiatCurrency,   Code = nameof(FinancialUnitTypeOptions.FiatCurrency)   },
            new() { FinancialUnitTypeId = (byte)FinancialUnitTypeOptions.CryptoCurrency, Code = nameof(FinancialUnitTypeOptions.CryptoCurrency) },
        });

        modelBuilder.Entity<OperationAction>().HasData(new OperationAction[]
        {
            new() { OperationActionId = (byte)OperationActionOptions.Debit,  Code = nameof(OperationActionOptions.Debit)  },
            new() { OperationActionId = (byte)OperationActionOptions.Credit, Code = nameof(OperationActionOptions.Credit) },
        });
    }
}
