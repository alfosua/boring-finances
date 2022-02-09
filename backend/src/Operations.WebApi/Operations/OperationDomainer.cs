using BoringFinances.Operations.Data.Operations;
using BoringFinances.Operations.WebApi.Utilities;

namespace BoringFinances.Operations.WebApi.Operations;

public class OperationDomainer : IOperationDomainer
{
    private readonly ITemporalEntityDomainer _temporalEntityDomain;
    private readonly INoteDomainer _noteDomain;

    public OperationDomainer(ITemporalEntityDomainer temporalEntityDomain, INoteDomainer noteDomain)
    {
        _temporalEntityDomain = temporalEntityDomain;
        _noteDomain = noteDomain;
    }

    public Operation DecorateWithEntriesTo(Operation operation, IEnumerable<OperationEntryDto> operationEntryDtos, DateTime effectTimestamp)
    {
        var entriesLookup = operation.Entries.ToDictionary(x => x.OperationEntryId, x => x);

        var entryIdsFromDtos = operationEntryDtos
            .Where(x => x.OperationEntryId.HasValue)
            .Select(x => x.OperationEntryId!.Value)
            .ToList();

        var persistingEntries = operationEntryDtos
            .Select(x => ResolveOperationEntryFrom(x, entriesLookup, effectTimestamp));

        var deletedEntries = operation.Entries
            .Where(x => !entryIdsFromDtos.Contains(x.OperationEntryId))
            .Select(x => _temporalEntityDomain.TimestampDeletionTo(x, effectTimestamp));

        operation.Entries = persistingEntries
            .Concat(deletedEntries)
            .ToList();

        return operation;
    }

    private OperationEntry ResolveOperationEntryFrom(OperationEntryDto operationEntryDto, IReadOnlyDictionary<int, OperationEntry> lookup, DateTime effectTimestamp)
    {
        var existingEntry = operationEntryDto.OperationEntryId is not null
            ? lookup.GetValueOrDefault(operationEntryDto.OperationEntryId.Value)
            : null;

        var targetEntry = existingEntry ?? CreateOperationEntryOn(effectTimestamp);
        var targetVersion = targetEntry.Versions.First();
        var entryWithEffect = _temporalEntityDomain.TimestampEffectTo(targetEntry, targetVersion, effectTimestamp);
        var entryWithMapping = MapToOperationEntry(operationEntryDto, entryWithEffect);
        var entryWithNotes = _noteDomain.DecorateWithNotesTo(entryWithMapping, operationEntryDto.Notes);

        return entryWithNotes;
    }

    private OperationEntry MapToOperationEntry(OperationEntryDto operationEntryDto, OperationEntry? operationEntry = null)
    {
        operationEntry ??= new();

        var version = operationEntry.Versions.First();

        version.OperationId = operationEntry.OperationId;
        version.OperationEntryId = operationEntry.OperationEntryId;
        version.AccountId = operationEntryDto.AccountId;
        version.FinancialUnitId = operationEntryDto.FinancialUnitId;
        version.OperationActionId = operationEntryDto.OperationActionId;
        version.Amount = operationEntryDto.Amount;
        version.DateTime = operationEntryDto.DateTime;

        return operationEntry;
    }

    private OperationEntry CreateOperationEntryOn(DateTime dateTime)
    {
        var operationEntry = new OperationEntry();
        var timestamped = _temporalEntityDomain.TimestampCreationTo(operationEntry, dateTime);
        return timestamped;
    }
}
