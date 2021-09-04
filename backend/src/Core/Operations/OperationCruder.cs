using BoringSoftware.Finances.Data;
using BoringSoftware.Finances.Entities.Operations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoringSoftware.Finances.Core.Operations
{
    public class OperationCruder : IOperationCruder
    {
        #region Dependencies

        private readonly BoringFinancesDbContext _dbContext;
        private readonly IOperationMapper _mapper;

        #endregion Dependencies

        #region Constructor

        public OperationCruder(BoringFinancesDbContext dbContext, IOperationMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        #endregion Constructor

        #region Services

        public async Task<OperationCrudFacade> CreateOneFromAsync(OperationCrudFacade facade)
        {
            var operationToCreate = new Operation { Id = Guid.NewGuid() };

            var operationWithMapping = MapToOperation(facade, operationToCreate);

            var operationToAdd = operationWithMapping;

            _dbContext.Add(operationWithMapping);

            await _dbContext.SaveChangesAsync();

            var result = _mapper.MapToOperationCrudFacade(operationToAdd);

            return result;
        }

        public async Task<OperationCrudFacade> ReadOneByIdAsync(Guid operationId)
        {
            var operationToDelete = await _dbContext.Operations
                .AsNoTracking()
                .Include(x => x.Entries).ThenInclude(x => x.Notes)
                .Include(x => x.Notes)
                .FirstOrDefaultAsync(x => x.Id == operationId);

            if (operationToDelete is null)
            {
                throw new KeyNotFoundException("The operation does not exist");
            }

            var result = _mapper.MapToOperationCrudFacade(operationToDelete);

            return result;
        }

        public async Task<OperationCrudFacade> UpdateOneByIdFromAsync(Guid operationId, OperationCrudFacade facade)
        {
            var operationToModify = await _dbContext.Operations
                .Include(x => x.Entries).ThenInclude(x => x.Notes)
                .Include(x => x.Notes)
                .FirstOrDefaultAsync(x => x.Id == operationId);

            if (operationToModify is null)
            {
                throw new KeyNotFoundException("The operation does not exist");
            }

            var operationWithMapping = MapToOperation(facade, operationToModify);

            var operationToUpdate = operationWithMapping;

            await _dbContext.SaveChangesAsync();

            var result = _mapper.MapToOperationCrudFacade(operationToUpdate);

            return result;
        }

        public async Task<OperationCrudFacade> DeleteOneByIdAsync(Guid operationId)
        {
            var operationToDelete = await _dbContext.Operations
                .Include(x => x.Entries).ThenInclude(x => x.Notes)
                .Include(x => x.Notes)
                .FirstOrDefaultAsync(x => x.Id == operationId);

            if (operationToDelete is null)
            {
                throw new KeyNotFoundException("The operation does not exist");
            }

            var result = _mapper.MapToOperationCrudFacade(operationToDelete);

            _dbContext.Operations.Remove(operationToDelete);

            await _dbContext.SaveChangesAsync();

            return result;
        }

        #endregion Services

        #region Processing

        private Operation MapToOperation(OperationCrudFacade operationCrudFacade, Operation operation = null)
        {
            operation ??= new Operation();

            operation.Notes = operationCrudFacade.Notes?
                .Select(noteContent => new OperationNote
                {
                    Content = noteContent,
                })
                .ToList()
                ?? new List<OperationNote>();

            var existingOperationEntriesById = operation.Entries?.ToDictionary(x => x?.Id, x => x);

            operation.Entries = operationCrudFacade.Entries?
                .Select(x => MapToOperationEntry(x, existingOperationEntriesById?.GetValueOrDefault(x.Id.GetValueOrDefault())))
                .ToList()
                ?? new List<OperationEntry>();

            return operation;
        }

        private OperationEntry MapToOperationEntry(OperationCrudEntryFacade operationCrudEntryFacade, OperationEntry operationEntry = null)
        {
            operationEntry ??= new OperationEntry();

            operationEntry.AccountId = operationCrudEntryFacade.AccountId;
            operationEntry.Amount = operationCrudEntryFacade.Amount;
            operationEntry.Date = operationCrudEntryFacade.DateTime;
            operationEntry.FinancialUnitId = operationCrudEntryFacade.FinancialUnitId;
            operationEntry.OperationEntryTypeId = operationCrudEntryFacade.OperationEntryTypeId;
            operationEntry.Notes = operationCrudEntryFacade.Notes?
                .Select(noteContent => new OperationEntryNote
                {
                    Content = noteContent,
                })
                .ToList()
                ?? new List<OperationEntryNote>();

            return operationEntry;
        }

        #endregion Processing
    }
}
