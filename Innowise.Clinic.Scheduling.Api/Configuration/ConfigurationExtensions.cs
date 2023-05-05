using System.Text;
using Hangfire;
using Innowise.Clinic.Scheduling.Persistence;
using Innowise.Clinic.Scheduling.Persistence.Repositories.Implementations;
using Innowise.Clinic.Scheduling.Persistence.Repositories.Interfaces;
using Innowise.Clinic.Scheduling.Services.HangfireHelper;
using Innowise.Clinic.Scheduling.Services.MassTransitService.Consumers;
using Innowise.Clinic.Scheduling.Services.Options;
using Innowise.Clinic.Scheduling.Services.ScheduleGenerationService.Implementations;
using Innowise.Clinic.Scheduling.Services.ScheduleGenerationService.Interfaces;
using Innowise.Clinic.Scheduling.Services.ScheduleService.Implementations;
using Innowise.Clinic.Scheduling.Services.ScheduleService.Interfaces;
using Innowise.Clinic.Scheduling.Services.ShiftService.Implementations;
using Innowise.Clinic.Scheduling.Services.ShiftService.Interfaces;
using Innowise.Clinic.Scheduling.Services.TimeSlotService.Implementations;
using Innowise.Clinic.Scheduling.Services.TimeSlotService.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace Innowise.Clinic.Scheduling.Api.Configuration;

public static class ConfigurationExtensions
{
    public static IServiceCollection ConfigureCrossServiceCommunication(this IServiceCollection services,
        IConfiguration configuration)
    {
        var rabbitMqConfig = configuration.GetSection("RabbitConfigurations");
        services.AddMassTransit(x =>
        {
            x.AddConsumer<TimeSlotReservationRequestConsumer>();
            x.AddConsumer<TimeSlotUpdateRequestConsumer>();
            x.AddConsumer<DoctorChangesSchedulingConsumer>();
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(rabbitMqConfig["HostName"], h =>
                {
                    h.Username(rabbitMqConfig["UserName"]);
                    h.Password(rabbitMqConfig["Password"]);
                });
                cfg.ConfigureEndpoints(context);
            });
        });
        return services;
    }

    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ScheduleGenerationConfigOptions>(
            configuration.GetSection("ScheduleGenerationConfiguration"));
        services.Configure<WorkingDayConfigOptions>(
            configuration.GetSection("ScheduleConfiguration"));

        services.AddSingleton<HangfireHelper>();
        services.AddScoped<IScheduleGenerationService, PreferenceBasedScheduleGenerationService>();
        services.AddScoped<IScheduleService, ScheduleService>();
        services.AddScoped<IShiftService, ShiftService>();
        services.AddScoped<ITimeSlotService, TimeSlotService>();
        return services;
    }

    public static IServiceCollection ConfigureRepositories(this IServiceCollection services)
    {
        services.AddScoped<IDoctorRepository, DoctorRepository>();
        return services;
    }

    public static IServiceCollection ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<SchedulingDbContext>(opt =>
            opt.UseSqlServer(configuration.GetConnectionString("Default")));
        return services;
    }

    public static IServiceCollection ConfigureTaskScheduler(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHangfire(x => x.UseSqlServerStorage(configuration.GetConnectionString("Default")));
        services.AddHangfireServer();
        return services;
    }

    public static IServiceCollection ConfigureSecurity(this IServiceCollection services)
    {
        services.AddAuthentication(options =>
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
                        InvalidOperationException("The JWT encryption key is not added to environmental variables.")))
            };
        });
        return services;
    }

    public static IServiceCollection ConfigureSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwaggerGen(opts =>
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
        return services;
    }
    
    public static WebApplicationBuilder ConfigureSerilog(this WebApplicationBuilder builder)
    {
        var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.FromLogContext()
            .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(builder.Configuration["ElasticSearchHost"]))
            {
                AutoRegisterTemplate = true,
                OverwriteTemplate = true,
                IndexFormat = $"clinic.scheduling-{0:yy.MM}",
                BatchAction = ElasticOpType.Index,
                DetectElasticsearchVersion = true,
            })
            .WriteTo.Console()
            .CreateLogger();

        Log.Logger = logger;
        builder.Host.UseSerilog(logger);
        return builder;
    }
}