﻿using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using SensorSim.API.Config;
using SensorSim.API.Services;
using SensorSim.API.Services.Actuator;
using SensorSim.Domain;
using SensorSim.Domain.Interface;
using SensorSim.Infrastructure;
using SensorSim.Infrastructure.Repositories;

namespace SensorSim.API;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers()
            .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            );
        services.AddEndpointsApiExplorer();
        services.AddCors();

        services.AddSwaggerGen(options =>
        {
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "SensorSim.API", Version = "v1" });
        });

        services.AddDbContext<DataContext>(options =>
            options.UseSqlServer(
                _configuration.GetConnectionString("Database"),
                assembly => assembly.MigrationsAssembly("SensorSim.Domain.Migrations")
            )
        );

        services.AddSingleton<ISensorConfig<Temperature>, TemperatureSensorConfig>();
        services.AddSingleton<ISensor<Temperature>, TemperatureSensorService>();
        services.AddSingleton<IActuatorConfig<Temperature>, TemperatureActuatorConfig>();
        services.AddSingleton<IActuator<Temperature>, TemperatureActuatorService>();
        
        services.AddSingleton<ISensorConfig<Pressure>, PressureSensorConfig>();
        services.AddSingleton<ISensor<Pressure>, PressureSensorService>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
        );
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}