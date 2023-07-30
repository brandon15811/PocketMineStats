using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PocketMineStats.Data;
using PocketMineStats.Profiles;
using PocketMineStats.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationInsightsTelemetry();

builder.Services.AddControllersWithViews().AddNewtonsoftJson().ConfigureApiBehaviorOptions(LogModelStateErrors);

var connectionString = builder.Configuration.GetConnectionString("StatsContext");

builder.Services.AddDbContext<StatsContext>(dbContextOptions => dbContextOptions
    .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), x => x.UseNewtonsoftJson()));

builder.Services.AddTransient<UpdateStatsService>();
builder.Services.AddHostedService<UpdateStatsBackgroundService>();

builder.Services.AddSingleton(_ =>
{
    var configuration = new MapperConfiguration(cfg =>
    {
        cfg.AddCollectionMappers();
        cfg.AddProfile(new MappingProfile());
    });
#if DEBUG
    configuration.AssertConfigurationIsValid();
#endif
    return configuration.CreateMapper();
});


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<StatsContext>();
    db.Database.Migrate();
}


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseWhen(
    ctx => ctx.Request.Path.StartsWithSegments("/api/post"),
    ab => ab.Use(next => context => {
        //Allow reading of raw request body later
        context.Request.EnableBuffering();
        return next(context);
    })
);

app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

void LogModelStateErrors(ApiBehaviorOptions options)
{
    // To preserve the default behavior, capture the original delegate to call later.
    var invalidModelStateResponseFactory = options.InvalidModelStateResponseFactory;
    options.InvalidModelStateResponseFactory = context =>
    {
        try
        {
            var modelErrors = context.ModelState.Values.SelectMany(state => state.Errors).Select(e => e.ErrorMessage).ToList();
            if (modelErrors.Any())
            {
                var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                logger.LogError("Model state error: {error}", string.Join(Environment.NewLine, modelErrors));
                try
                {
                    if (context.HttpContext.Features.Get<IHttpBodyControlFeature>() is IHttpBodyControlFeature feature)
                    {
                        feature.AllowSynchronousIO = true;
                    }
                    var reqBody = context.HttpContext.Request.Body;
                    if (reqBody.CanSeek) { reqBody.Position = 0; }
                    var sr = new StreamReader(reqBody);
                    var body = sr.ReadToEnd();
                    logger.LogError("Model state error body: {body}", body);
    
                } catch (Exception e)
                {
                    logger.LogError(e, "Error reading request body while logging model state error");
                }
            }
        }
        catch (Exception ex)
        {
            try
            {
                var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "Failed to log model state error");
            } catch { }
        }
        // Invoke the default behavior, which produces a ValidationProblemDetails response.
        // To produce a custom response, return a different implementation of IActionResult instead.
        return invalidModelStateResponseFactory(context);
    };
}