namespace BoringFinances.Operations.WebApi.FinancialUnits;

public static class FinancialUnitExtensions
{
    public static IServiceCollection AddFinancialUnits(this IServiceCollection services)
    {
        services.AddTransient<IFinancialUnitCruder, FinancialUnitCruder>();
        services.AddTransient<IFinancialUnitLister, FinancialUnitLister>();
        services.AddTransient<IFinancialUnitDtoMapper, FinancialUnitDtoMapper>();

        return services;
    }
}
