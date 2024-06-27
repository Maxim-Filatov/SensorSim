﻿using SensorSim.Domain.Interface;

namespace SensorSim.API.Convertors;

public class PrimaryConverter : IConverter
{
    public IStaticFunction StaticFunction { get; set; }

    public ISystematicError SystematicError { get; set; }

    public IRandomError RandomError { get; set; }

    public PrimaryConverter(IStaticFunction staticFunction, ISystematicError systematicError, IRandomError randomError)
    {
        StaticFunction = staticFunction;
        SystematicError = systematicError;
        RandomError = randomError;
    }

    public double Calculate(double value)
    {
        return StaticFunction.Calculate(value) + SystematicError.Emulate(value) + RandomError.Emulate(value);
    }
}