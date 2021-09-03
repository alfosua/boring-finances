using BoringSoftware.Finances.Data;
using BoringSoftware.Finances.Entities.FinancialUnits;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BoringSoftware.Finances.Core.FinancialUnits
{
    public class FinancialUnitCruder : IFinancialUnitCruder
    {
        #region Dependencies

        private readonly BoringFinancesDbContext _dbContext;
        private readonly IFinancialUnitMapper _mapper;

        #endregion Dependencies

        #region Constructor

        public FinancialUnitCruder(BoringFinancesDbContext dbContext, IFinancialUnitMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        #endregion Constructor

        #region Services

        public async Task<FinancialUnitCrudFacade> CreateOneFromAsync(FinancialUnitCrudFacade facade)
        {
            var financialUnitToCreate = new FinancialUnit { Id = Guid.NewGuid() };

            var financialUnitWithMapping = MapToFinancialUnit(facade, financialUnitToCreate);

            var financialUnitToAdd = financialUnitWithMapping;

            var financialUnitAddedInDb = await _dbContext.AddAndSaveChangesAsync(financialUnitToAdd);

            var result = _mapper.MapToFinancialUnitCrudFacade(financialUnitAddedInDb);

            return result;
        }

        public async Task<FinancialUnitCrudFacade> ReadOneByIdASync(Guid financialUnitId)
        {
            var financialUnitToRead = await _dbContext.FinancialUnits
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == financialUnitId);

            if (financialUnitToRead is null)
            {
                throw new KeyNotFoundException("Financial unit does not exist");
            }

            var result = _mapper.MapToFinancialUnitCrudFacade(financialUnitToRead);

            return result;
        }

        public async Task<FinancialUnitCrudFacade> UpdateOneByIdFromAsync(Guid financialUnitId, FinancialUnitCrudFacade facade)
        {
            var financialUnitToModify = await _dbContext.FinancialUnits
                .FirstOrDefaultAsync(x => x.Id == financialUnitId);

            if (financialUnitToModify is null)
            {
                throw new KeyNotFoundException("Financial unit does not exist");
            }

            var financialUnitWithMapping = MapToFinancialUnit(facade, financialUnitToModify);

            var financialUnitToUpdate = financialUnitWithMapping;

            var financialUnitUpdatedIndb = await _dbContext.PassAndSaveChangesAsync(financialUnitToUpdate);

            var result = _mapper.MapToFinancialUnitCrudFacade(financialUnitUpdatedIndb);

            return result;
        }

        public async Task<FinancialUnitCrudFacade> DeleteOneByIdASync(Guid financialUnitId)
        {
            var financialUnitToDelete = await _dbContext.FinancialUnits
                .FirstOrDefaultAsync(x => x.Id == financialUnitId);

            if (financialUnitToDelete is null)
            {
                throw new KeyNotFoundException("Financial unit does not exist");
            }

            var result = _mapper.MapToFinancialUnitCrudFacade(financialUnitToDelete);

            var financialUnitDeletedInDb = await _dbContext.DeleteAndSaveChangesAsync(financialUnitToDelete);

            return result;
        }

        #endregion Services

        #region Processing

        private FinancialUnit MapToFinancialUnit(FinancialUnitCrudFacade financialUnitCrudFacade, FinancialUnit financialUnit = null)
        {
            financialUnit ??= new();

            financialUnit.Kebab = financialUnitCrudFacade.Kebab;
            financialUnit.FinancialUnitTypeId = financialUnitCrudFacade.FinancialUnitTypeId;

            return financialUnit;
        }

        #endregion Processing
    }
}
