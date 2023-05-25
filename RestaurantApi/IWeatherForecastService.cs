using System.Collections.Generic;

namespace RestaurantApi
{
    public interface IWeatherForecastService
    {
        IEnumerable<WeatherForecast> Get();
        IEnumerable<WeatherForecast> Get(int resultsNumber, int minGrad, int maxGrad);
    }
}