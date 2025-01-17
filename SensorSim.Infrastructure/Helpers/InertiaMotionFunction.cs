﻿using SensorSim.Domain.Interface;

namespace SensorSim.Infrastructure.Helpers;

public class InertiaMotionFunction(double rateOfChange) : IMotionFunction
{
    private double RateOfChange { get; } = rateOfChange;

    public double Calculate(double value)
    {
        return Calculate(value, value, 1.0);
    }
    
    public double Calculate(double value, double destination, double speed)
    {
        var difference = destination - value; 
        var step = RateOfChange * speed;

        if (Math.Abs(difference) > step)
        {
            return value + Math.Sign(difference) * step;
        }
        
        return destination;
    }
}