using BoringFinances.Operations.Data;
using BoringFinances.Operations.WebApi.Accounts;
using BoringFinances.Operations.WebApi.FinancialUnits;
using BoringFinances.Operations.WebApi.Operations;
using BoringFinances.Operations.WebApi.Utilities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var operationDbContextConnectionString = builder.Configuration.GetConnectionString(nameof(OperationDbContext));
var allowedOrigins = builder.Configuration.GetValue<string>("AllowedOrigins").Split(";");

builder.Services.AddCors(options => options
    .AddDefaultPolicy(x => x
        .WithOrigins(allowedOrigins)
        .AllowAnyHeader()
        .AllowAnyMethod()));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<OperationDbContext>(options => options.UseNpgsql(operationDbContextConnectionString));

builder.Services.AddAccounts();
builder.Services.AddOperations();
builder.Services.AddFinancialUnits();
builder.Services.AddUtilities();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
