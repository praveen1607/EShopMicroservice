//Below namespaces are commented out as they are not used in this file currently
// These are used for exception handling
//using Microsoft.AspNetCore.Diagnostics;
//using Microsoft.AspNetCore.Mvc; // Ensure this namespace is included for ProblemDetails

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(typeof(Program).Assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddValidatorsFromAssemblies(new[] { typeof(Program).Assembly });

builder.Services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();

if (builder.Environment.IsDevelopment())
{
    builder.Services.InitializeMartenWith<CatalogInitialData>();
}


builder.Services.AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapCarter();

//This is used for smaller applications and other ways to handle exceptions can be used for larger applications
//app.UseExceptionHandler(exceptionHandlerApp =>
//{
//    exceptionHandlerApp.Run(async context =>
//    {
//        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
//        if (exception == null)
//        {
//            return;
//        }
//        var problemDetails = new ProblemDetails // Corrected variable name to lowercase
//        {
//            Title = "An error occurred while processing your request.",
//            Status = 500,
//            Detail = exception.Message
//        };

//        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
//        logger.LogError(exception, exception.Message);
//        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
//        context.Response.ContentType = "application/json";
//        await context.Response.WriteAsJsonAsync(problemDetails);
//    });
//});

app.UseExceptionHandler(options => { });


app.Run();
