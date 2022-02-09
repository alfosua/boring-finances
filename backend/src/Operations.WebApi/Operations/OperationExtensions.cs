namespace BoringFinances.Operations.WebApi.Operations;

public static class OperationExtensions
{
    public static IServiceCollection AddOperations(this IServiceCollection services)
    {
        services.AddTransient<IOperationCruder, OperationCruder>();
        services.AddTransient<IOperationLister, OperationLister>();
        services.AddTransient<IOperationDomainer, OperationDomainer>();
        services.AddTransient<IOperationDtoMapper, OperationDtoMapper>();

        return services;
    }
}
