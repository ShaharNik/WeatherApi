namespace WeatherAssingment.Models
{
    public class CityNameAndKey
    {
        public CityNameAndKey(string cityKey, string localizedName)
        {
            CityKey = cityKey;
            LocalizedName = localizedName;
        }

        public string CityKey { get; set; }
        public string LocalizedName { get; set; }
    }
}
