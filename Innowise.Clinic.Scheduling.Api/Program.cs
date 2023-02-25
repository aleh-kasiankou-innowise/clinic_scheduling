using System.Text;
using Innowise.Clinic.Scheduling.Persistence;
using Innowise.Clinic.Scheduling.Services.Options;
using Innowise.Clinic.Scheduling.Services.ScheduleGenerationService.Implementations;
using Innowise.Clinic.Scheduling.Services.ScheduleGenerationService.Interfaces;
using Innowise.Clinic.Scheduling.Services.ScheduleService.Implementations;
using Innowise.Clinic.Scheduling.Services.ScheduleService.Interfaces;
using Innowise.Clinic.Scheduling.Services.ShiftService.Implementations;
using Innowise.Clinic.Scheduling.Services.ShiftService.Interfaces;
using Innowise.Clinic.Scheduling.Services.TimeSlotService.Implementations;
using Innowise.Clinic.Scheduling.Services.TimeSlotService.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opts =>
{
    opts.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,
        "Innowise.Clinic.Scheduling.Api.xml"));

    opts.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "JWT Authorization header using the Bearer scheme."
    });

    opts.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "bearerAuth" }
            },
            new string[] { }
        }
    });
});

builder.Services.AddDbContext<SchedulingDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<IScheduleGenerationService, PreferenceBasedScheduleGenerationService>();
builder.Services.AddScoped<IScheduleService, ScheduleService>();
builder.Services.AddScoped<IShiftService, ShiftService>();
builder.Services.AddScoped<ITimeSlotService, TimeSlotService>();

builder.Services.Configure<RouteOptions>(options => { options.LowercaseUrls = true; });
builder.Services.Configure<ScheduleGenerationConfigOptions>(builder.Configuration.GetSection("ScheduleGenerationConfiguration"));
builder.Services.Configure<WorkingDayConfigOptions>(
    builder.Configuration.GetSection("ScheduleConfiguration"));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            Environment.GetEnvironmentVariable("JWT__KEY") ?? throw new
                InvalidOperationException()))
    };
});



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

app.UseAuthorization();

app.MapControllers();

app.Run();