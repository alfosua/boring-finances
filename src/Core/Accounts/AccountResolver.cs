using BoringSoftware.Finances.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoringSoftware.Finances.Core.Accounts
{
    public class AccountResolver : IAccountResolver
    {
        #region Dependencies

        private readonly BoringFinancesDbContext _dbContext;
        private IMemoryCache _memoryCache;

        #endregion Dependencies

        #region Constructor

        public AccountResolver(BoringFinancesDbContext dbContext, IMemoryCache memoryCache)
        {
            _dbContext = dbContext;
            _memoryCache = memoryCache;
        }

        #endregion Constructor

        #region Services

        public async Task<Guid> ResolveOneFromAsync(string stringValue)
        {
            var resultsFromStrings = await GetResultsFromStringsWithNewOnesAsync(new[] { stringValue });

            return resultsFromStrings[stringValue];
        }

        public async Task<IReadOnlyDictionary<string, Guid>> ResolveManyFromAsync(IEnumerable<string> stringValues)
        {
            var stringValueList = stringValues.ToList();

            var resultsFromStrings = await GetResultsFromStringsWithNewOnesAsync(stringValueList);

            var neededResultsQuery = resultsFromStrings.Where(x => stringValueList.Contains(x.Key));

            var results = neededResultsQuery.ToDictionary(x => x.Key, x => x.Value);

            return results;
        }

        #endregion Services

        #region Internal Processing

        public async Task<Dictionary<string, Guid>> GetResultsFromStringsWithNewOnesAsync(IEnumerable<string> stringValues)
        {
            string cacheKey = $"{ nameof(AccountResolver) }:Results:FromStrings";

            var cache = _memoryCache.GetOrCreate(key: cacheKey, factory: c => new Dictionary<string, Guid>());

            var newStringValues = stringValues.Where(x => !cache.ContainsKey(x)).ToList();

            if (newStringValues.Any())
            {
                var accountIdKebabPairsQuery = _dbContext.Accounts
                    .Where(x => newStringValues.Contains(x.Id.ToString()) || newStringValues.Contains(x.Kebab))
                    .Select(x => new { AccountId = x.Id, Kebab = x.Kebab });

                var accountIdKebabPairs = await accountIdKebabPairsQuery.ToListAsync();

                foreach (var pair in accountIdKebabPairs)
                {
                    cache[pair.AccountId.ToString()] = pair.AccountId;

                    if (!string.IsNullOrEmpty(pair.Kebab))
                    {
                        cache[pair.Kebab] = pair.AccountId;
                    }
                }

                _memoryCache.Set(cacheKey, cache);
            }

            return cache;
        }

        #endregion Internal Processing
    }
}
