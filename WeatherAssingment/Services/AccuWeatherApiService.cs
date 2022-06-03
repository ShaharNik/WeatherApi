using Newtonsoft.Json.Linq;
using System.Collections;
using WeatherAssingment.Models;

namespace WeatherAssingment.Services
{
    public class AccuWeatherApiService
    {
        public async Task<WeatherDto> GetCurrentWeather(string cityKey)
        {
            // Before calling to get current weather need to check if the current weather for current city exists in DB if it does get the weather from DB without calling AccuWeather API
            // (need to save only the Celsius Temperature value and WeatherText).
            //city key: 329424
            string API_KET = "WCGHsE7Qq82I5pdwPwmBLsijykfPjWLj"; // Rochester
            string baseURL = $"http://dataservice.accuweather.com/currentconditions/v1/{cityKey}?apikey={API_KET}";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseURL);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    using (HttpResponseMessage res = await client.GetAsync(baseURL))
                    {
                        using (HttpContent content = res.Content)
                        {
                            //var data = await content.ReadFromJsonAsync();
                            string data = await content.ReadAsStringAsync();
                            if (data != null)
                            {
                                JArray jsonArray = JArray.Parse(data);
                                dynamic dataObj = JObject.Parse(jsonArray[0].ToString());
                                if (dataObj != null)
                                {
                                    string weatherText = (string)dataObj["WeatherText"];
                                    string tempratureValue = (string)dataObj["Temperature"]["Metric"]["Value"];
                                    WeatherDto weatherAndTemprature = new WeatherDto(weatherText, tempratureValue);
                                    return weatherAndTemprature;
                                }
                                else
                                {
                                    return null;
                                }

                            }
                            else
                            {
                                //If data is null log it into console.
                                Console.WriteLine("Data is null!");
                                return null;
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return null;
            }
        }
        public async Task<IEnumerable> AutoComplete(string searchString) // https://developer.accuweather.com/accuweather-locations-api/apis/get/locations/v1/cities/autocomplete
        {
            string API_KET = "WCGHsE7Qq82I5pdwPwmBLsijykfPjWLj"; // Rochester
            string baseURL = $"http://dataservice.accuweather.com/locations/v1/cities/autocomplete?apikey={API_KET}&q={searchString}";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseURL);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    using (HttpResponseMessage res = await client.GetAsync(baseURL))
                    {
                        res.EnsureSuccessStatusCode();
                        using (HttpContent content = res.Content)
                        {
                            string data = await content.ReadAsStringAsync();
                            if (data != null)
                            {
                                ArrayList cityNamesList = new ArrayList();
                                JArray jsonArray = JArray.Parse(data);
                                foreach (JObject item in jsonArray) // <-- Note that here we used JObject instead of usual JProperty
                                {
                                    if (item != null)
                                    {
                                        var cityKey = item.GetValue("Key").ToString();
                                        string localizedName = item.GetValue("LocalizedName").ToString();
                                        CityNameAndKey keyAndName = new CityNameAndKey(cityKey, localizedName);
                                        cityNamesList.Add(keyAndName);
                                    }
                                }
                                return cityNamesList;
                            }
                            else
                            {
                                //If data is null log it into console.
                                Console.WriteLine("Data is null!");
                                return "Data is null!";
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return "Data is null!";
            }
        }
    }


}
