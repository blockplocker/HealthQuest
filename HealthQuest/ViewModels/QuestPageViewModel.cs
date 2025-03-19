using HealthQuest.Models;
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
                Quests.Add(quest);
            }
        }
    }
}
