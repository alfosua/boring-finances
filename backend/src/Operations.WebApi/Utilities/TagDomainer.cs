using BoringFinances.Operations.Data;
using BoringFinances.Operations.Data.Annotations;
using Microsoft.EntityFrameworkCore;

namespace BoringFinances.Operations.WebApi.Utilities;

public class TagDomainer : ITagDomainer
{
    private readonly OperationDbContext _dbContext;

    public TagDomainer(OperationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TEntity> DecorateWithTagsToAsync<TEntity>(TEntity entity, IEnumerable<string> tagNames)
        where TEntity : IHasTags
    {
        if (!tagNames.Any())
        {
            return entity;
        }

        var tagNamesList = tagNames.Select(x => x.ToLowerInvariant()).ToList();

        var currentTagNames = entity.Tags
            .Select(x => x.Name)
            .ToList();

        var persistingTags = entity.Tags
            .Where(x => tagNamesList.Contains(x.Name))
            .ToList();

        var targetTagNamesList = tagNames
            .Where(x => !currentTagNames.Contains(x))
            .ToList();

        var targetExistingTags = await _dbContext.Tags
            .Where(x => targetTagNamesList.Contains(x.Name))
            .ToListAsync();
        var targetExistingTagNamesSet = targetExistingTags.Select(x => x.Name).ToHashSet();

        var targetNewTags = targetTagNamesList
            .Where(x => !targetExistingTagNamesSet.Contains(x))
            .Select(x => new Tag { Name = x });

        var entityTags = persistingTags
            .Concat(targetExistingTags)
            .Concat(targetNewTags)
            .ToList();

        entity.Tags = entityTags;

        return entity;
    }
}
