using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HealthQuest.ViewModels
{
    internal class AddExercisePageViewModel
    {
        public Models.Exercise Exercises { get; set; }
        public AddExercisePageViewModel()
        {
            var task = Task.Run(() => GetUserAsync());
            task.Wait();
            Exercises = task.Result;
        }
        
        public static async Task<Models.Exercise> GetUserAsync()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://api.api-ninjas.com/");
            client.DefaultRequestHeaders.Add("X-Api-Key", "DX/RKxnboDiBvoVX1r2ahQ==wq1LvBlp4blVsGxR");
            Models.Exercise Exercises = null;
            HttpResponseMessage response = await client.GetAsync("v1/exercises");
            if (response.IsSuccessStatusCode)
            {
                string responseString = await response.Content.ReadAsStringAsync();
                Exercises = JsonSerializer.Deserialize<Models.Exercise>(responseString);
            }
            return Exercises;
        }
    }
}
