using BoringFinances.Operations.Data;
using BoringFinances.Operations.Data.FinancialUnits;
using BoringFinances.Operations.WebApi.Utilities;
using Microsoft.EntityFrameworkCore;

namespace BoringFinances.Operations.WebApi.FinancialUnits;

public class FinancialUnitCruder : IFinancialUnitCruder
{
    private readonly OperationDbContext _dbContext;
    private readonly ITemporalEntityDomainer _temporalEntityDomain;
    private readonly INoteDomainer _noteDomain;
    private readonly ITagDomainer _tagDomain;
    private readonly IFinancialUnitDtoMapper _financialUnitDtoMapper;

    public FinancialUnitCruder(
        OperationDbContext dbContext,
        ITemporalEntityDomainer temporalEntityDomain,
        INoteDomainer noteDomain,
        ITagDomainer tagDomain,
        IFinancialUnitDtoMapper financialUnitDtoMapper)
    {
        _dbContext = dbContext;
        _temporalEntityDomain = temporalEntityDomain;
        _noteDomain = noteDomain;
        _tagDomain = tagDomain;
        _financialUnitDtoMapper = financialUnitDtoMapper;
    }

    public async Task<FinancialUnitDto> CreateOneFromAsync(FinancialUnitDto financialUnitDto)
    {
        var validEffectTimestamp = DateTime.UtcNow;

        var financialUnitToCreate = new FinancialUnit();
        var financialUnitTimestamped = _temporalEntityDomain.TimestampCreationTo(financialUnitToCreate, validEffectTimestamp);
        var financialUnitWithMapping = MapToFinancialUnit(financialUnitDto, financialUnitTimestamped);
        var financialUnitWithNotes = _noteDomain.DecorateWithNotesTo(financialUnitWithMapping, financialUnitDto.Notes);
        var financialUnitWithTags = await _tagDomain.DecorateWithTagsToAsync(financialUnitWithNotes, financialUnitDto.Tags);
        
        var financialUnitCreated = await _dbContext.Piping(financialUnitWithTags).AddAndSaveChangesAsync();
        var result = _financialUnitDtoMapper.CreateFinancialUnitDtoFrom(financialUnitCreated);

        return result;
    }

    public async Task<FinancialUnitDto> ReadOneByIdAsync(long financialUnitId)
    {
        var result = await _dbContext.FinancialUnits
            .Where(x => x.FinancialUnitId == financialUnitId && !x.Deleted.HasValue)
            .Select(_financialUnitDtoMapper.SelectExpression)
            .SingleOrDefaultAsync();

        if (result is null)
        {
            throw new KeyNotFoundException(nameof(financialUnitId));
        }

        return result;
    }

    public async Task<FinancialUnitDto> UpdateOneByIdFromAsync(long financialUnitId, FinancialUnitDto financialUnitDto)
    {
        var financialUnitQueried = await FindFinancialUnitAsync(financialUnitId);
        var financialUnitWithMapping = MapToFinancialUnit(financialUnitDto, financialUnitQueried);
        var financialUnitWithNotes = _noteDomain.DecorateWithNotesTo(financialUnitWithMapping, financialUnitDto.Notes);
        var financialUnitWithTags = await _tagDomain.DecorateWithTagsToAsync(financialUnitWithNotes, financialUnitDto.Tags);

        var financialUnitUpdated = await _dbContext.Piping(financialUnitWithTags).UpdateAndSaveChangesAsync();
        var result = _financialUnitDtoMapper.CreateFinancialUnitDtoFrom(financialUnitUpdated);

        return result;
    }

    public async Task<FinancialUnitDto> DeleteOneByIdAsync(long financialUnitId)
    {
        var validEffectTimestamp = DateTime.UtcNow;

        var financialUnitQueried = await FindFinancialUnitAsync(financialUnitId);
        var financialUnitTimestamped = _temporalEntityDomain.TimestampDeletionTo(financialUnitQueried, validEffectTimestamp);

        var financialUnitDeleted = await _dbContext.Piping(financialUnitTimestamped).UpdateAndSaveChangesAsync();
        var result = _financialUnitDtoMapper.CreateFinancialUnitDtoFrom(financialUnitDeleted);

        return result;
    }

    private async Task<FinancialUnit> FindFinancialUnitAsync(long financialUnitId)
    {
        var financialUnitToUpdate = await _dbContext.FinancialUnits
            .Include(x => x.Notes)
            .Include(x => x.Tags)
            .FirstOrDefaultAsync(x => x.FinancialUnitId == financialUnitId && !x.Deleted.HasValue);

        if (financialUnitToUpdate is null)
        {
            throw new KeyNotFoundException(nameof(financialUnitId));
        }

        return financialUnitToUpdate;
    }

    public FinancialUnit MapToFinancialUnit(FinancialUnitDto financialUnitDto, FinancialUnit financialUnit)
    {
        financialUnit ??= new();

        financialUnit.Name = financialUnitDto.Name;
        financialUnit.FinancialUnitTypeId = financialUnitDto.FinancialUnitTypeId;
        financialUnit.Code = financialUnitDto.Code;
        financialUnit.FractionalDigits = financialUnitDto.FractionalDigits;

        return financialUnit;
    }
}
