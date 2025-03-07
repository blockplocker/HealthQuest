using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthQuest.Models
{
    class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Hp { get; set; }
        public int Stamina { get; set; }
        public int Strenght { get; set; }
        public int Agility { get; set; }
        public int Vigor { get; set; }

        // Navigation properties
        public DailyQuest DailyQuest { get; set; }
        public ICollection<Quest> Quests { get; set; }
    }
}
