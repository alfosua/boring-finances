using BoringSoftware.Finances.Core.Accounts;
using BoringSoftware.Finances.Core.FinancialUnits;
using BoringSoftware.Finances.Dtos.Operations;
using BoringSoftware.Finances.Dtos.Utils;
using BoringSoftware.Finances.Entities.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoringSoftware.Finances.Core.Operations
{
    public class OperationMapper : IOperationMapper
    {
        private readonly IAccountResolver _accountResolver;
        private readonly IFinancialUnitResolver _financialUnitResolver;
        private readonly IOperationEntryTypeResolver _entryTypeResolver;

        public OperationMapper(IAccountResolver accountResolver, IFinancialUnitResolver financialUnitResolver, IOperationEntryTypeResolver entryTypeResolver)
        {
            _accountResolver = accountResolver;
            _financialUnitResolver = financialUnitResolver;
            _entryTypeResolver = entryTypeResolver;
        }

        public async Task<OperationCrudFacade> MapToOperationCrudFacadeAsync(OperationEditionJson operationEditionJson, OperationCrudFacade operationCrudFacade = null)
        {
            operationCrudFacade ??= new OperationCrudFacade();

            var accountReferences = operationEditionJson.Entries.Select(x => x.Account);
            var accountReferencesLookup = await _accountResolver.ResolveManyFromAsync(accountReferences);

            var financialUnitReferences = operationEditionJson.Entries.Select(x => x.FinancialUnit);
            var financialUnitReferencesLookup = await _financialUnitResolver.ResolveManyFromAsync(financialUnitReferences);

            var operationEntryTypeReferences = operationEditionJson.Entries.Select(x => x.OperationEntryType);
            var operationEntryTypeReferencesLookup = _entryTypeResolver.ResolveManyFrom(operationEntryTypeReferences);

            operationCrudFacade.Entries = operationEditionJson.Entries?
                .Select(x => new OperationCrudEntryFacade
                {
                    Id = x.Id,
                    AccountId = accountReferencesLookup.GetValueOrDefault(x.Account),
                    Amount = x.Amount,
                    DateTime = x.DateTime,
                    FinancialUnitId = financialUnitReferencesLookup.GetValueOrDefault(x.FinancialUnit),
                    OperationEntryTypeId = operationEntryTypeReferencesLookup.GetValueOrDefault(x.OperationEntryType),
                    Notes = x.Notes,
                })
                .ToList();
            operationCrudFacade.Notes = operationEditionJson.Notes;

            return operationCrudFacade;
        }

        public OperationReadingJson MapToOperationReadingJson(OperationCrudFacade operationCrudFacade, OperationReadingJson operationReadingJson = null)
        {
            operationReadingJson ??= new OperationReadingJson();

            operationReadingJson.Id = operationCrudFacade.Id.GetValueOrDefault();
            operationReadingJson.Entries = operationCrudFacade.Entries
                .Select(x => new OperationEntryReadingJson
                {
                    Id = x.Id.GetValueOrDefault(),
                    Amount = x.Amount,
                    DateTime = x.DateTime,
                    Account = new IdHrefPair<Guid> {
                        Id = x.AccountId,
                        Href = $"/api/v0/accounts/{ x.AccountId }",
                    },
                    OperationEntryType = new IdCodeHrefGroup<byte>
                    {
                        Id = x.OperationEntryTypeId,
                        Href = $"/api/v0/operations/entries/types/{ x.OperationEntryTypeId }",
                        Code = ((OperationEntryTypeOption)x.OperationEntryTypeId).ToString(),
                    },
                    FinancialUnit = new IdHrefPair<Guid>
                    {
                        Id = x.FinancialUnitId,
                        Href = $"/api/v0/financial-units/types/{ x.FinancialUnitId }",
                    },
                    Notes = x.Notes,
                });
            operationReadingJson.Notes = operationCrudFacade.Notes;

            return operationReadingJson;
        }

        public OperationCrudFacade MapToOperationCrudFacade(Operation operation, OperationCrudFacade operationCrudFacade = null)
        {
            operationCrudFacade ??= new OperationCrudFacade();

            operationCrudFacade.Id = operation.Id;

            operationCrudFacade.Notes = operation.Notes?
                .Select(x => x.Content)
                .ToList()
                ?? new List<string>();

            operationCrudFacade.Entries = operation.Entries?
                .Select(x => new OperationCrudEntryFacade
                {
                    Id = x.Id,
                    AccountId = x.AccountId,
                    Amount = x.Amount,
                    DateTime = x.Date,
                    FinancialUnitId = x.FinancialUnitId,
                    OperationEntryTypeId = x.OperationEntryTypeId,
                    Notes = x.Notes?
                        .Select(x => x.Content)
                        .ToList()
                        ?? new List<string>(),
                })
                .ToList()
                ?? new List<OperationCrudEntryFacade>();

            return operationCrudFacade;
        }
    }
}
