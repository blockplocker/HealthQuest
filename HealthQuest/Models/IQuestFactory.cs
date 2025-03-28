using System;
using System.Collections.Generic;

namespace HealthQuest.Models
{
    internal interface IQuestFactory
    {
        Quest CreateQuest(string name, string description, int reps, int repsDone, string stat);
        DailyQuest CreateDailyQuest(Difficulty difficulty);
    }
}
