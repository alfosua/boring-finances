using BoringFinances.Operations.Data.Operations;
using System.Linq.Expressions;

namespace BoringFinances.Operations.WebApi.Operations;

public class OperationDtoMapper : IOperationDtoMapper
{
    public Expression<Func<Operation, OperationDto>> MostRecentVersionMappingExpression =>
        x => new OperationDto
        {
            OperationId = x.OperationId,
            Entries = x.Entries
                .Where(x => !x.Deleted.HasValue)
                .Select(x => new
                {
                    Entry = x,
                    Version = x.Versions
                        .OrderByDescending(x => x.Effective)
                        .First(),
                })
                .Select(x => new OperationEntryDto
                {
                    OperationEntryId = x.Entry.OperationEntryId,
                    AccountId = x.Version.AccountId,
                    FinancialUnitId = x.Version.FinancialUnitId,
                    OperationActionId = x.Version.OperationActionId,
                    Amount = x.Version.Amount,
                    DateTime = x.Version.DateTime,
                    Notes = x.Entry.Notes
                        .Select(x => x.Text),
                }),
            Notes = x.Notes
                .Select(x => x.Text),
            Tags = x.Tags
                .Select(x => x.Name)
                .ToList(),
        };

    public OperationDto CreateOperationDtoFrom(Operation operation)
    {
        return new()
        {
            OperationId = operation.OperationId,
            Entries = operation.Entries
                .Select(x => new { Entry = x, Version = x.Versions.First() })
                .Select(x => new OperationEntryDto
                {
                    OperationEntryId = x.Entry.OperationEntryId,
                    AccountId = x.Version.AccountId,
                    FinancialUnitId = x.Version.FinancialUnitId,
                    OperationActionId = x.Version.OperationActionId,
                    Amount = x.Version.Amount,
                    DateTime = x.Version.DateTime,
                    Notes = x.Entry.Notes
                        .Select(x => x.Text),
                }),
            Notes = operation.Notes
                .Select(x => x.Text),
            Tags = operation.Tags
                .Select(x => x.Name)
                .ToList(),
        };
    }
}
