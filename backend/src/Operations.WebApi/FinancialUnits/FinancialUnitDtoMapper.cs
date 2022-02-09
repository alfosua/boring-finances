using BoringFinances.Operations.Data.FinancialUnits;
using System.Linq.Expressions;

namespace BoringFinances.Operations.WebApi.FinancialUnits;

public class FinancialUnitDtoMapper : IFinancialUnitDtoMapper
{
    public Expression<Func<FinancialUnit, FinancialUnitDto>> SelectExpression =>
        x => new FinancialUnitDto
        {
            FinancialUnitId = x.FinancialUnitId,
            Name = x.Name,
            FinancialUnitTypeId = x.FinancialUnitTypeId,
            Code = x.Code,
            FractionalDigits = x.FractionalDigits,
            Notes = x.Notes
                .Select(x => x.Text),
            Tags = x.Tags
                .Select(x => x.Name)
                .ToList(),
        };

    public FinancialUnitDto CreateFinancialUnitDtoFrom(FinancialUnit account)
    {
        return new()
        {
            FinancialUnitId = account.FinancialUnitId,
            Name = account.Name,
            FinancialUnitTypeId = account.FinancialUnitTypeId,
            Code = account.Code,
            FractionalDigits = account.FractionalDigits,
            Notes = account.Notes
                .Select(x => x.Text),
            Tags = account.Tags
                .Select(x => x.Name)
                .ToList(),
        };
    }
}
