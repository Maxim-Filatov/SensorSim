﻿using Refit;
using SensorSim.Domain.DTO.Sensor;

namespace SensorSim.Actuator.API.Interface;

public interface ISensorApi
{
    /// <summary>
    /// Read the quantity of a sensor
    /// </summary>
    /// <param name="sensorId"></param>
    /// <returns></returns>
    [Get("/api/sensors/{sensorId}")]
    Task<GetSensorResponseModel> ReadQuantity(string sensorId);

    /// <summary>
    /// Set the quantity of a sensor
    /// </summary>
    /// <param name="sensorId"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [Post("/api/sensors/{sensorId}")]
    Task<SetSensorResponseModel> SetQuantity(string sensorId, [Body] SetSensorValueRequestModel request);

    /// <summary>
    /// Delete param
    /// </summary>
    /// <param name="actuatorId"></param>
    /// <returns></returns>
    [Delete("/api/sensors/{actuatorId}")]
    Task Delete(string actuatorId);
}