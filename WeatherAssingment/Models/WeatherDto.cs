namespace WeatherAssingment.Models
{
    public class WeatherDto
    {
        private string tempratureValue;

        public WeatherDto(string weatherText, string tempratureValue)
        {
            WeatherText = weatherText;
            TempratureValueCelsius = tempratureValue;
        }

        public string WeatherText { get; set; }
        public string TempratureValueCelsius { get; set; }
    }
}
