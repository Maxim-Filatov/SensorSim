﻿using Microsoft.Extensions.Logging;
using SensorSim.Domain.Interface;

namespace SensorSim.Domain;

public abstract class Sensor<T> : ISensor<T> where T : IPhysicalQuantity
{
    public ILogger<ISensor<T>> Logger { get; set; }

    public ISensorConfig<T> Config { get; set; }

    public T CurrentQuantity { get; set; }

    public PhysicalValueExposure Exposure { get; set; }

    public Sensor(ILogger<ISensor<T>> logger, ISensorConfig<T> config)
    {
        Logger = logger;
        Config = config;
        CurrentQuantity = config.InitialQuantity;
        Exposure = new PhysicalValueExposure(CurrentQuantity.Value, 1);
    }

    public double PrimaryConverter()
    {
        return CurrentQuantity.Value +
               Config.StaticFunction.Calculate(CurrentQuantity.Value) +
               Config.SystematicError.Calculate(CurrentQuantity.Value) +
               Config.RandomError.Calculate(CurrentQuantity.Value);
    }

    public double SecondaryConverter()
    {
        return Config.MotionFunction.Calculate(PrimaryConverter(), Exposure.Value, Exposure.Duration);
    }

    public T ReadQuantity()
    {
        return CurrentQuantity;
    }

    public T UpdateQuantity()
    {
        CurrentQuantity.Value = SecondaryConverter();
        return ReadQuantity();
    }

    public T SetQuantity(double value)
    {
        SetDirection(value, 1);
        CurrentQuantity.Value = value;
        return ReadQuantity();
    }

    public void SetDirection(double destination, double duration)
    {
        SetDirection(new PhysicalValueExposure
        {
            Value = destination,
            Duration = duration
        });
    }

    public void SetDirection(PhysicalValueExposure exposure)
    {
        Exposure = exposure;
    }

    public void Calibrate(List<double> values)
    {
        Config.StaticFunction.SetOptions(values);
    }
}