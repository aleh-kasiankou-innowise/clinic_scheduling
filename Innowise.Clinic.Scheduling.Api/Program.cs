using Hangfire;
using Innowise.Clinic.Scheduling.Api.Configuration;
using Innowise.Clinic.Scheduling.Persistence;
using Innowise.Clinic.Scheduling.Services.HangfireService;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger(builder.Configuration);
builder.Services.ConfigureDatabase(builder.Configuration);
builder.Services.ConfigureServices(builder.Configuration);
builder.Services.ConfigureTaskScheduler(builder.Configuration);
builder.Services.Configure<RouteOptions>(options => { options.LowercaseUrls = true; });

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

app.Run();