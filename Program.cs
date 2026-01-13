using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("dev", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors("dev");

app.MapGet("/", () => Results.Ok(new { message = "Calculator API" }));

app.MapPost("/api/calc", ([FromBody] CalcRequest request) =>
{
    if (request.Op is null)
    {
        return Results.BadRequest(new CalcResponse(null, "Operation is required."));
    }

    var op = request.Op.Trim();

    if (op.Length == 0)
    {
        return Results.BadRequest(new CalcResponse(null, "Operation is required."));
    }

    if (op == "/" && request.B == 0)
    {
        return Results.BadRequest(new CalcResponse(null, "Division by zero is not allowed."));
    }

    double result;

    switch (op)
    {
        case "+":
            result = request.A + request.B;
            break;
        case "-":
            result = request.A - request.B;
            break;
        case "*":
            result = request.A * request.B;
            break;
        case "/":
            result = request.A / request.B;
            break;
        default:
            return Results.BadRequest(new CalcResponse(null, "Unsupported operation."));
    }

    return Results.Ok(new CalcResponse(result, null));
});

app.Run();

record CalcRequest(double A, double B, string Op);
record CalcResponse(double? Result, string? Error);
