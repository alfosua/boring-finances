using AutoMapper;
using BoringFinances.Services.Contracts;
using BoringFinances.Services.Core.Mapping;
using BoringFinances.Services.Data;
using Marten;
using Microsoft.AspNetCore.Mvc;
using Weasel.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetConnectionString("Marten")!);

    if (builder.Environment.IsDevelopment())
    {
        options.AutoCreateSchemaObjects = AutoCreate.All;
    }
});

builder.Services.AddAutoMapper(typeof(MainProfile));

var app = builder.Build();

app.Use(async (context, next) =>
{
    try
    {
        await next(context);
    }
    catch (Exception ex)
    {
        var response = ApiResponse.CreateError(new ErrorResponseItem[]
        {
            new()
            {
                Message = ex.Message,
                Exception = ex.GetType().Name,
                StackTrace = ex.StackTrace,
                InnerMessage = ex.InnerException?.Message,
                InnerException = ex.InnerException?.GetType().Name,
                InnerStackTrace = ex.InnerException?.StackTrace,
            },
        });

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        await context.Response.WriteAsJsonAsync(response);
    }
});

app.MapGet("/pulse", () => "Hello World!");

app.MapPost("/budgets", async (BudgetInput input, [FromServices] IDocumentSession session, [FromServices] IMapper mapper) =>
{
    var budget = mapper.Map<Budget>(input);

    session.Insert(budget);

    await session.SaveChangesAsync();

    var result = mapper.Map<BudgetOutput>(budget);

    var response = ApiResponse.CreateSingle(result);

    return TypedResults.Created($"/budgets/{budget.Id}", budget);
});

app.MapGet("/budgets/{id:guid}", async (Guid id, [FromServices] IQuerySession session, [FromServices] IMapper mapper) =>
{
    var budget = await session.LoadAsync<Budget>(id)
        ?? throw new KeyNotFoundException("The given budget was not found");

    var result = mapper.Map<BudgetOutput>(budget);

    var response = ApiResponse.CreateSingle(result);

    return TypedResults.Ok(response);
});

app.MapPut("/budgets/{id:guid}", async (Guid id, BudgetInput input, [FromServices] IDocumentSession session, [FromServices] IMapper mapper) =>
{
    var budget = await session.LoadAsync<Budget>(id)
        ?? throw new KeyNotFoundException("The given budget was not found");

    mapper.Map(input, budget);

    session.Update(budget);

    await session.SaveChangesAsync();

    var result = mapper.Map<BudgetOutput>(budget);

    var response = ApiResponse.CreateSingle(result);

    return TypedResults.Ok(response);
});

app.MapDelete("/budgets/{id:guid}", async (Guid id, [FromServices] IDocumentSession session) =>
{
    var budget = await session.LoadAsync<Budget>(id)
        ?? throw new KeyNotFoundException("The given budget was not found");

    session.Delete(budget);

    await session.SaveChangesAsync();

    return TypedResults.Ok(ApiResponse.CreateBasic());
});

app.MapGet("/budgets", async ([FromServices] IQuerySession session, [FromServices] IMapper mapper) =>
{
    var budgets = await session.Query<Budget>().ToListAsync();

    var result = budgets.Select(x => mapper.Map<BudgetOutput>(x));

    var response = ApiResponse.CreateArray(result);

    return TypedResults.Ok(response);
});

app.Run();
