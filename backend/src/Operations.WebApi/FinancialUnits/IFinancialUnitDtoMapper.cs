using BoringFinances.Operations.Data.FinancialUnits;
using System.Linq.Expressions;

namespace BoringFinances.Operations.WebApi.FinancialUnits;

public interface IFinancialUnitDtoMapper
{
    Expression<Func<FinancialUnit, FinancialUnitDto>> SelectExpression { get; }

    FinancialUnitDto CreateFinancialUnitDtoFrom(FinancialUnit account);
}
