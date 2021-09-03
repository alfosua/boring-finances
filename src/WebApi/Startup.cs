using BoringSoftware.Finances.Core.Accounts;
using BoringSoftware.Finances.Core.FinancialUnits;
using BoringSoftware.Finances.Core.Operations;
using BoringSoftware.Finances.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace BoringSoftware.Finances.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string dbContextConnectionString = Configuration.GetConnectionString(nameof(BoringFinancesDbContext));

            services.AddCors(options => options.AddDefaultPolicy(policy => policy
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()));

            services.AddDbContext<BoringFinancesDbContext>(options => options.UseNpgsql(dbContextConnectionString));

            services.AddControllers();

            services.AddMemoryCache();

            services.AddTransient<IAccountCruder, AccountCruder>();
            services.AddTransient<IAccountMapper, AccountMapper>();
            services.AddTransient<IAccountResolver, AccountResolver>();
            services.AddTransient<IAccountTypeResolver, AccountTypeResolver>();
            
            services.AddTransient<IFinancialUnitCruder, FinancialUnitCruder>();
            services.AddTransient<IFinancialUnitMapper, FinancialUnitMapper>();
            services.AddTransient<IFinancialUnitResolver, FinancialUnitResolver>();
            services.AddTransient<IFinancialUnitTypeResolver, FinancialUnitTypeResolver>();

            services.AddTransient<IOperationCruder, OperationCruder>();
            services.AddTransient<IOperationEntryTypeResolver, OperationEntryTypeResolver>();
            services.AddTransient<IOperationMapper, OperationMapper>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BoringSoftware.Finances.WebApi", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BoringSoftware.Finances.WebApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
