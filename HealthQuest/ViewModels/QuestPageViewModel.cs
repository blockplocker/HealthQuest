using HealthQuest.Models;
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

            Quests.Add(new Quest
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Pushups",
                Reps = 10,
                RepsDone = 0,
                Stat = "Strenght"
            });
            Quests.Add(new Quest
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Situps",
                Reps = 10,
                RepsDone = 0,
                Stat = "Agility"
            });
            Quests.Add(new Quest
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Plank 1 min",
                Reps = 1,
                RepsDone = 0,
                Stat = "Stamina"
            });
        }
    }
}
