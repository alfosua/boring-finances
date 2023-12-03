using AutoMapper;
using BoringFinances.Services.Contracts;
using BoringFinances.Services.Data;

namespace BoringFinances.Services.Core.Mapping;

public class MainProfile : Profile
{
    public MainProfile()
    {
        CreateMap<AmountDefinitionInput, IAmountDefinition>().ConvertUsing<AmountDefinitionConverter>();
        CreateMap<BudgetInput, Budget>();
        CreateMap<BudgetItemInput, BudgetItem>();
        CreateMap<Budget, BudgetOutput>();
    }
}
