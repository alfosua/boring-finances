namespace BoringFinances.Operations.WebApi.Accounts;

public static class AccountExtensions
{
    public static IServiceCollection AddAccounts(this IServiceCollection services)
    {
        services.AddTransient<IAccountCruder, AccountCruder>();
        services.AddTransient<IAccountLister, AccountLister>();
        services.AddTransient<IAccountDtoMapper, AccountDtoMapper>();

        return services;
    }
}
