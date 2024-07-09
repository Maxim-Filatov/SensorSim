﻿using SensorSim.API.Helpers;
using SensorSim.Domain;
using SensorSim.Domain.Interface;

namespace SensorSim.API.Config;

public class TemperatureActuatorConfig : IActuatorConfig<Temperature>
{
    public Temperature InitialQuantity { get; } = new (25.0);

    public IMotionFunction MotionFunction { get; } = new InertiaMotionFunction(1.0);
}