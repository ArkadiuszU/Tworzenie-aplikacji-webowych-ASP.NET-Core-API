using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1
{
    public interface IWeatherForecastService
    {
        IEnumerable<WeatherForecast> Get(int min, int max, int number);

    }
}
