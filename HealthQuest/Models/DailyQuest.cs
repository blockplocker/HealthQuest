using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthQuest.Models
{
    class DailyQuest
    {
        public int Pushups { get; set; }
        public int SitUps { get; set; }
        public int Squats { get; set; }
        public int Walk { get; set; }
        public int UserId { get; set; }

        // Navigation property
        public User User { get; set; }
    }
}
