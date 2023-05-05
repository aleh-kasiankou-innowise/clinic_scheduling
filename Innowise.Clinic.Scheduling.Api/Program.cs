using Hangfire;
using Innowise.Clinic.Scheduling.Api.Configuration;
using Innowise.Clinic.Scheduling.Persistence;
using Innowise.Clinic.Scheduling.Services.HangfireService;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSecurity();
builder.Services.ConfigureSwagger(builder.Configuration);
builder.Services.ConfigureDatabase(builder.Configuration);
builder.Services.ConfigureRepositories();
builder.Services.ConfigureServices(builder.Configuration);
builder.Services.ConfigureTaskScheduler(builder.Configuration);
builder.Services.ConfigureCrossServiceCommunication(builder.Configuration);
builder.Services.Configure<RouteOptions>(options => { options.LowercaseUrls = true; });
builder.ConfigureSerilog();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<SchedulingDbContext>();
    if ((await context.Database.GetPendingMigrationsAsync()).Any()) await context.Database.MigrateAsync();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = new[] { new AllowGuestAccessToDashboardFilter() }
});
await app.ConfigureBackgroundScheduleGeneration();

app.UseAuthorization();

app.MapControllers();

Log.Information("The Scheduling service is starting");
app.Run();
Log.Information("The scheduling service is stopping");
await Log.CloseAndFlushAsync();