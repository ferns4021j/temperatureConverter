using TConverter.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace TConverter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TempController : ControllerBase
    {
        public class TempResponse
        {
            public short From { get; set; }
            public double Value { get; set; }
            public short To { get; set; }
            public string Message { get; set; }
        }

        private readonly ILogger<TempController> _logger;

        public TempController(ILogger<TempController> logger)
        {
            _logger = logger;
        }

        // Celsius to Fahrenheit Formula: (°C * 1.8) + 32 = °F
        // Fahrenheit to Celsius Formula: (°F - 32) / 1.8 = °C
        // °K = (F + 459.67) x 5/9
        // °K = °C + 273.15
        // °C = °K - 273.15
        // °F = °K * 9 / 5 -459.67
        // Absolute zero 0 K = −273.15° C = −459.67° F.

        /// <summary>
        /// Temperature Converter
        /// NOTE: Does not capture overflow exceptions.. :(
        /// </summary>
        /// <param name="from"></param>
        /// <param name="value"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("convert")]
        public TempResponse GetTemperature(TemperatureType from, double value, TemperatureType to)
        {
            _logger.LogDebug($"Temp Get Called {from}  {value} - {to}");
            var ret = new TempResponse
            {
                From = (short)from,
                To = (short)to,
            };
            if (from == to)
            {
                var rng = new Random();
                ret = InvalidParameter(from, to, rng.Next(-100, 100));
            }
            else
            {
                if (from == TemperatureType.Celsius)
                {
                    if (value < -273.15)
                    {
                        ret = InvalidParameter(from, to, value, "Value cannot be less than -273.15");
                    }
                    if (to == TemperatureType.Fahrenheit)
                    {
                        ret.Value = (value * 1.8) + 32;
                    }
                    else
                    {
                        ret.Value = value + 273.15;
                    }
                }
                if (from == TemperatureType.Fahrenheit)
                {
                    if (value < -459.67)
                    {
                        ret = InvalidParameter(from, to, value, "Value cannot be less than -459.67");
                    }
                    else
                    {
                        if (to == TemperatureType.Celsius)
                        {
                            ret.Value = (value - 32) / 1.8;
                        }
                        else
                        {
                            ret.Value = (value + 459.67) * 5.0 / 9.0;
                        }
                    }
                }
                if (from == TemperatureType.Kelvin)
                {
                    if (value < 0)
                    {
                        ret = InvalidParameter(from, to, value, "Value cannot be less than 0");
                    }
                    else
                    {
                        if (to == TemperatureType.Celsius)
                        {
                            ret.Value = value - 273.15;
                        }
                        else
                        {
                            ret.Value = (value * 9.0 / 5.0) - 459.67;
                        }
                    }
                }
            }
            ret.Value = Math.Round(ret.Value, 2);
            return ret;
        }

        private static TempResponse InvalidParameter(TemperatureType from, TemperatureType to, double rng, string msg = "Invalid Parameter")
        {
            return new TempResponse
            {
                From = (short)from,
                Value = rng,
                To = (short)to,
                Message = msg
            };
        }
    }
}
