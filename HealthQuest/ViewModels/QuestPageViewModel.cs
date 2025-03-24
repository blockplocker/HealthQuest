using HealthQuest.Models;
using HealthQuest.Services;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthQuest.ViewModels
{
    internal class QuestPageViewModel
    {
        public ObservableCollection<Quest> Quests { get; set; }

        public QuestPageViewModel()
        {
            Quests = new ObservableCollection<Quest>();
            LoadQuestsAsync();
        }

        private async Task LoadQuestsAsync()
        {
            var questCollection = Data.DB.GetQuestCollection();
            var quests = await questCollection.Find(_ => true).ToListAsync();
            foreach (var quest in quests)
            {
                if (quest.DayCompleted?.ToUniversalTime().Date != DateTime.UtcNow.Date)
                {
                    quest.ResetQuest();
                    await Data.DB.ReplaceQuestAsync(quest);
                }
                Quests.Add(quest);
            }
        }

        public async Task UpdateStatsAsync(Quest quest)
        {
            var stats = StatsManager.Instance.Stats;

            switch (quest.Stat)
            {
                case "Strenght":
                    stats.Strenght += 1;
                    break;
                case "Agility":
                    stats.Agility += 1;
                    break;
                case "Vigor":
                    stats.Vigor += 1;
                    break;
                case "Stamina":
                    stats.Stamina += 1;
                    break;
                default:
                    throw new ArgumentException("Invalid stat type");
            }

            await StatsManager.Instance.SaveStatsAsync();
        }
    }
}
