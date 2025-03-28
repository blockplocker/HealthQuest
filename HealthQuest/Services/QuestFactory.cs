using HealthQuest.Models;
using System;
using System.Collections.Generic;

namespace HealthQuest.Services
{
    internal class QuestFactory : IQuestFactory
    {
        public Quest CreateQuest(string name, string description, int reps, int repsDone, string stat)
        {
            return new Quest
            {
                Id = Guid.NewGuid().ToString(),
                Name = name,
                Description = description,
                Reps = reps,
                RepsDone = repsDone,
                Stat = stat
            };
        }

        public DailyQuest CreateDailyQuest(Difficulty difficulty)
        {
            return new DailyQuest
            {
                Id = Guid.NewGuid().ToString(),
                Difficulty = difficulty,
                Pushups = 0,
                SitUps = 0,
                Squats = 0,
                Walk = 0,
                CurrentDay = DateTime.Today

            };
        }
    }
}
