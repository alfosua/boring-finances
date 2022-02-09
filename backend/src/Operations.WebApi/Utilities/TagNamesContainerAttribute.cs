using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace BoringFinances.Operations.WebApi.Utilities;

public class TagNamesContainerAttribute : ValidationAttribute
{
    public TagNamesContainerAttribute()
    {
    }

    public string GetErrorMessage() =>
        $"A tag can only have letters, numbers, underscores, dashes and whitespaces.";

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var tagNames = (ICollection<string>)value!;
        var matchTries = tagNames.Select((x, i) => new { Index = i, Match = Regex.IsMatch(x, @"^^[A-Za-z0-9-_ ]*$") });
        var unmatches = matchTries.Where(x => !x.Match);

        if (unmatches.Any())
        {
            return new ValidationResult(GetErrorMessage(), unmatches.Select(x => $"[{x.Index}]"));
        }

        return ValidationResult.Success;
    }
}
