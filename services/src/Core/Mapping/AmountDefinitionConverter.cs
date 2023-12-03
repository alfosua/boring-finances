using AutoMapper;
using BoringFinances.Services.Contracts;
using BoringFinances.Services.Data;

namespace BoringFinances.Services.Core.Mapping;

public class AmountDefinitionConverter
    : ITypeConverter<AmountDefinitionInput, IAmountDefinition>
    , ITypeConverter<IAmountDefinition, AmountDefinitionInput>
{
    public IAmountDefinition Convert(AmountDefinitionInput source, IAmountDefinition destination, ResolutionContext context) => source switch
    {
        { FixedAmount: not null } => new FixedAmountDefinition(source.FixedAmount ?? 0),
        _ => throw new NotImplementedException("Amount definition input not supported."),
    };

    public AmountDefinitionInput Convert(IAmountDefinition source, AmountDefinitionInput destination, ResolutionContext context) => source switch
    {
        FixedAmountDefinition fad => new AmountDefinitionInput { FixedAmount = fad.Amount },
        _ => throw new NotImplementedException("Amount definition not supported."),
    };
}
