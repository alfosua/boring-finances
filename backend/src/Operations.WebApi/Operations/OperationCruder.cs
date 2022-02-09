using BoringFinances.Operations.Data;
using BoringFinances.Operations.Data.Operations;
using BoringFinances.Operations.WebApi.Utilities;
using Microsoft.EntityFrameworkCore;

namespace BoringFinances.Operations.WebApi.Operations;

public class OperationCruder : IOperationCruder
{
    private readonly OperationDbContext _dbContext;
    private readonly IOperationDomainer _operationDomain;
    private readonly ITemporalEntityDomainer _temporalEntityDomain;
    private readonly INoteDomainer _noteDomain;
    private readonly ITagDomainer _tagDomain;
    private readonly IOperationDtoMapper _operationDtoMapper;

    public OperationCruder(
        OperationDbContext dbContext,
        IOperationDomainer operationDomain,
        ITemporalEntityDomainer temporalEntityDomain,
        INoteDomainer noteDomain,
        ITagDomainer tagDomain,
        IOperationDtoMapper operationDtoMapper)
    {
        _dbContext = dbContext;
        _operationDomain = operationDomain;
        _temporalEntityDomain = temporalEntityDomain;
        _noteDomain = noteDomain;
        _tagDomain = tagDomain;
        _operationDtoMapper = operationDtoMapper;
    }

    public async Task<OperationDto> CreateOneFromAsync(OperationDto operationDto)
    {
        var validEffectTimestamp = DateTime.UtcNow;

        var operationToCreate = new Operation();
        var operationTimestamped = _temporalEntityDomain.TimestampCreationTo(operationToCreate, validEffectTimestamp);
        var operationWithEntries = _operationDomain.DecorateWithEntriesTo(operationTimestamped, operationDto.Entries, validEffectTimestamp);
        var operationWithNotes = _noteDomain.DecorateWithNotesTo(operationWithEntries, operationDto.Notes);
        var operationWithTags = await _tagDomain.DecorateWithTagsToAsync(operationWithNotes, operationDto.Tags);
        
        var operationCreated = await _dbContext.Piping(operationWithTags).AddAndSaveChangesAsync();
        var result = _operationDtoMapper.CreateOperationDtoFrom(operationCreated);

        return result;
    }

    public async Task<OperationDto> ReadOneByIdAsync(long operationId)
    {
        var result = await _dbContext.Operations
            .Where(x => x.OperationId == operationId && !x.Deleted.HasValue)
            .Select(_operationDtoMapper.MostRecentVersionMappingExpression)
            .SingleOrDefaultAsync();

        if (result is null)
        {
            throw new KeyNotFoundException(nameof(operationId));
        }

        return result;
    }

    public async Task<OperationDto> UpdateOneByIdFromAsync(long operationId, OperationDto operationDto)
    {
        var validEffectTimestamp = DateTime.UtcNow;

        var operationQueried = await FindOperationAsync(operationId);
        var operationWithEntries = _operationDomain.DecorateWithEntriesTo(operationQueried, operationDto.Entries, validEffectTimestamp);
        var operationWithNotes = _noteDomain.DecorateWithNotesTo(operationWithEntries, operationDto.Notes);
        var operationWithTags = await _tagDomain.DecorateWithTagsToAsync(operationWithNotes, operationDto.Tags);

        var operationToUpdate = _dbContext.Piping(operationWithTags)
            .AddRangeFromInto(x => x.Entries.SelectMany(x => x.Versions));
        var operationUpdated = await _dbContext.Piping(operationToUpdate).UpdateAndSaveChangesAsync();
        var result = _operationDtoMapper.CreateOperationDtoFrom(operationUpdated);

        return result;
    }

    public async Task<OperationDto> DeleteOneByIdAsync(long operationId)
    {
        var validEffectTimestamp = DateTime.UtcNow;

        var operationQueried = await FindOperationAsync(operationId);
        var operationTimestamped = _temporalEntityDomain.TimestampDeletionTo(operationQueried, validEffectTimestamp);

        var operationDeleted = await _dbContext.Piping(operationTimestamped).UpdateAndSaveChangesAsync();
        var result = _operationDtoMapper.CreateOperationDtoFrom(operationDeleted);

        return result;
    }

    private async Task<Operation> FindOperationAsync(long operationId)
    {
        var operationToUpdate = await _dbContext.Operations
            .Include(x => x.Notes)
            .Include(x => x.Tags)
            .Include(x => x.Entries
                .Where(x => !x.Deleted.HasValue))
            .ThenInclude(x => x.Notes)
            .FirstOrDefaultAsync(x => x.OperationId == operationId && !x.Deleted.HasValue);

        if (operationToUpdate is null)
        {
            throw new KeyNotFoundException(nameof(operationId));
        }

        return operationToUpdate;
    }
}
