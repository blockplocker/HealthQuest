using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HealthQuest
{
    class WeatherService
    {
        public static async Task<Models.Weather> GetWeatherAsync(string uri)
        {
            var client = new HttpClient();

            client.BaseAddress = new Uri("https://api.api-ninjas.com/");
            client.DefaultRequestHeaders.Add("X-Api-Key", "DX/RKxnboDiBvoVX1r2ahQ==wq1LvBlp4blVsGxR");

            Models.Weather weather = null;

            HttpResponseMessage response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                string responseString = await response.Content.ReadAsStringAsync();
                weather = JsonSerializer.Deserialize<Models.Weather>(responseString);
            }

            return weather;
        }
    }
}
