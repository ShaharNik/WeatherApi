using Microsoft.AspNetCore.Mvc;
using System.Collections;
using WeatherAssingment.Models;
using WeatherAssingment.Services;

namespace WeatherAssingment.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly AccuWeatherApiService _weathersServices;

        public WeatherController(AccuWeatherApiService weathersServices)
        {
            _weathersServices = weathersServices;
        }

        [HttpGet("getCityKeyByName/{cityName}")]
        public async Task<IEnumerable> GetCityKey(string cityName)
        {
            var cityKey = await _weathersServices.AutoComplete(cityName); 
            return cityKey; // List of founded City Names and thier Keys.

        }
        [HttpGet("getWeatherByCityKey/{cityKey}")]
        public async Task<WeatherDto> GetCurrentWeather(string cityKey)
        {
            var cityTempratureCelcius = await _weathersServices.GetCurrentWeather(cityKey);
            return cityTempratureCelcius;
        }
    }
}