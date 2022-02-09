namespace BoringFinances.Operations.WebApi.Utilities
{
    public static class Extensions
    {
        public static IServiceCollection AddUtilities(this IServiceCollection services)
        {
            services.AddTransient<INoteDomainer, NoteDomainer>();
            services.AddTransient<IQueryDomainer, QueryDomainer>();
            services.AddTransient<ITagDomainer, TagDomainer>();
            services.AddTransient<ITemporalEntityDomainer, TemporalEntityDomainer>();

            return services;
        }
    }
}
